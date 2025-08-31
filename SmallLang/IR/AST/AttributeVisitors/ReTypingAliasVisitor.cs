using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(ReTypingAliasNode node)
    {
        node.TypeOfExpression = node.Type.TypeLiteralType;
        DoIfNotNull(() => node.Scope!.Define(node.Identifier.VariableName!, node.Type.TypeLiteralType!), node.Identifier.VariableName, node.Scope, node.Type.TypeLiteralType);
    }
}