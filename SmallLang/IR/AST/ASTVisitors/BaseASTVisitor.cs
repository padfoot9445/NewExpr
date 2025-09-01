using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors;

public abstract class BaseASTVisitor : ISmallLangNodeVisitor<ISmallLangNode>
{
    public ISmallLangNode BeginVisiting(ISmallLangNode node)
    {
        PreVisit(node);

        int Last = unchecked(node.GetHashCode() + 1);
        int CurrentHashCode;

        while (Last != (CurrentHashCode = node.GetHashCode()))
        {
            Last = CurrentHashCode;
            RecursiveVisit(null, node);
        }
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
    protected virtual void PreVisit(ISmallLangNode node) { }

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


    protected abstract ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self);
    protected abstract ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self);
    protected abstract ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self);
    protected abstract ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self);
    protected abstract ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self);
    protected abstract ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self);
    protected abstract ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self);
    protected abstract ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self);
    protected abstract ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self);
    protected abstract ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self);
    protected abstract ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self);
    protected abstract ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self);
    protected abstract ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self);
    protected abstract ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self);
    protected abstract ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self);
    protected abstract ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self);
    protected abstract ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self);
    protected abstract ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self);
    protected abstract ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self);
    protected abstract ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self);
    protected abstract ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self);
    protected abstract ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self);
    protected abstract ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self);
    protected abstract ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self);
    protected abstract ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self);
    protected abstract ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self);
    protected abstract ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self);
    protected abstract ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self);
    protected abstract ISmallLangNode VisitFunctionIdentifier(ISmallLangNode? Parent, FunctionIdentifierNode self);
    protected abstract ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self);
    protected abstract ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self);
    protected abstract ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self);
    protected abstract ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self);
    protected abstract ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self);
    protected abstract ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self);
    protected abstract ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self);
    protected abstract ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self);
    protected abstract ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self);


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

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionIdentifierNode self) => Combinator(Parent, self, VisitFunctionIdentifier);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, CopyExprNode self) => Combinator(Parent, self, VisitCopyExpr);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, NewExprNode self) => Combinator(Parent, self, VisitNewExpr);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, IndexNode self) => Combinator(Parent, self, VisitIndex);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FunctionCallNode self) => Combinator(Parent, self, VisitFunctionCall);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ArgListElementNode self) => Combinator(Parent, self, VisitArgListElement);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, TypeCSVNode self) => Combinator(Parent, self, VisitTypeCSV);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, UnaryExpressionNode self) => Combinator(Parent, self, VisitUnaryExpression);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, ElseNode self) => Combinator(Parent, self, VisitElse);

    ISmallLangNode ISmallLangNodeVisitor<ISmallLangNode>.Visit(ISmallLangNode? Parent, FactorialSymbolNode self) => Combinator(Parent, self, VisitFactorialSymbol);
}