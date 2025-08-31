using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class AssignScopeVisitor : ISmallLangNodeVisitor<object?>
{
    private static object? PropagateScope(ISmallLangNode? Parent, SmallLangNode self)
    {
        if (Parent is not null)
        {
            self.Scope ??= Parent.Scope;
        }
        else
        {
            self.Scope = new Scope { Parent = null };
        }
        return default;
    }

    public object? Visit(ISmallLangNode? Parent, ReTypingAliasNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ReTypeOriginalNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, IdentifierNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, SectionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FunctionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, LoopCTRLNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ForNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, WhileNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, LoopLabelNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ReturnNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, BaseTypeNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, GenericTypeNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, IfNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, SwitchNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ExprSectionCombinedNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, AliasExprNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, DeclarationNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, DeclarationModifierNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, AssignmentPrimeNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FactorialExpressionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, BinaryExpressionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ComparisonExpressionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, OperatorExpressionPairNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, PrimaryNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FunctionIdentifierNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, CopyExprNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, NewExprNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, IndexNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FunctionCallNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ArgListElementNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, TypeCSVNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, UnaryExpressionNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, ElseNode self) => PropagateScope(Parent, self);

    public object? Visit(ISmallLangNode? Parent, FactorialSymbolNode self) => PropagateScope(Parent, self);
}