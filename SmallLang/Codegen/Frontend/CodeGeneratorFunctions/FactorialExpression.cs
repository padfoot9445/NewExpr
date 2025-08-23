using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryVisitorSubFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;


internal static class FactorialExpressionVisitor
{
    internal static void Visit(FactorialExpressionNode Self, CodeGenerator Driver)
    {

        Driver.EnteringChunk(() =>
        {


            var register = Driver.GetRegisters(Self).First();
            var register2 = Driver.GetRegisters(Self).First();
            int Width = (int)Self.Attributes.TypeOfExpression!.Size;

            Driver.Exec(Self.Expression1);
            Driver.Emit(HighLevelOperation.LoadFromStack(register, Width));
            Driver.Emit(HighLevelOperation.Factorial<int, int, byte, int>(register, register2, Self.Attributes.TypeOfExpression, Self.FactorialSymbols1.Count()));
            Driver.Next();
        });

    }
}