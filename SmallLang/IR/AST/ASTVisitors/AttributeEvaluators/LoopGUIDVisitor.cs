using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class LoopGUIDVisitor : BaseASTVisitor
{
    protected override ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self)
    {
        self.LoopGUID ??= new();
        return base.VisitFor(Parent, self);
    }

    protected override ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self)
    {
        self.LoopGUID ??= new();
        return base.VisitWhile(Parent, self);
    }
}