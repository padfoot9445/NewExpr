using System.Diagnostics;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;
namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class AssignScopeVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        ((SmallLangNode)node).Scope = new Scope(null);
        base.PreVisit(node);
    }
    protected override void Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    {
        if (ReferenceEquals(self, CurrentRootNode)) return;
        ((SmallLangNode)(ISmallLangNode)self).Scope ??= Parent!.Scope!;
    }
    protected override ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self)
    {
        if (ReferenceEquals(self, CurrentRootNode) || self.Scope is not null) return self;
        Debug.Assert(Parent?.Scope is not null);
        self.Scope = new Scope(Parent.Scope);
        return self;
    }
}