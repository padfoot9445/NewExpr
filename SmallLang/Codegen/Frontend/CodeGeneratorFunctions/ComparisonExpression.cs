using System.Diagnostics;
using Common.Dispatchers;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using ArgumentType = Common.LinearIR.NumberWrapper<int, BackingNumberType>;
using TypeType = Common.LinearIR.NumberWrapper<byte, BackingNumberType>;

internal static class ComparisonExpressionVisitor
{
    internal static void Visit(ComparisonExpressionNode Self, CodeGenerator Driver)
    {
        var GreatestCommonType = Self.OperatorExpressionPairs.Aggregate(Self.Expression.GenericSLType,
            (x, y) => x!.GreatestCommonType(y.Expression.GenericSLType!));

        Debug.Assert(GreatestCommonType is not null);
        List<IExpressionNode> Expressions = [Self.Expression, .. Self.OperatorExpressionPairs.Select(x => x.Expression)];
        List<TokenType> Operators = [.. Self.OperatorExpressionPairs.Select(x => x.Data.TT)];
        var ExpressionRegister = Driver.GetRegisters(GreatestCommonType).First();
        List<int> Registers = [ExpressionRegister, .. Expressions.Select(_ => Driver.GetRegisters(GreatestCommonType!).First())];
        var Width = GreatestCommonType.Size;

        var ResultRegisters = Enumerable.Range(0, Operators.Count).Select(_ => Driver.GetRegisters(TypeData.Bool).First()).ToList();

        var ResultRegister = Driver.GetRegisters(TypeData.Bool).First();


        Driver.EnteringChunk(() =>
        {
            //exploit the fact that in x == y == z => (x == y) and (y == z) y must be loaded in the second expression
            //
            Driver.Cast(Self.Expression, GreatestCommonType);
            Driver.Emit(HighLevelOperation.LoadFromStack(ExpressionRegister, Width));
            Driver.Emit(HighLevelOperation.BatchAnd(1, Expressions.Count, ResultRegister));
            Driver.Emit(HighLevelOperation.PushFromRegister(ResultRegister, TypeData.Bool.Size));
        });

        for (int i = 1; i < Expressions.Count; i++)
        {
            Driver.NewChunk(i, () =>
            {
                Driver.Cast(Expressions[i], GreatestCommonType);
                Driver.Emit(HighLevelOperation.LoadFromStack(Registers[i], Width));

                Driver.Emit(
                    Operators[i - 1].Map<TokenType, Func<ArgumentType, ArgumentType, ArgumentType, TypeType, HighLevelOperation>>(
                    (TokenType.EqualTo, HighLevelOperation.EqualTo),
                    (TokenType.NotEqualTo, HighLevelOperation.NotEqualTo),
                    (TokenType.GreaterThan, HighLevelOperation.GreaterThan),
                    (TokenType.GreaterThanOrEqualTo, HighLevelOperation.GreaterThanOrEqualTo),
                    (TokenType.LessThan, HighLevelOperation.LessThan),
                    (TokenType.LessThanOrEqualTo, HighLevelOperation.LessThanOrEqualTo)
                    )(Registers[i - 1], Registers[i], ResultRegisters[i], GreatestCommonType)
                );
            });
        }
    }
}