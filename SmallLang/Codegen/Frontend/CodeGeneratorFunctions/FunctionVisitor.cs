using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.Verify(Self, ImportantASTNodeType.Function);
        //assume that Entering chunk does not have JMP CHUNK1

        //CHUNK1
        Driver.Data.Sections.NewChunk();
        Driver.Exec(Self.Children.Last());
    }
}