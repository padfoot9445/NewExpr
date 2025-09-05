using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class ExpectedTypeOfExpressionVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        AssertPropertyIsNotNull<IHasAttributeGenericSLType>(x => x.GenericSLType is not null);
        base.PreVisit(node);
    }

    protected override ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self)
    {
        NotNull(self.Expression1.GenericSLType);
        self.ExpectedTypeOfExpression = self.Expression1.GenericSLType.ChildNodes.Last();
        return base.VisitIndex(Parent, self);
    }
}