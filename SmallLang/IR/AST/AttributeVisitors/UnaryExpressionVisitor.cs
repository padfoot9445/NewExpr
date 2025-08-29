using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(UnaryExpressionNode node)
    {
        node.ExpectedTypeOfExpression = node.Expression.ExpectedTypeOfExpression;
        node.TypeOfExpression = node.Expression.TypeOfExpression;
    }
}