using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(FunctionNode node)
    {
        DoIfNotNull(() => node.VariableName = node.Scope!.GetName(node.Data.Lexeme), node.Scope);
        DoIfNotNull(() => node.Scope!.Define(node.VariableName!), node.VariableName, node.Scope);
        node.FunctionBody.Scope = new Scope { Parent = node.Scope };
    }
}