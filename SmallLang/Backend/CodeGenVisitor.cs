using System.Linq.Expressions;
using System.Xml;
using Common.AST;
using Common.LinearIR;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang.Backend;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public class CodeGenVisitor
{
    public virtual uint[] OutputRegisters { get; set; } = []; //modified by the callee to propagate where it put the value
    public virtual bool OutputToRegister { get; set; } = true;
    public virtual uint? DestinationRegister { get; set; }
    public uint LastUsedRegister { get; set; } = 0;
    public List<Operation<Opcode, BackingNumberType>> Instructions = [];
    public List<uint> StaticData = [];
    public Dictionary<VariableName, uint> VariableNameToRegister = [];
    //public Dictionary<VariableName, VariableModifiers> VariableNameToModifiers = [];
    public void Exec(Node? parent, Node node, bool? OutputToRegister = null, uint? DestinationRegister = null, uint[]? OutputRegisters = null) => Dispatch(node, OutputToRegister, DestinationRegister, OutputRegisters)(parent, node);
    public void SetState(bool? OTR, uint? DR, uint[]? OR)
    {
        OutputToRegister = OTR ?? OutputToRegister;
        DestinationRegister = DR ?? DestinationRegister;
        OutputRegisters = OR ?? OutputRegisters;
    }
    public Action<Node?, Node> Dispatch(Node node, bool? OutputToRegister = null, uint? DestinationRegister = null, uint[]? OutputRegisters = null)
    {
        SetState(OutputToRegister, DestinationRegister, OutputRegisters);
        switch (node.NodeType)
        {
            case ImportantASTNodeType.ReTypingAlias: return ReTypingAlias.GenerateCode;
            case ImportantASTNodeType.ReTypeOriginal: return ReTypeOriginal.GenerateCode;
            case ImportantASTNodeType.Identifier: return Identifier.GenerateCode;
            case ImportantASTNodeType.Section: return Section.GenerateCode;
            case ImportantASTNodeType.Function: return Function.GenerateCode;
            case ImportantASTNodeType.LoopCTRL: return LoopCTRL.GenerateCode;
            case ImportantASTNodeType.For: return For.GenerateCode;
            case ImportantASTNodeType.While: return While.GenerateCode;
            case ImportantASTNodeType.ValInLCTRL: return ValInLCTRL.GenerateCode;
            case ImportantASTNodeType.LoopLabel: return LoopLabel.GenerateCode;
            case ImportantASTNodeType.Return: return Return.GenerateCode;
            case ImportantASTNodeType.BaseType: return BaseType.GenerateCode;
            case ImportantASTNodeType.GenericType: return GenericType.GenerateCode;
            case ImportantASTNodeType.If: return If.GenerateCode;
            case ImportantASTNodeType.Switch: return Switch.GenerateCode;
            case ImportantASTNodeType.ExprStatementCombined: return ExprStatementCombined.GenerateCode;
            case ImportantASTNodeType.TypeAndIdentifierCSV: return TypeAndIdentifierCSV.GenerateCode;
            case ImportantASTNodeType.TypeAndIdentifierCSVElement: return TypeAndIdentifierCSVElement.GenerateCode;
            case ImportantASTNodeType.AliasExpr: return AliasExpr.GenerateCode;
            case ImportantASTNodeType.Declaration: return Declaration.GenerateCode;
            case ImportantASTNodeType.DeclarationModifiersCombined: return DeclarationModifiersCombined.GenerateCode;
            case ImportantASTNodeType.DeclarationModifier: return DeclarationModifier.GenerateCode;
            case ImportantASTNodeType.FunctionArgDeclModifiers: return FunctionArgDeclModifiers.GenerateCode;
            case ImportantASTNodeType.FunctionArgDeclModifiersCombined: return FunctionArgDeclModifiersCombined.GenerateCode;
            case ImportantASTNodeType.AssignmentPrime: return AssignmentPrime.GenerateCode;
            case ImportantASTNodeType.FactorialExpression: return FactorialExpression.GenerateCode;
            case ImportantASTNodeType.BinaryExpression: return BinaryExpression.GenerateCode;
            case ImportantASTNodeType.ComparisionExpression: return ComparisionExpression.GenerateCode;
            case ImportantASTNodeType.OperatorExpressionPair: return OperatorExpressionPair.GenerateCode;
            case ImportantASTNodeType.Primary: return Primary.GenerateCode;
            case ImportantASTNodeType.CopyExpr: return CopyExpr.GenerateCode;
            case ImportantASTNodeType.NewExpr: return NewExpr.GenerateCode;
            case ImportantASTNodeType.Index: return Index.GenerateCode;
            case ImportantASTNodeType.FunctionCall: return FunctionCall.GenerateCode;
            case ImportantASTNodeType.ArgList: return ArgList.GenerateCode;
            case ImportantASTNodeType.ArgListElement: return ArgListElement.GenerateCode;
            case ImportantASTNodeType.TypeCSV: return TypeCSV.GenerateCode;
            case ImportantASTNodeType.UnaryExpression: return UnaryExpression.GenerateCode;
            default: throw new Exception(node.NodeType.ToString() + "not expected");
        }
    }
    private BaseCodeGenComponent? _ReTypingAlias = null;
    protected virtual BaseCodeGenComponent ReTypingAlias => throw new NotImplementedException();
    private BaseCodeGenComponent? _ReTypeOriginal = null;
    protected virtual BaseCodeGenComponent ReTypeOriginal => throw new NotImplementedException();
    private BaseCodeGenComponent? _Identifier = null;
    protected virtual BaseCodeGenComponent Identifier => Primary;
    private BaseCodeGenComponent? _Section = null;
    protected virtual BaseCodeGenComponent Section => _Section is null ? _Section = new Section(this) : _Section;
    private BaseCodeGenComponent? _Function = null;
    protected virtual BaseCodeGenComponent Function => throw new NotImplementedException();
    private BaseCodeGenComponent? _LoopCTRL = null;
    protected virtual BaseCodeGenComponent LoopCTRL => throw new NotImplementedException();
    private BaseCodeGenComponent? _For = null;
    protected virtual BaseCodeGenComponent For => throw new NotImplementedException();
    private BaseCodeGenComponent? _While = null;
    protected virtual BaseCodeGenComponent While => throw new NotImplementedException();
    private BaseCodeGenComponent? _ValInLCTRL = null;
    protected virtual BaseCodeGenComponent ValInLCTRL => throw new NotImplementedException();
    private BaseCodeGenComponent? _LoopLabel = null;
    protected virtual BaseCodeGenComponent LoopLabel => throw new NotImplementedException();
    private BaseCodeGenComponent? _Return = null;
    protected virtual BaseCodeGenComponent Return => throw new NotImplementedException();
    private BaseCodeGenComponent? _BaseType = null;
    protected virtual BaseCodeGenComponent BaseType => throw new NotImplementedException();
    private BaseCodeGenComponent? _GenericType = null;
    protected virtual BaseCodeGenComponent GenericType => throw new NotImplementedException();
    private BaseCodeGenComponent? _If = null;
    protected virtual BaseCodeGenComponent If => throw new NotImplementedException();
    private BaseCodeGenComponent? _Switch = null;
    protected virtual BaseCodeGenComponent Switch => throw new NotImplementedException();
    private BaseCodeGenComponent? _ExprStatementCombined = null;
    protected virtual BaseCodeGenComponent ExprStatementCombined => throw new NotImplementedException();
    private BaseCodeGenComponent? _TypeAndIdentifierCSV = null;
    protected virtual BaseCodeGenComponent TypeAndIdentifierCSV => throw new NotImplementedException();
    private BaseCodeGenComponent? _TypeAndIdentifierCSVElement = null;
    protected virtual BaseCodeGenComponent TypeAndIdentifierCSVElement => throw new NotImplementedException();
    private BaseCodeGenComponent? _AliasExpr = null;
    protected virtual BaseCodeGenComponent AliasExpr => throw new NotImplementedException();
    private BaseCodeGenComponent? _Declaration = null;
    protected virtual BaseCodeGenComponent Declaration => _Declaration ??= new Declaration(this);
    private BaseCodeGenComponent? _DeclarationModifiersCombined = null;
    protected virtual BaseCodeGenComponent DeclarationModifiersCombined => throw new NotImplementedException();
    private BaseCodeGenComponent? _DeclarationModifier = null;
    protected virtual BaseCodeGenComponent DeclarationModifier => throw new NotImplementedException();
    private BaseCodeGenComponent? _FunctionArgDeclModifiers = null;
    protected virtual BaseCodeGenComponent FunctionArgDeclModifiers => throw new NotImplementedException();
    private BaseCodeGenComponent? _FunctionArgDeclModifiersCombined = null;
    protected virtual BaseCodeGenComponent FunctionArgDeclModifiersCombined => throw new NotImplementedException();
    private BaseCodeGenComponent? _AssignmentPrime = null;
    protected virtual BaseCodeGenComponent AssignmentPrime => _AssignmentPrime ??= new AssignmentPrime(this);
    private BaseCodeGenComponent? _FactorialExpression = null;
    protected virtual BaseCodeGenComponent FactorialExpression => throw new NotImplementedException();
    private BaseCodeGenComponent? _BinaryExpression = null;
    protected virtual BaseCodeGenComponent BinaryExpression => throw new NotImplementedException();
    private BaseCodeGenComponent? _ComparisionExpression = null;
    protected virtual BaseCodeGenComponent ComparisionExpression => throw new NotImplementedException();
    private BaseCodeGenComponent? _OperatorExpressionPair = null;
    protected virtual BaseCodeGenComponent OperatorExpressionPair => throw new NotImplementedException();
    private BaseCodeGenComponent? _Primary = null;
    protected virtual BaseCodeGenComponent Primary => _Primary is null ? _Primary = new Primary(this) : _Primary;
    private BaseCodeGenComponent? _CopyExpr = null;
    protected virtual BaseCodeGenComponent CopyExpr => throw new NotImplementedException();
    private BaseCodeGenComponent? _NewExpr = null;
    protected virtual BaseCodeGenComponent NewExpr => new NewExpr(this);
    private BaseCodeGenComponent? _Index = null;
    protected virtual BaseCodeGenComponent Index => throw new NotImplementedException();
    private BaseCodeGenComponent? _FunctionCall = null;
    protected virtual BaseCodeGenComponent FunctionCall => _FunctionCall is null ? _FunctionCall = new FunctionCall(this) : _FunctionCall;
    private BaseCodeGenComponent? _ArgList = null;
    protected virtual BaseCodeGenComponent ArgList => throw new NotImplementedException();
    private BaseCodeGenComponent? _ArgListElement = null;
    protected virtual BaseCodeGenComponent ArgListElement => throw new NotImplementedException();
    private BaseCodeGenComponent? _TypeCSV = null;
    protected virtual BaseCodeGenComponent TypeCSV => throw new NotImplementedException();
    private BaseCodeGenComponent? _UnaryExpression = null;
    protected virtual BaseCodeGenComponent UnaryExpression => throw new NotImplementedException();
}