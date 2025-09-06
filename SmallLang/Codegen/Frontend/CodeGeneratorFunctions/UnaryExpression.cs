using Common.Dispatchers;
using Common.LinearIR;
using Common.Tokens;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using ArgumentType = NumberWrapper<int, BackingNumberType>;
using TypeType = NumberWrapper<byte, BackingNumberType>;

internal static class UnaryExpressionVisitor
{
    internal static void Visit(UnaryExpressionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            var SrcRegister = Driver.GetRegisters(Self).First();

            var DstRegister = Driver.GetRegisters(Self).First();

            Driver.Emit(Self.Data.TT.Map<TokenType, Func<ArgumentType, ArgumentType, TypeType, HighLevelOperation>>
            (
                (TokenType.BitwiseNegation, HighLevelOperation.BitwiseNegation),
                (TokenType.Subtraction, HighLevelOperation.UnarySubtraction),
                (TokenType.LogicalNot, HighLevelOperation.LogicalNot)
            )(SrcRegister, DstRegister, Self.TypeOfExpression!));

            Driver.Next();
        });
    }
}