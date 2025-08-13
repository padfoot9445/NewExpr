using SmallLang.IR.AST;

namespace SmallLang.CodeGen.Frontend;

internal static class Section
{
    public static void ParseSection(Node Self, CodeGenerator Driver)
    {
        Driver.Verify(Self, ImportantASTNodeType.Section);
        foreach (var child in Self.Children)
        {
            Driver.DynamicDispatch(child);
        }
    }
}