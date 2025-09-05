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


    protected override ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent,
        DeclarationModifiersCombinedNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent,
        FunctionArgDeclModifiersNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent,
        FunctionArgDeclModifiersCombinedNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent,
        OperatorExpressionPairNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self)
    {
        if (ReferenceEquals(self, CurrentRootNode) || self.Scope is not null) return self;

        Debug.Assert(Parent?.Scope is not null);
        self.Scope = new Scope(Parent.Scope);
        return self;
    }

    protected override ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent,
        TypeAndIdentifierCSVElementNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self)
    {
        return self;
    }

    protected override ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self)
    {
        return self;
    }
}