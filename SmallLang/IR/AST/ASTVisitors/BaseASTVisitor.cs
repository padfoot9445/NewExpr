using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors;

public abstract class BaseASTVisitor : ISmallLangNodeVisitor<ISmallLangNode>
{
    public ISmallLangNode BeginVisiting(ISmallLangNode node)
    {
        CurrentRootNode = node;

        PreVisit(node);

        int Last = unchecked(node.GetHashCode() + 1);
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
        foreach (var childnode in self.ChildNodes)
        {
            RecursiveVisit(self, childnode);
        }
    }

    [AllowNull]
    private ISmallLangNode CurrentRootNode { get; set; } = null;
    protected ISmallLangNode RecursiveGetParent(ISmallLangNode self, Func<ISmallLangNode, bool> Predicate) => self.RecursiveGetParent(CurrentRootNode, Predicate);

    protected virtual void PreVisit(ISmallLangNode node) { }
    protected virtual void PostVisit(ISmallLangNode node) { }

    protected virtual void Prologue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    where TArgumentType : ISmallLangNode
    { }
    protected virtual void Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    where TArgumentType : ISmallLangNode
    { }

    protected virtual ISmallLangNode Combinator<TArgumentType>(ISmallLangNode? Parent, TArgumentType self, Func<ISmallLangNode?, TArgumentType, ISmallLangNode> BodyFunction)
    where TArgumentType : ISmallLangNode
    {

        Prologue(Parent, self);
        var ret = BodyFunction(Parent, self);
        Epilogue(Parent, self);
        return ret;
    }


    protected virtual ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self) => self;
    protected virtual ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self) => self;
    protected virtual ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self) => self;
    protected virtual ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self) => self;
    protected virtual ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self) => self;
    protected virtual ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self) => self;
    protected virtual ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self) => self;
    protected virtual ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self) => self;
    protected virtual ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self) => self;
    protected virtual ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self) => self;
    protected virtual ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self) => self;
    protected virtual ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self) => self;
    protected virtual ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self) => self;
    protected virtual ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self) => self;
    protected virtual ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self) => self;
    protected virtual ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => self;
    protected virtual ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self) => self;
    protected virtual ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self) => self;
    protected virtual ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => self;
    protected virtual ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self) => self;
    protected virtual ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => self;
    protected virtual ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => self;
    protected virtual ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self) => self;
    protected virtual ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self) => self;
    protected virtual ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self) => self;
    protected virtual ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self) => self;
    protected virtual ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self) => self;
    protected virtual ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self) => self;
    protected virtual ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self) => self;
    protected virtual ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self) => self;
    protected virtual ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self) => self;
    protected virtual ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self) => self;
    protected virtual ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self) => self;
    protected virtual ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self) => self;
    protected virtual ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self) => self;
    protected virtual ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self) => self;
    protected virtual ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self) => self;


    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReTypingAliasNode self) => Combinator(Parent, self, VisitReTypingAlias);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReTypeOriginalNode self) => Combinator(Parent, self, VisitReTypeOriginal);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IdentifierNode self) => Combinator(Parent, self, VisitIdentifier);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, SectionNode self) => Combinator(Parent, self, VisitSection);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionNode self) => Combinator(Parent, self, VisitFunction);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, LoopCTRLNode self) => Combinator(Parent, self, VisitLoopCTRL);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ForNode self) => Combinator(Parent, self, VisitFor);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, WhileNode self) => Combinator(Parent, self, VisitWhile);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, LoopLabelNode self) => Combinator(Parent, self, VisitLoopLabel);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ReturnNode self) => Combinator(Parent, self, VisitReturn);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, BaseTypeNode self) => Combinator(Parent, self, VisitBaseType);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, GenericTypeNode self) => Combinator(Parent, self, VisitGenericType);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IfNode self) => Combinator(Parent, self, VisitIf);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, SwitchNode self) => Combinator(Parent, self, VisitSwitch);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ExprSectionCombinedNode self) => Combinator(Parent, self, VisitExprSectionCombined);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => Combinator(Parent, self, VisitTypeAndIdentifierCSVElement);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, AliasExprNode self) => Combinator(Parent, self, VisitAliasExpr);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, DeclarationNode self) => Combinator(Parent, self, VisitDeclaration);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => Combinator(Parent, self, VisitDeclarationModifiersCombined);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, DeclarationModifierNode self) => Combinator(Parent, self, VisitDeclarationModifier);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => Combinator(Parent, self, VisitFunctionArgDeclModifiers);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => Combinator(Parent, self, VisitFunctionArgDeclModifiersCombined);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, AssignmentPrimeNode self) => Combinator(Parent, self, VisitAssignmentPrime);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FactorialExpressionNode self) => Combinator(Parent, self, VisitFactorialExpression);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, BinaryExpressionNode self) => Combinator(Parent, self, VisitBinaryExpression);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ComparisonExpressionNode self) => Combinator(Parent, self, VisitComparisonExpression);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, OperatorExpressionPairNode self) => Combinator(Parent, self, VisitOperatorExpressionPair);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, PrimaryNode self) => Combinator(Parent, self, VisitPrimary);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, CopyExprNode self) => Combinator(Parent, self, VisitCopyExpr);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, NewExprNode self) => Combinator(Parent, self, VisitNewExpr);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IndexNode self) => Combinator(Parent, self, VisitIndex);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionCallNode self) => Combinator(Parent, self, VisitFunctionCall);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ArgListElementNode self) => Combinator(Parent, self, VisitArgListElement);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, TypeCSVNode self) => Combinator(Parent, self, VisitTypeCSV);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, UnaryExpressionNode self) => Combinator(Parent, self, VisitUnaryExpression);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ElseNode self) => Combinator(Parent, self, VisitElse);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FactorialSymbolNode self) => Combinator(Parent, self, VisitFactorialSymbol);
    protected static void NotNull([NotNull] object? O1) => Debug.Assert(O1 is not null);
    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2)
    {
        NotNull(O1); NotNull(O2);
    }
    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2, [NotNull] object? O3)
    {
        NotNull(O1, O2); NotNull(O3);
    }
    protected static void NotNull([NotNull] object? O1, [NotNull] object? O2, [NotNull] object? O3, [NotNull] object? O4)
    {
        NotNull(O1, O2); NotNull(O3, O4);
    }
}