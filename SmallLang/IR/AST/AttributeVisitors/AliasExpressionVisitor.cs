using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(AliasExprNode node)
    {
        DoIfNotNull(() => node.Scope!.Define(node.Identifier2.VariableName!, node.Identifier1.TypeOfExpression!), node.Scope, node.Identifier1.TypeOfExpression, node.Identifier2.VariableName);
        DoIfNotNull(() => node.TypeOfExpression = node.Identifier1.TypeOfExpression, node.Identifier1.TypeOfExpression);
    }
}