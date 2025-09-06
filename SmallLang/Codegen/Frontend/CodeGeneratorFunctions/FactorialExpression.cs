using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FactorialExpressionVisitor
{
    internal static void Visit(FactorialExpressionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            var register = Driver.GetRegisters(Self).First();
            var register2 = Driver.GetRegisters(Self).First();
            var Width = (int)Self.TypeOfExpression!.Size;

            Driver.Exec(Self.Expression);
            Driver.Emit(HighLevelOperation.LoadFromStack(register, Width));
            Driver.Emit(HighLevelOperation.Factorial<int, int, byte, int>(register, register2, Self.TypeOfExpression,
                Self.FactorialSymbols.Count()));
            Driver.Next();
        });
    }
}