using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class AssignScopeVisitor : ASTVisitor<object?, object?, object?, object?>
{
    private static void SetScope(IHasAttributeScope Self, IHasAttributeScopeSettable Dst)
    {

    }
    protected override object? Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
        => ((SmallLangNode)(ISmallLangNode)self).Scope = new Scope() { Parent = Parent?.Scope };
    protected override object? Cast(object? body)
    {
        return body;
    }

    protected override object? VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self) => default;

    protected override object? VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self) => default;

    protected override object? VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self) => default;

    protected override object? VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self) => default;

    protected override object? VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self) => default;

    protected override object? VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self) => default;

    protected override object? VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self) => default;

    protected override object? VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self) => default;

    protected override object? VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self) => default;

    protected override object? VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => default;

    protected override object? VisitElse(ISmallLangNode? Parent, ElseNode self) => ((IHasAttributeScopeSettable)self.Statement).Scope = new Scope() { Parent = self.Scope };

    protected override object? VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self) => default;

    protected override object? VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self) => default;

    protected override object? VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self) => default;

    protected override object? VisitFor(ISmallLangNode? Parent, ForNode self) => default;

    protected override object? VisitFunction(ISmallLangNode? Parent, FunctionNode self) => default;

    protected override object? VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => default;

    protected override object? VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => default;

    protected override object? VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self) => default;

    protected override object? VisitFunctionIdentifier(ISmallLangNode? Parent, FunctionIdentifierNode self) => default;

    protected override object? VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self) => default;

    protected override object? VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self) => default;

    protected override object? VisitIf(ISmallLangNode? Parent, IfNode self) => default;

    protected override object? VisitIndex(ISmallLangNode? Parent, IndexNode self) => default;

    protected override object? VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self) => default;

    protected override object? VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self) => default;

    protected override object? VisitNewExpr(ISmallLangNode? Parent, NewExprNode self) => default;

    protected override object? VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self) => default;

    protected override object? VisitPrimary(ISmallLangNode? Parent, PrimaryNode self) => default;

    protected override object? VisitReturn(ISmallLangNode? Parent, ReturnNode self) => default;

    protected override object? VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self) => default;

    protected override object? VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self) => default;

    protected override object? VisitSection(ISmallLangNode? Parent, SectionNode self) => default;

    protected override object? VisitSwitch(ISmallLangNode? Parent, SwitchNode self) => default;

    protected override object? VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => default;

    protected override object? VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self) => default;

    protected override object? VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self) => default;

    protected override object? VisitWhile(ISmallLangNode? Parent, WhileNode self) => default;
}