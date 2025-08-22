using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionVisitor
{
    public static void Visit(FunctionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            //assume that Entering chunk does not have JMP CHUNK1
        });

        Driver.NewChunk(1, () =>
        {
            Driver.Exec(Self.Statement1);
        });

        Driver.NewChunk(2, () =>
        {
            Driver.Next();
        });
    }
}