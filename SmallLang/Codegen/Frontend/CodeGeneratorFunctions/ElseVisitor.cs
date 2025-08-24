using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;



internal static class ElseVisitor
{
    internal static void Visit(ElseNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Driver.Exec(Self.Statement);
        });
    }
}