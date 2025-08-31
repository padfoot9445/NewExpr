using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors;

public abstract class BaseASTVisitor<T, TPrologue, TBody, TEpilogue> : ISmallLangNodeVisitor<T>
{
    public List<T> BeginVisiting(ISmallLangNode node) => BeginVisiting<List<T>>(node, (x, y) => [.. x, .. y], (x, y) => [.. x, y]);
    public TOut BeginVisiting<TOut>(ISmallLangNode node, Func<TOut, TOut, TOut> Accumulator, Func<TOut, T, TOut> SubAccumulator)
    where TOut : new()
    {
        TOut RetVal = new();
        int Last = unchecked(node.GetHashCode() + 1);
        int CurrentHashCode;

        while (Last != (CurrentHashCode = node.GetHashCode()))
        {
            Last = CurrentHashCode;
            RetVal = Accumulator(RetVal, RecursiveVisit(null, node, Accumulator, SubAccumulator));
        }
        return RetVal;
    }
    private TOut RecursiveVisit<TOut>(ISmallLangNode? Parent, ISmallLangNode self, Func<TOut, TOut, TOut> Accumulator, Func<TOut, T, TOut> SubAccumulator)
    where TOut : new()
    {
        var RetVal = SubAccumulator(new(), self.AcceptVisitor(Parent, this));
        foreach (var childnode in self.ChildNodes)
        {
            RetVal = Accumulator(RetVal, RecursiveVisit(self, childnode, Accumulator, SubAccumulator));
        }
        return RetVal;
    }

    protected virtual TPrologue? Prologue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    where TArgumentType : ISmallLangNode
    {
        return default;
    }
    protected virtual TEpilogue? Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
    where TArgumentType : ISmallLangNode
    {
        return default;
    }

    protected virtual T ReturnCombiner(TPrologue? prologue, TBody body, TEpilogue? epilogue)
    {
        return Cast(body);
    }
    protected abstract T Cast(TBody body);
    protected virtual T Combinator<TArgumentType>(ISmallLangNode? Parent, TArgumentType self, Func<ISmallLangNode?, TArgumentType, TBody> BodyFunction)
    where TArgumentType : ISmallLangNode
    {
        return ReturnCombiner(
            Prologue(Parent, self),
            BodyFunction(Parent, self),
            Epilogue(Parent, self)
        );
    }


    protected abstract TBody VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self);
    protected abstract TBody VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self);
    protected abstract TBody VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self);
    protected abstract TBody VisitSection(ISmallLangNode? Parent, SectionNode self);
    protected abstract TBody VisitFunction(ISmallLangNode? Parent, FunctionNode self);
    protected abstract TBody VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self);
    protected abstract TBody VisitFor(ISmallLangNode? Parent, ForNode self);
    protected abstract TBody VisitWhile(ISmallLangNode? Parent, WhileNode self);
    protected abstract TBody VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self);
    protected abstract TBody VisitReturn(ISmallLangNode? Parent, ReturnNode self);
    protected abstract TBody VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self);
    protected abstract TBody VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self);
    protected abstract TBody VisitIf(ISmallLangNode? Parent, IfNode self);
    protected abstract TBody VisitSwitch(ISmallLangNode? Parent, SwitchNode self);
    protected abstract TBody VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self);
    protected abstract TBody VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self);
    protected abstract TBody VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self);
    protected abstract TBody VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self);
    protected abstract TBody VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self);
    protected abstract TBody VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self);
    protected abstract TBody VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self);
    protected abstract TBody VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self);
    protected abstract TBody VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self);
    protected abstract TBody VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self);
    protected abstract TBody VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self);
    protected abstract TBody VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self);
    protected abstract TBody VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self);
    protected abstract TBody VisitPrimary(ISmallLangNode? Parent, PrimaryNode self);
    protected abstract TBody VisitFunctionIdentifier(ISmallLangNode? Parent, FunctionIdentifierNode self);
    protected abstract TBody VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self);
    protected abstract TBody VisitNewExpr(ISmallLangNode? Parent, NewExprNode self);
    protected abstract TBody VisitIndex(ISmallLangNode? Parent, IndexNode self);
    protected abstract TBody VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self);
    protected abstract TBody VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self);
    protected abstract TBody VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self);
    protected abstract TBody VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self);
    protected abstract TBody VisitElse(ISmallLangNode? Parent, ElseNode self);
    protected abstract TBody VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self);


    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ReTypingAliasNode self) => Combinator(Parent, self, VisitReTypingAlias);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ReTypeOriginalNode self) => Combinator(Parent, self, VisitReTypeOriginal);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, IdentifierNode self) => Combinator(Parent, self, VisitIdentifier);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, SectionNode self) => Combinator(Parent, self, VisitSection);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FunctionNode self) => Combinator(Parent, self, VisitFunction);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, LoopCTRLNode self) => Combinator(Parent, self, VisitLoopCTRL);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ForNode self) => Combinator(Parent, self, VisitFor);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, WhileNode self) => Combinator(Parent, self, VisitWhile);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, LoopLabelNode self) => Combinator(Parent, self, VisitLoopLabel);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ReturnNode self) => Combinator(Parent, self, VisitReturn);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, BaseTypeNode self) => Combinator(Parent, self, VisitBaseType);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, GenericTypeNode self) => Combinator(Parent, self, VisitGenericType);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, IfNode self) => Combinator(Parent, self, VisitIf);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, SwitchNode self) => Combinator(Parent, self, VisitSwitch);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ExprSectionCombinedNode self) => Combinator(Parent, self, VisitExprSectionCombined);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => Combinator(Parent, self, VisitTypeAndIdentifierCSVElement);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, AliasExprNode self) => Combinator(Parent, self, VisitAliasExpr);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, DeclarationNode self) => Combinator(Parent, self, VisitDeclaration);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => Combinator(Parent, self, VisitDeclarationModifiersCombined);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, DeclarationModifierNode self) => Combinator(Parent, self, VisitDeclarationModifier);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => Combinator(Parent, self, VisitFunctionArgDeclModifiers);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => Combinator(Parent, self, VisitFunctionArgDeclModifiersCombined);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, AssignmentPrimeNode self) => Combinator(Parent, self, VisitAssignmentPrime);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FactorialExpressionNode self) => Combinator(Parent, self, VisitFactorialExpression);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, BinaryExpressionNode self) => Combinator(Parent, self, VisitBinaryExpression);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ComparisonExpressionNode self) => Combinator(Parent, self, VisitComparisonExpression);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, OperatorExpressionPairNode self) => Combinator(Parent, self, VisitOperatorExpressionPair);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, PrimaryNode self) => Combinator(Parent, self, VisitPrimary);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FunctionIdentifierNode self) => Combinator(Parent, self, VisitFunctionIdentifier);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, CopyExprNode self) => Combinator(Parent, self, VisitCopyExpr);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, NewExprNode self) => Combinator(Parent, self, VisitNewExpr);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, IndexNode self) => Combinator(Parent, self, VisitIndex);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FunctionCallNode self) => Combinator(Parent, self, VisitFunctionCall);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ArgListElementNode self) => Combinator(Parent, self, VisitArgListElement);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, TypeCSVNode self) => Combinator(Parent, self, VisitTypeCSV);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, UnaryExpressionNode self) => Combinator(Parent, self, VisitUnaryExpression);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, ElseNode self) => Combinator(Parent, self, VisitElse);

    T ISmallLangNodeVisitor<T>.Visit(ISmallLangNode? Parent, FactorialSymbolNode self) => Combinator(Parent, self, VisitFactorialSymbol);
}