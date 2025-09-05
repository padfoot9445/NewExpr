using SmallLang.IR.AST.Generated;

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