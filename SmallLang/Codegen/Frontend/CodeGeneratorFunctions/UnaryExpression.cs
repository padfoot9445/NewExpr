using Common.Dispatchers;
using Common.Tokens;
using sly.lexer;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using ArgumentType = Common.LinearIR.NumberWrapper<int, BackingNumberType>;
using TypeType = Common.LinearIR.NumberWrapper<byte, BackingNumberType>;

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
                (TokenType.Subtraction, HighLevelOperation.Subtraction),
                (TokenType.LogicalNot, HighLevelOperation.LogicalNot)
            )(SrcRegister, DstRegister, Self.TypeOfExpression!));

            Driver.Next();
        });
    }
}