using Common.LinearIR;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class SwitchVisitor
{
    public static void Visit(SwitchNode Self, CodeGenerator Driver)
    {

        var Expressions = Self.ExprStatementCombineds.Select(x => x.Expression1).ToList();
        var Statements = Self.ExprStatementCombineds.Select(x => x.Statement1).ToList();
        var Length = Self.ExprStatementCombineds.Count();
        var ExpressionType = Self.Expression.TypeOfExpression!;
        List<int> Registers = [Driver.GetRegisters((int)ExpressionType.Size).First(), .. Expressions.Select(_ => Driver.GetRegisters((int)ExpressionType.Size).First())];

        Driver.EnteringChunk(() =>
        {
            Driver.Exec(Self.Expression);
            Driver.Emit(HighLevelOperation.LoadFromStack(Registers[0], ExpressionType.Size));
            Driver.Emit(HighLevelOperation.Switch<int, int, int, byte>(Registers[0], Length, 1, ExpressionType));
        });

        for (int i = 1; i <= Length; i++)
        {
            Driver.NewChunk(i * 2 - 1, () =>
            {
                Driver.Cast(Expressions[i - 1], ExpressionType);
                Driver.Emit(HighLevelOperation.LoadFromStack(Registers[i], ExpressionType.Size));
            });

            Driver.NewChunk(i * 2, () =>
            {
                Driver.Exec(Statements[i - 1]);
            });
        }

        Driver.NewChunk(Length * 2 + 1, () =>
        {
            Driver.Next();
        });


    }
}