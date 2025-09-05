using SmallLang.IR.AST.Generated;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class ElseVisitor
{
    internal static void Visit(ElseNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() => { Driver.Exec(Self.Section); });
    }
}