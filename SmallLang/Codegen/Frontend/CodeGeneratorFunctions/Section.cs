namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator
{
    private void ParseSection(Node Self)
    {
        Verify(Self, ImportantASTNodeType.Section);
        foreach (var child in Self.Children)
        {
            DynamicDispatch(child);
        }
    }
}