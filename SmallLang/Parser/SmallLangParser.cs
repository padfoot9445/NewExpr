
using Common.AST;
using Common.Tokens;
using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;
using LyToken = sly.lexer.Token<Common.Tokens.TokenType>;
using NodeType = Common.AST.DynamicASTNode<SmallLang.ASTNodeType, SmallLang.Attributes>;
namespace SmallLang.Parser;
public partial class SmallLangParser
{
    void AppendIfNotEmpty(List<NodeType> nodeList, ValueOption<NodeType> Considered)
    {
        if (GetFromValOp(Considered) is NodeType node)
        {
            nodeList.Add(node);
        }
    }
    List<NodeType> BuildChildren(params object[] Vals)
    {
        List<NodeType> rc = new();
        foreach (var val in Vals)
        {
            if (val is NodeType node)
            {
                rc.Add(node);
            }
            else if (val is ValueOption<NodeType> valOp)
            {
                AppendIfNotEmpty(rc, valOp);
            }
            else if (val is ValueOption<LyToken> ValT)
            {
                if (FromToken(ValT) is IToken Token)
                {
                    if (Token.TT == TokenType.Identifier)
                    {
                        rc.Add(new NodeType(Token, [], ASTNodeType.Identifier));
                    }
                    else
                    {
                        rc.Add(new NodeType(Token, [], ASTNodeType.Terminal));
                    }
                }
            }
            else if (val is LyToken TVal)
            {
                var Token = FromToken(TVal);
                if (Token.TT == TokenType.Identifier)
                {
                    rc.Add(new NodeType(Token, [], ASTNodeType.Identifier));
                }
                else
                {
                    rc.Add(new NodeType(Token, [], ASTNodeType.Terminal));
                }
            }
            else
            {
                throw new Exception($"Unexpected type: {val.GetType().Name}");
            }
        }
        return rc;
    }
    NodeType? GetFromValOp(ValueOption<NodeType> value)
    {
        if (value.IsSome)
        {
            return value.Match(x => x, () => throw new Exception("IsSome was true when no value was found"));
        }
        return null;
    }
    IToken FromToken(LyToken t) => IToken.NewToken(t.TokenID, t.Value, t.Position.Index, null);
    IToken? FromToken(ValueOption<LyToken> t) => t.Match(x => FromToken(x), () => null!);
    [Production($"{nameof(Statement)}*")]
    public NodeType Section(List<NodeType> Statements)
    {
        return new NodeType(null, Statements, ASTNodeType.Section);
    }
    [Production($"[{nameof(SCExpr)} | {nameof(Loop)} | {nameof(Cond)} | {nameof(Function)} | {nameof(Block)} | {nameof(ReturnStatement)} | {nameof(LoopControlStatement)}]")]
    public NodeType Statement(NodeType SubStatement) => SubStatement;
    [Production("Expression Semicolon [d]")]
    public NodeType SCExpr(NodeType Expression) => Expression;
    [Production("Return [d] SCExpr")]
    public NodeType ReturnStatement(NodeType SCExpr) => SCExpr with { NodeType = ASTNodeType.Return };
    [Production($"[Break | Continue] {nameof(NestedValueInLoopControl)}?")]
    public NodeType LoopControlStatement(LyToken Operator, ValueOption<NodeType> NestedVal)
    {
        List<NodeType> rc = [];
        AppendIfNotEmpty(rc, NestedVal);
        return new(FromToken(Operator), rc, ASTNodeType.LoopCTRL);
    }
    [Production($"Identifier")]
    public NodeType NestedValueInLoopControl(LyToken val) => new NodeType(FromToken(val), [], ASTNodeType.ValInLCTRL);

    [Production($"[{nameof(ForLoopHeader)}) | {nameof(WhileLoopHeader)}] {nameof(LoopLabel)}? {nameof(Statement)} {nameof(Else)}?")]
    public NodeType Loop(NodeType LoopHeader, ValueOption<NodeType> Label, NodeType StatementExpr, ValueOption<NodeType> ElseExpr)
    {
        var rt = LoopHeader.NodeType;
        List<NodeType> rc = LoopHeader.Children;
        AppendIfNotEmpty(rc, Label);
        rc.Add(StatementExpr);
        AppendIfNotEmpty(rc, ElseExpr);
        return new NodeType(null, rc, rt);
    }
    [Production("As [d] Identifier")]
    public NodeType LoopLabel(LyToken ident) => new NodeType(FromToken(ident), [], ASTNodeType.LoopLabel);
    [Production($"For [d] OpenParen [d] {nameof(Expression)} Semicolon [d] {nameof(Expression)} Semicolon [d] {nameof(Expression)} CloseParen [d]")]
    public NodeType ForLoopHeader(NodeType Init, NodeType Condition, NodeType Step) => new NodeType(null, [Init, Condition, Step], ASTNodeType.For);
    [Production($"While [d] OpenParen [d] {nameof(Expression)} CloseParen [d]")]
    public NodeType WhileLoopHeader(NodeType Condition) => new NodeType(null, [Condition], ASTNodeType.While);
    [Production($"[{nameof(Cond)} | {nameof(If)}]")]
    public NodeType Cond(NodeType C) => C;
    [Production($"If [d] OpenParen [d] {nameof(Expression)} CloseParen [d] {nameof(Statement)} {nameof(Else)}?")]
    public NodeType If(NodeType Cond, NodeType StatementExpr, ValueOption<NodeType> ElseExpr)
    {
        var ThisCombined = new NodeType(null, [Cond, StatementExpr], ASTNodeType.ExprStatementCombined);
        List<NodeType> rc = [ThisCombined];
        if (GetFromValOp(ElseExpr) is NodeType node)
        {
            if (node.NodeType == ASTNodeType.If)
            {
                rc.AddRange(node.Children); //should automatically add the remaining elifs and else as well
            }
            else
            {
                rc.Add(node); //node is statement or child thereof (the latter is probably what it is but we don't care)
            }
        }
        return new(null, rc, ASTNodeType.If);
    }
    [Production($"Else [d] {nameof(Statement)}")]
    public NodeType Else(NodeType AStatement) => AStatement; //passthrough
    [Production($"Switch [d] OpenParen [d] {nameof(Expression)} CloseParen [d] OpenCurly [d] {nameof(SwitchBody)}* CloseCurly [d]")]
    public NodeType Switch(NodeType AExpression, List<NodeType> ASwitchBody) => new NodeType(null, [AExpression, .. ASwitchBody], ASTNodeType.Switch);

    [Production($"{nameof(Expression)} Colon [d] {nameof(Statement)}")]
    public NodeType SwitchBody(NodeType AExpr, NodeType AStatement) => new(null, [AExpr, AStatement], ASTNodeType.ExprStatementCombined);
    [Production($"{nameof(Type)} Identifier OpenParen [d] {nameof(TypeAndIdentifierCSV)}? CloseParen [d] {nameof(Statement)}")]
    public NodeType Function(NodeType Type, LyToken Ident, ValueOption<NodeType> TICSV, NodeType Statement) => new(FromToken(Ident), BuildChildren(Type, TICSV, Statement), ASTNodeType.Function);
    [Production($"{nameof(TypeAndIdentifierCSVElement)} (Comma [d] {nameof(TypeAndIdentifierCSV)})*")]
    public NodeType TypeAndIdentifierCSV(NodeType Element, List<Group<TokenType, NodeType>> Prime) => new(null, [Element, .. Prime.Select(x => x.Items.First().Value)], ASTNodeType.TypeAndIdentifierCSV);
    [Production($"{nameof(FunctionArgDeclModifiersCombined)}? {nameof(Type)} Identifier")]
    public NodeType TypeAndIdentifierCSVElement(ValueOption<NodeType> Modifiers, NodeType AType, LyToken Ident) => new(FromToken(Ident), BuildChildren(Modifiers, AType), ASTNodeType.TypeAndIdentifierCSVElement);
    [Production($"OpenCurly [d] {nameof(Section)} CloseCurly [d]")]
    public NodeType Block(NodeType Section) => Section;
    [Production($"{nameof(AliasExpr)}")]
    public NodeType Expression(NodeType pass) => pass;
    [Production($"[{nameof(AliasExpr1)} | {nameof(AliasExpr2)} | {nameof(AliasExpr3)} | {nameof(DeclarationExpr)}]")]
    public NodeType AliasExpr(NodeType Node) => Node;
    [Production($"Identifier As [d] Identifier")]
    public NodeType AliasExpr1(LyToken Ident, LyToken Ident2) => new(FromToken(Ident), BuildChildren(Ident2), ASTNodeType.AliasExpr);
    [Production($"Identifier As [d] {nameof(Type)} Identifier")]
    public NodeType AliasExpr2(LyToken Ident, NodeType Type, LyToken Ident2) => new(FromToken(Ident), BuildChildren(Type, Ident2), ASTNodeType.ReTypingAlias);
    [Production($"Identifier As [d] {nameof(Type)}")]
    public NodeType AliasExpr3(LyToken Ident, NodeType Type) => new(FromToken(Ident), BuildChildren(Type), ASTNodeType.ReTypeOriginal);
    [Production($"[{nameof(DeclarationExpr1)} | {nameof(AssignmentExpr)}]")]
    public NodeType DeclarationExpr(NodeType Node) => Node;
    [Production($"{nameof(DeclarationModifiersCombined)}? {nameof(Type)} Identifier {nameof(AssignmentPrime)}")]
    public NodeType DeclarationExpr1(ValueOption<NodeType> Modifiers, NodeType AType, LyToken Ident, ValueOption<NodeType> AAssignmentPrime) => new(FromToken(Ident), BuildChildren(Modifiers, AType, AAssignmentPrime), ASTNodeType.Declaration);
    [Production($"{nameof(DeclarationModifier)}+")]
    public NodeType DeclarationModifiersCombined(List<NodeType> Modifiers) => new(null, Modifiers, ASTNodeType.DeclarationModifiersCombined);
    [Production($"[Ref | Readonly | Frozen | Immut]")]
    public NodeType DeclarationModifier(LyToken Mod) => new(FromToken(Mod), [], ASTNodeType.DeclarationModifier);
    [Production($"[Ref | Readonly | Frozen | Immut | Copy]")]
    public NodeType FunctionArgDeclModifier(LyToken Mod) => DeclarationModifier(Mod) with { NodeType = ASTNodeType.FunctionArgDeclModifiers };
    [Production($"{nameof(FunctionArgDeclModifier)}+")]
    public NodeType FunctionArgDeclModifiersCombined(List<NodeType> Modifiers) => new(null, Modifiers, ASTNodeType.FunctionArgDeclModifiersCombined);
    [Production($"Equals {nameof(Expression)}")]
    public NodeType AssignmentPrime(LyToken EQ, NodeType Expr) => new(FromToken(EQ), [Expr], ASTNodeType.AssignmentPrime);
    [Production($"[{nameof(AssignmentExpr1)} | SmallLangParser_expressions]")]
    public NodeType AssignmentExpr(NodeType Node) => Node;
    [Production($"Identifier {nameof(AssignmentPrime)}?")]
    public NodeType AssignmentExpr1(LyToken Ident, ValueOption<NodeType> AAssignmentPrime) => new(FromToken(Ident), BuildChildren(AAssignmentPrime), ASTNodeType.AssignmentExpr);
    [Operand]
    [Production($"{nameof(LPrimary)} {nameof(PrimaryPrime)}?")]
    public NodeType Primary(NodeType APrimary, ValueOption<NodeType> Prime)
    {
        if (GetFromValOp(Prime) is NodeType NodePrime)
        {
            return new(NodePrime.Data, [APrimary, .. NodePrime.Children], NodePrime.NodeType);
        }
        else return APrimary;
    }
    [Production($"[{nameof(IndexPrime)} | {nameof(FunctionCallPrime)}]")]
    public NodeType PrimaryPrime(NodeType Node) => Node;
    [Production($"OpenSquare [d] {nameof(Expression)} CloseSquare [d]")]
    public NodeType IndexPrime(NodeType Expr) => new(null, [Expr], ASTNodeType.Index);
    [Production($"[{nameof(NewExpr)} | {nameof(LPrimary1)} | {nameof(LPrimary2)} | {nameof(LPrimary3)}]")]
    public NodeType LPrimary(NodeType Node) => Node;
    [Production($"OpenParen [d] {nameof(Expression)} CloseParen [d]")]
    public NodeType LPrimary1(NodeType Expr) => Expr;
    [Production($"Copy [d] {nameof(Expression)}")]
    public NodeType LPrimary2(NodeType Expr) => new(null, [Expr], ASTNodeType.CopyExpr);
    [Production($"[Identifier | Number | String | TrueLiteral | FalseLiteral]")]
    public NodeType LPrimary3(LyToken Token) => new(FromToken(Token), [], ASTNodeType.Primary);
    [Production($"New [d] {nameof(Type)} OpenParen [d] {nameof(ArgList)}?")]
    public NodeType NewExpr(NodeType AType, ValueOption<NodeType> AArgList) => new(null, BuildChildren(AType, AArgList), ASTNodeType.NewExpr);
    [Production($"Dot? OpenParen [d] {nameof(ArgList)} CloseParen [d]")]
    public NodeType FunctionCallPrime(ValueOption<LyToken> Dot, NodeType AArgList) => new(FromToken(Dot), BuildChildren(AArgList), ASTNodeType.FunctionCall);
    [Production($"{nameof(ArgListElement)} {nameof(ArgListPrime)}+")]
    public NodeType ArgList(NodeType Element, List<NodeType> Elements) => new(null, [Element, .. Elements], ASTNodeType.ArgList);
    [Production($"{nameof(ArgumentLabel)}? {nameof(Expression)}")]
    public NodeType ArgListElement(ValueOption<NodeType> Label, NodeType Expr) => new(null, BuildChildren(Label, Expr), ASTNodeType.ArgListElement);
    [Production($"Comma [d] {nameof(ArgListElement)}")]
    public NodeType ArgListPrime(NodeType Element) => Element;
    [Production($"Identifier Colon [d]")]
    public NodeType ArgumentLabel(LyToken Ident) => new(FromToken(Ident), [], ASTNodeType.Identifier);
    [Production($"{nameof(Type)} (Comma [d] {nameof(Type)})")]
    public NodeType TypeCSV(NodeType AType, List<Group<TokenType, NodeType>> OtherTypes)
    {
        return new(null, [AType, .. OtherTypes.Select(x => x.Items.First().Value)], ASTNodeType.TypeCSV);
    }
    [Production($"[{nameof(BaseType)} | {nameof(GenericType)}]")]
    public NodeType Type(NodeType Node) => Node;
    [Production($"[TypeArray | TypeList | TypeSet | TypeDict | TypeCollection] OpenAngleSquare [d] {nameof(TypeCSV)} CloseAngleSquare [d]")]
    public NodeType GenericType(LyToken TypeToken, NodeType TypeArgs) => new(FromToken(TypeToken), [TypeArgs], ASTNodeType.GenericType);
    [Production($"byte | short | int | long | bigint | float | double | rational | bigfloat | string | char | void")]
    public NodeType BaseType(LyToken TypeToken) => new(FromToken(TypeToken), [], ASTNodeType.BaseType);
}