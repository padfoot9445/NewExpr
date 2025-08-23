using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class SectionVisitor
{
    public static void Visit(SectionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {

            foreach (var child in Self.Statements)
            {
                Driver.Exec(child);
            }
            Driver.Next();
        });
    }
}