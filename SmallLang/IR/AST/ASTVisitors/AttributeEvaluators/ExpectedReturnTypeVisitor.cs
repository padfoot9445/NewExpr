using System.Diagnostics;
using Common.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class ExpectedReturnTypeVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        Debug.Assert(node.Flatten().OfType<IHasAttributeTypeLiteralType>().All(x => x.TypeLiteralType is not null));
        base.PreVisit(node);
    }

    protected override ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self)
    {
        self.ExpectedReturnType ??=
            ((FunctionNode)RecursiveGetParent(self, x => x is FunctionNode)).Type.TypeLiteralType;
        return base.VisitReturn(Parent, self);
    }
}