using Common.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class AssignScopeVisitor : BaseASTVisitor
{
    private readonly Scope GlobalScope = new() { Parent = null };

    protected override void Epilogue<TArgumentType>(ISmallLangNode? Parent, TArgumentType self)
        => ((SmallLangNode)(ISmallLangNode)self).Scope ??= Parent?.Scope ?? GlobalScope;


    protected override ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self) => self;

    protected override ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self) => self;

    protected override ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self) => self;

    protected override ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self) => self;

    protected override ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self) => self;

    protected override ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self) => self;

    protected override ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self) => self;

    protected override ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self) => self;

    protected override ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self) => self;

    protected override ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => self;

    protected override ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self) => self;

    protected override ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self) => self;

    protected override ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self) => self;

    protected override ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self) => self;

    protected override ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self) => self;

    protected override ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self) => self;

    protected override ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => self;

    protected override ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => self;

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self) => self;

    protected override ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self) => self;

    protected override ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self) => self;

    protected override ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self) => self;
    protected override ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self) => self;

    protected override ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self) => self;

    protected override ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self) => self;

    protected override ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self) => self;

    protected override ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self) => self;

    protected override ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self) => self;

    protected override ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self) => self;

    protected override ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self) => self;

    protected override ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self) => self;

    protected override ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self)
    {
        self.Scope ??= new Scope() { Parent = Parent?.Scope };
        return self;
    }

    protected override ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self) => self;

    protected override ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => self;

    protected override ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self) => self;

    protected override ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self) => self;

    protected override ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self) => self;
}