namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

public class AttributeEvaluator : BaseASTVisitor
{
    private readonly List<BaseASTVisitor> OrderedPasses =
    [
        new AssignScopeVisitor(),

        new TypeLiteralTypeVisitor(),
        new FunctionIDVisitor(),
        new VariableNameVisitor(),
        new LoopGUIDVisitor(),
        new GUIDOfTargetLoopVisitor(),
        new GenericSLTypeVisitor(),
        new TypeOfExpressionVisitor(),
        new ExpectedReturnTypeVisitor(),
        new ExpectedTypeOfExpressionVisitor()
    ];

    protected override void PreVisit(ISmallLangNode node)
    {
        foreach (var pass in OrderedPasses) pass.BeginVisiting(node);
        base.PreVisit(node);
    }
}