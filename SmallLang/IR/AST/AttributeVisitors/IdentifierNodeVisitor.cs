using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(IdentifierNode node)
    {
        DoIfNotNull(() => node.VariableName = node.Scope!.GetName(node.Data.Lexeme), node.Scope);
        DoIfNotNull(() => node.TypeOfExpression = node.Scope!.TypeOf(node.VariableName!), node.Scope, node.VariableName);
    }
}