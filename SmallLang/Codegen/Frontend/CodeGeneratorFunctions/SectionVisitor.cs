using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class SectionVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.Verify(Self, ImportantASTNodeType.Section);
        foreach (var child in Self.Children)
        {
            Driver.Exec(child);
        }
    }
}