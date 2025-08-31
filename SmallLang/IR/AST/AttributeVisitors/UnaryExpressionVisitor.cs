using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(UnaryExpressionNode node)
    {
        node.TypeOfExpression = node.Expression.TypeOfExpression;
    }
}