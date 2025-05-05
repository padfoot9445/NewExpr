#pragma warning disable IDE0130
using Common.AST;
using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.Backend;
#pragma warning restore IDE0130
public partial class CodeGenVisitor : BaseCodeGenVisitor
{
    private Dictionary<string, uint> FunctionNameToID = [];
    private void Emit(Operation<uint> Instruction)
    {
        throw new NotImplementedException();
    }
    protected override bool AliasExpr(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ArgList(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ArgListElement(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool AssignmentPrime(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool BaseType(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool BinaryExpression(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ComparisionExpression(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool CopyExpr(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Declaration(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool DeclarationModifier(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool DeclarationModifiersCombined(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ExprStatementCombined(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool FactorialExpression(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool For(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Function(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool FunctionArgDeclModifiers(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool FunctionArgDeclModifiersCombined(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }


    protected override bool GenericType(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Identifier(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool If(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Index(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool LoopCTRL(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool LoopLabel(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool NewExpr(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool OperatorExpressionPair(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Primary(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Return(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ReTypeOriginal(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ReTypingAlias(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Section(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool Switch(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool TypeAndIdentifierCSV(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool TypeAndIdentifierCSVElement(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool TypeCSV(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool UnaryExpression(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool ValInLCTRL(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }

    protected override bool While(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }
}