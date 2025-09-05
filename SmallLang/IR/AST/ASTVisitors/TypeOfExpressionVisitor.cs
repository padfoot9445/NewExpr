using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class TypeOfExpressionVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        AssertPropertyIsNotNull<IHasAttributeGenericSLType>(x => x.GenericSLType is not null);
        base.PreVisit(node);
    }

    protected override void Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    {
        if (self is IHasAttributeTypeOfExpressionSettable and IHasAttributeGenericSLType)
            ((IHasAttributeTypeOfExpressionSettable)self).TypeOfExpression =
                ((IHasAttributeGenericSLType)self).GenericSLType!.OutmostType;
        base.Epilogue(Parent, self);
    }
}