using SmallLang.IR.AST;

namespace SmallLang.CodeGen.Frontend;

internal static class SectionVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        Driver.Verify(Self, ImportantASTNodeType.Section);
        foreach (var child in Self.Children)
        {
            Driver.DynamicDispatch(child);
        }
    }
}