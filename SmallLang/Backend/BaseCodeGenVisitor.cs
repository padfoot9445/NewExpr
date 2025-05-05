using Common.AST;
using Common.Evaluator;

namespace SmallLang.Backend;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public abstract class BaseCodeGenVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    protected bool Exec(Node? parent, Node self) => Dispatch(self)(parent, self);
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        switch (node.NodeType)
        {
            case ImportantASTNodeType.ReTypingAlias: return ReTypingAlias;
            case ImportantASTNodeType.ReTypeOriginal: return ReTypeOriginal;
            case ImportantASTNodeType.Identifier: return Identifier;
            case ImportantASTNodeType.Section: return Section;
            case ImportantASTNodeType.Function: return Function;
            case ImportantASTNodeType.LoopCTRL: return LoopCTRL;
            case ImportantASTNodeType.For: return For;
            case ImportantASTNodeType.While: return While;
            case ImportantASTNodeType.ValInLCTRL: return ValInLCTRL;
            case ImportantASTNodeType.LoopLabel: return LoopLabel;
            case ImportantASTNodeType.Return: return Return;
            case ImportantASTNodeType.BaseType: return BaseType;
            case ImportantASTNodeType.GenericType: return GenericType;
            case ImportantASTNodeType.If: return If;
            case ImportantASTNodeType.Switch: return Switch;
            case ImportantASTNodeType.ExprStatementCombined: return ExprStatementCombined;
            case ImportantASTNodeType.TypeAndIdentifierCSV: return TypeAndIdentifierCSV;
            case ImportantASTNodeType.TypeAndIdentifierCSVElement: return TypeAndIdentifierCSVElement;
            case ImportantASTNodeType.AliasExpr: return AliasExpr;
            case ImportantASTNodeType.Declaration: return Declaration;
            case ImportantASTNodeType.DeclarationModifiersCombined: return DeclarationModifiersCombined;
            case ImportantASTNodeType.DeclarationModifier: return DeclarationModifier;
            case ImportantASTNodeType.FunctionArgDeclModifiers: return FunctionArgDeclModifiers;
            case ImportantASTNodeType.FunctionArgDeclModifiersCombined: return FunctionArgDeclModifiersCombined;
            case ImportantASTNodeType.AssignmentPrime: return AssignmentPrime;
            case ImportantASTNodeType.FactorialExpression: return FactorialExpression;
            case ImportantASTNodeType.BinaryExpression: return BinaryExpression;
            case ImportantASTNodeType.ComparisionExpression: return ComparisionExpression;
            case ImportantASTNodeType.OperatorExpressionPair: return OperatorExpressionPair;
            case ImportantASTNodeType.Primary: return Primary;
            case ImportantASTNodeType.CopyExpr: return CopyExpr;
            case ImportantASTNodeType.NewExpr: return NewExpr;
            case ImportantASTNodeType.Index: return Index;
            case ImportantASTNodeType.FunctionCall: return FunctionCall;
            case ImportantASTNodeType.ArgList: return ArgList;
            case ImportantASTNodeType.ArgListElement: return ArgListElement;
            case ImportantASTNodeType.TypeCSV: return TypeCSV;
            case ImportantASTNodeType.UnaryExpression: return UnaryExpression;
            default: throw new Exception(node.NodeType.ToString() + "not expected");
        }
    }
    protected abstract bool ReTypingAlias(Node? Parent, Node self);
    protected abstract bool ReTypeOriginal(Node? Parent, Node self);
    protected abstract bool Identifier(Node? Parent, Node self);
    protected abstract bool Section(Node? Parent, Node self);
    protected abstract bool Function(Node? Parent, Node self);
    protected abstract bool LoopCTRL(Node? Parent, Node self);
    protected abstract bool For(Node? Parent, Node self);
    protected abstract bool While(Node? Parent, Node self);
    protected abstract bool ValInLCTRL(Node? Parent, Node self);
    protected abstract bool LoopLabel(Node? Parent, Node self);
    protected abstract bool Return(Node? Parent, Node self);
    protected abstract bool BaseType(Node? Parent, Node self);
    protected abstract bool GenericType(Node? Parent, Node self);
    protected abstract bool If(Node? Parent, Node self);
    protected abstract bool Switch(Node? Parent, Node self);
    protected abstract bool ExprStatementCombined(Node? Parent, Node self);
    protected abstract bool TypeAndIdentifierCSV(Node? Parent, Node self);
    protected abstract bool TypeAndIdentifierCSVElement(Node? Parent, Node self);
    protected abstract bool AliasExpr(Node? Parent, Node self);
    protected abstract bool Declaration(Node? Parent, Node self);
    protected abstract bool DeclarationModifiersCombined(Node? Parent, Node self);
    protected abstract bool DeclarationModifier(Node? Parent, Node self);
    protected abstract bool FunctionArgDeclModifiers(Node? Parent, Node self);
    protected abstract bool FunctionArgDeclModifiersCombined(Node? Parent, Node self);
    protected abstract bool AssignmentPrime(Node? Parent, Node self);
    protected abstract bool FactorialExpression(Node? Parent, Node self);
    protected abstract bool BinaryExpression(Node? Parent, Node self);
    protected abstract bool ComparisionExpression(Node? Parent, Node self);
    protected abstract bool OperatorExpressionPair(Node? Parent, Node self);
    protected abstract bool Primary(Node? Parent, Node self);
    protected abstract bool CopyExpr(Node? Parent, Node self);
    protected abstract bool NewExpr(Node? Parent, Node self);
    protected abstract bool Index(Node? Parent, Node self);
    protected abstract bool FunctionCall(Node? Parent, Node self);
    protected abstract bool ArgList(Node? Parent, Node self);
    protected abstract bool ArgListElement(Node? Parent, Node self);
    protected abstract bool TypeCSV(Node? Parent, Node self);
    protected abstract bool UnaryExpression(Node? Parent, Node self);
}
