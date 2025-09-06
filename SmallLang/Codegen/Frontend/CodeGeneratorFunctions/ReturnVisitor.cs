using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class ReturnVisitor
{
    internal static void Visit(ReturnNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Driver.Cast(Self.Expression, Self.ExpectedReturnType!);
            var reg = Driver.GetRegisters().Single();
            Driver.Emit(HighLevelOperation.LoadFromStack(reg, Self.ExpectedReturnType!.Size));
            Driver.Emit(HighLevelOperation.Return(reg, Self.ExpectedReturnType!.Size));

            Driver.Next();
        });
    }
}