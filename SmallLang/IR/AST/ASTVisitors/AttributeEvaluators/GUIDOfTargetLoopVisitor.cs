using System.Diagnostics;
using Common.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class GUIDOfTargetLoopVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        Debug.Assert(node.Flatten().OfType<IHasAttributeLoopGUID>().All(x => x.LoopGUID is not null));
        base.PreVisit(node);
    }

    private static bool CompareLoopLabel(IdentifierNode Label, ILoopNode LoopNode)
    {
        return
            (LoopNode is ForNode ForNode && ForNode.LoopLabel?.Identifier.Data.Lexeme == Label.Data.Lexeme) ||
            (LoopNode is WhileNode WhileNode && WhileNode.LoopLabel?.Identifier.Data.Lexeme == Label.Data.Lexeme);
    }

    protected override ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        if (self.Identifier is null)
            self.GUIDOfTargetLoop = ((IHasAttributeLoopGUID)RecursiveGetParent(self, x => x is IHasAttributeLoopGUID))
                .LoopGUID;
        else
            self.GUIDOfTargetLoop = ((IHasAttributeLoopGUID)RecursiveGetParent(self,
                x => x is ILoopNode LoopNode && CompareLoopLabel(self.Identifier, LoopNode))).LoopGUID;
        return base.VisitLoopCTRL(Parent, self);
    }
}