using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(ReTypeOriginalNode node)
    {
        DoIfNotNull(() => node.TypeOfExpression = node.Type.TypeLiteralType, node.Type.TypeLiteralType);
    }
}