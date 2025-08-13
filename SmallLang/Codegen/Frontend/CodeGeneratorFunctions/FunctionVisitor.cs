using SmallLang.IR.AST;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        Driver.Verify(Self, ImportantASTNodeType.Function);
        //assume that Entering chunk does not have JMP CHUNK1

        //CHUNK1
        Driver.Data.Sections.NewChunk();
        Driver.DynamicDispatch(Self.Children.Last());
    }
}