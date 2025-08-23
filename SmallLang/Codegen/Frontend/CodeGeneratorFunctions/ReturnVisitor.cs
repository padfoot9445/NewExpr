using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;



internal static class ReturnVisitor
{
    internal static void Visit(ReturnNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Driver.Cast(Self.Expression, Self.ExpectedReturnType!);
            var reg = Driver.GetRegisters(1).Single();
            Driver.Emit(HighLevelOperation.LoadFromStack(reg, Self.ExpectedReturnType!.Size));
            Driver.Emit(HighLevelOperation.Return(reg, Self.ExpectedReturnType!.Size));

            Driver.Next();
        });
    }
}