using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class NewExpressionVisitor
{
    internal static void Visit(NewExprNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            FunctionCallVisitor.CallFunction(Self, Self.ArgList, Driver);
            Driver.Next();
        });
    }
}