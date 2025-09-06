using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Common.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors;

public abstract class BaseASTVisitor : ISmallLangNodeVisitor<ISmallLangNode>
{
    [AllowNull] protected ISmallLangNode CurrentRootNode { get; set; }


    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        return Combinator(Parent, self, VisitReTypingAlias);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReTypeOriginalNode self)
    {
        return Combinator(Parent, self, VisitReTypeOriginal);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IdentifierNode self)
    {
        return Combinator(Parent, self, VisitIdentifier);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, SectionNode self)
    {
        return Combinator(Parent, self, VisitSection);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionNode self)
    {
        return Combinator(Parent, self, VisitFunction);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        return Combinator(Parent, self, VisitLoopCTRL);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ForNode self)
    {
        return Combinator(Parent, self, VisitFor);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, WhileNode self)
    {
        return Combinator(Parent, self, VisitWhile);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, LoopLabelNode self)
    {
        return Combinator(Parent, self, VisitLoopLabel);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReturnNode self)
    {
        return Combinator(Parent, self, VisitReturn);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, BaseTypeNode self)
    {
        return Combinator(Parent, self, VisitBaseType);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, GenericTypeNode self)
    {
        return Combinator(Parent, self, VisitGenericType);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IfNode self)
    {
        return Combinator(Parent, self, VisitIf);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, SwitchNode self)
    {
        return Combinator(Parent, self, VisitSwitch);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ExprSectionCombinedNode self)
    {
        return Combinator(Parent, self, VisitExprSectionCombined);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent,
        TypeAndIdentifierCSVElementNode self)
    {
        return Combinator(Parent, self, VisitTypeAndIdentifierCSVElement);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, AliasExprNode self)
    {
        return Combinator(Parent, self, VisitAliasExpr);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, DeclarationNode self)
    {
        return Combinator(Parent, self, VisitDeclaration);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent,
        DeclarationModifiersCombinedNode self)
    {
        return Combinator(Parent, self, VisitDeclarationModifiersCombined);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, DeclarationModifierNode self)
    {
        return Combinator(Parent, self, VisitDeclarationModifier);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent,
        FunctionArgDeclModifiersNode self)
    {
        return Combinator(Parent, self, VisitFunctionArgDeclModifiers);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent,
        FunctionArgDeclModifiersCombinedNode self)
    {
        return Combinator(Parent, self, VisitFunctionArgDeclModifiersCombined);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, AssignmentPrimeNode self)
    {
        return Combinator(Parent, self, VisitAssignmentPrime);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FactorialExpressionNode self)
    {
        return Combinator(Parent, self, VisitFactorialExpression);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, BinaryExpressionNode self)
    {
        return Combinator(Parent, self, VisitBinaryExpression);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ComparisonExpressionNode self)
    {
        return Combinator(Parent, self, VisitComparisonExpression);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, OperatorExpressionPairNode self)
    {
        return Combinator(Parent, self, VisitOperatorExpressionPair);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, PrimaryNode self)
    {
        return Combinator(Parent, self, VisitPrimary);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, CopyExprNode self)
    {
        return Combinator(Parent, self, VisitCopyExpr);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, NewExprNode self)
    {
        return Combinator(Parent, self, VisitNewExpr);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IndexNode self)
    {
        return Combinator(Parent, self, VisitIndex);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionCallNode self)
    {
        return Combinator(Parent, self, VisitFunctionCall);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ArgListElementNode self)
    {
        return Combinator(Parent, self, VisitArgListElement);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, TypeCSVNode self)
    {
        return Combinator(Parent, self, VisitTypeCSV);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, UnaryExpressionNode self)
    {
        return Combinator(Parent, self, VisitUnaryExpression);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ElseNode self)
    {
        return Combinator(Parent, self, VisitElse);
    }

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FactorialSymbolNode self)
    {
        return Combinator(Parent, self, VisitFactorialSymbol);
    }

    public ISmallLangNode BeginVisiting(ISmallLangNode node)
    {
        CurrentRootNode = node;

        PreVisit(node);

        var Last = unchecked(node.GetHashCode() + 1);
        int CurrentHashCode;

        while (Last != (CurrentHashCode = node.GetHashCode()))
        {
            Last = CurrentHashCode;
            RecursiveVisit(null, node);
        }

        PostVisit(node);
        return node;
    }

    private void RecursiveVisit(ISmallLangNode? Parent, ISmallLangNode self)
    {
        self.AcceptVisitor(Parent, this);
        foreach (var childnode in self.ChildNodes) RecursiveVisit(self, childnode);
    }

    protected void AssertPropertyIsNotNull<T>(Func<T, bool> predicate)
    {
        Debug.Assert(CurrentRootNode.Flatten().OfType<T>().All(predicate));
    }

    protected ISmallLangNode RecursiveGetParent(ISmallLangNode self, Func<ISmallLangNode, bool> Predicate)
    {
        return self.RecursiveGetParent(CurrentRootNode, Predicate);
    }

    protected virtual void PreVisit(ISmallLangNode node)
    {
    }

    protected virtual void PostVisit(ISmallLangNode node)
    {
    }

    protected virtual void Prologue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
        where TArgumentType : ISmallLangNode
    {
    }

    protected virtual void Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
        where TArgumentType : ISmallLangNode
    {
    }

    protected virtual ISmallLangNode Combinator<TArgumentType>(ISmallLangNode? Parent, TArgumentType self,
        Func<ISmallLangNode?, TArgumentType, ISmallLangNode> BodyFunction)
        where TArgumentType : ISmallLangNode
    {
        Prologue(Parent, self);
        var ret = BodyFunction(Parent, self);
        Epilogue(Parent, self);
        return ret;
    }


    protected virtual ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent,
        TypeAndIdentifierCSVElementNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent,
        DeclarationModifiersCombinedNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent,
        FunctionArgDeclModifiersNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent,
        FunctionArgDeclModifiersCombinedNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent,
        OperatorExpressionPairNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self)
    {
        return self;
    }

    protected virtual ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self)
    {
        return self;
    }

    protected static void NotNull([NotNull] object? O1)
    {
        Debug.Assert(O1 is not null);
    }

    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2)
    {
        NotNull(O1);
        NotNull(O2);
    }

    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2, [NotNull] object? O3)
    {
        NotNull(O1, O2);
        NotNull(O3);
    }

    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2, [NotNull] object? O3,
        [NotNull] object? O4)
    {
        NotNull(O1, O2);
        NotNull(O3, O4);
    }
}