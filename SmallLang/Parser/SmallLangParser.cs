
using System.Data;
using Common.Tokens;
using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using LyToken = sly.lexer.Token<Common.Tokens.TokenType>;
using NodeType = SmallLang.IR.AST.ISmallLangNode;
namespace SmallLang.Parser;

public partial class SmallLangParser
{
    TOut WrapFunction<TOut, TIn>(Func<TIn, TOut> Wrapped, TIn Input, string Name)
    {
        try
        {
            return Wrapped(Input);
        }
        catch
        {
            Console.WriteLine($"Exception in NodeHandler {Name}");
            throw;
        }
    }
    Func<NodeType, T> TryCast<T>(string NameOfCastingMethod) where T : class, NodeType
    => x => TryCast<T>(x, NameOfCastingMethod);
    T TryCast<T>(NodeType node, string NameOfCastingMethod) where T : class, NodeType
    {
        return WrapFunction(TryCast<T>, node, NameOfCastingMethod);
    }
    T? TryCast<T>(ValueOption<NodeType> node, string NameOfCastingMethod) where T : class, NodeType
    {
        return WrapFunction(TryCast<T>, node, NameOfCastingMethod);
    }
    T TryCast<T>(NodeType Node) where T : class, NodeType
    {
        if (Node is T) return (T)Node;
        else throw new Exception($"Tried to cast node {Node} of type {Node.GetType()} to {typeof(T)}");
    }

    T? TryCast<T>(ValueOption<NodeType> Node) where T : class, NodeType
    {

        if (Node.IsSome)
        {
            return TryCast<T>(Node.Match(x => x, () => throw new Exception("IsSome was true when no value was found")));
        }
        return null;
    }

    SectionNode ToSection<T>(NodeType Node, string HandlerName) where T : class, IStatementNode
    {
        if (Node is SectionNode sectionNode) return sectionNode;
        else if (Node is T Statement) return new([Statement]);
        else throw new Exception($"Tried to cast Node to Section in {HandlerName}. Expected : {typeof(T)} or SectionNode but got {Node.GetType()}");
    }

    IToken FromToken(LyToken t) => t.IsEmpty ? throw new Exception($"Token {t} was empty") : IToken.NewToken(t.TokenID, t.Value, t.Position.Index, null, t.Position.Line);
    IToken? TryFromToken(LyToken t) => t.IsEmpty ? null : FromToken(t);
    [Production($"{nameof(NTSection)}: {nameof(NTStatement)}*")]
    public NodeType NTSection(List<NodeType> Statements)
    {
        return new SectionNode(Statements.Select(TryCast<IStatementNode>).ToList());
    }
    [Production($"{nameof(NTStatement)}: [{nameof(NTSCExpr)} | {nameof(NTLoop)} | {nameof(NTCond)} | {nameof(NTFunction)} | {nameof(NTBlock)} | {nameof(NTReturnStatement)} | {nameof(NTLoopControlStatement)}]")]
    public NodeType NTStatement(NodeType SubStatement) => SubStatement;
    [Production($"{nameof(NTSCExpr)}: {nameof(NTExpression)} Semicolon [d]")]

    public NodeType NTSCExpr(NodeType Expression) => Expression;
    [Production($"{nameof(NTReturnStatement)}: Return [d] {nameof(NTExpression)} Semicolon [d]")]
    public NodeType NTReturnStatement(NodeType SCExpr) => new ReturnNode((IExpressionNode)SCExpr);
    [Production($"{nameof(NTLoopControlStatement)}: [Break | Continue] {nameof(NTNestedValueInLoopControl)}? Semicolon [d]")]
    public NodeType NTLoopControlStatement(LyToken Operator, ValueOption<NodeType> NestedVal)
    {
        return new LoopCTRLNode(FromToken(Operator), TryCast<IdentifierNode>(NestedVal));
    }
    [Production($"{nameof(NTNestedValueInLoopControl)}: Identifier")]
    public NodeType NTNestedValueInLoopControl(LyToken val) => new IdentifierNode(FromToken(val));

    [Production($"{nameof(NTLoop)}: {nameof(NTForLoop)}")]
    [Production($"{nameof(NTLoop)}: {nameof(NTWhileLoop)}")]
    public NodeType NTLoop(NodeType Loop) => Loop;
    [Production($"{nameof(NTLoopLabel)}: As [d] Identifier")]
    public NodeType NTLoopLabel(LyToken ident) => new LoopLabelNode(FromToken(ident));
    [Production($"{nameof(NTForLoop)}: For [d] OpenParen [d] {nameof(NTExpression)} Semicolon [d] {nameof(NTExpression)} Semicolon [d] {nameof(NTExpression)} CloseParen [d] {nameof(NTLoopLabel)}? {nameof(NTStatement)} {nameof(NTElse)}?")]
    public NodeType NTForLoop(NodeType Init, NodeType Condition, NodeType Step, ValueOption<NodeType> LoopLabel, NodeType Statement, ValueOption<NodeType> Else) => new ForNode(
        TryCast<IExpressionNode>(Init, nameof(NTForLoop)),
        TryCast<IExpressionNode>(Condition, nameof(NTForLoop)),
        TryCast<IExpressionNode>(Step, nameof(NTForLoop)),
        TryCast<LoopLabelNode>(LoopLabel, nameof(NTForLoop)),
        ToSection<IStatementNode>(Statement, nameof(NTForLoop)),
        TryCast<ElseNode>(Else, nameof(NTForLoop))
    );
    [Production($"{nameof(NTWhileLoop)}: While [d] OpenParen [d] {nameof(NTExpression)} CloseParen [d] {nameof(NTLoopLabel)}? {nameof(NTStatement)} {nameof(NTElse)}?")]
    public NodeType NTWhileLoop(NodeType Condition, ValueOption<NodeType> LoopLabel, NodeType Statement, ValueOption<NodeType> Else) => new WhileNode(
        TryCast<IExpressionNode>(Condition, nameof(NTWhileLoop)),
        TryCast<LoopLabelNode>(LoopLabel, nameof(NTWhileLoop)),
        ToSection<IStatementNode>(Statement, nameof(NTWhileLoop)),
        TryCast<ElseNode>(Else, nameof(NTWhileLoop))
    );
    [Production($"{nameof(NTCond)}: [{nameof(NTSwitch)} | {nameof(NTIf)}]")]
    public NodeType NTCond(NodeType C) => C;
    [Production($"{nameof(NTIf)}: If [d] OpenParen [d] {nameof(NTExpression)} CloseParen [d] {nameof(NTStatement)} {nameof(NTElse)}?")]
    public NodeType NTIf(NodeType Cond, NodeType StatementExpr, ValueOption<NodeType> ElseExpr)
    {
        var ElseNode = TryCast<ElseNode>(ElseExpr);

        var ThisCombined = new ExprSectionCombinedNode(TryCast<IExpressionNode>(Cond), ToSection<IStatementNode>(StatementExpr, nameof(NTIf)));
        List<ExprSectionCombinedNode> rc = [ThisCombined];
        if (ElseNode is not null)
        {
            if (ElseNode.Statement is IfNode If)
            {
                rc.AddRange(If.ExprStatementCombineds);
                ElseNode = If.Else;
            }
        }
        return new IfNode(rc, ElseNode);

        //TODO: look at groups for a more natural implementation
    }


    [Production($"{nameof(NTElse)}: Else [d] {nameof(NTStatement)}")]
    public NodeType NTElse(NodeType AStatement) => new ElseNode(TryCast<IStatementNode>(AStatement, nameof(NTElse))); //passthrough
    [Production($"{nameof(NTSwitch)}: Switch [d] OpenParen [d] {nameof(NTExpression)} CloseParen [d] OpenCurly [d] {nameof(NTSwitchBody)}* CloseCurly [d]")]
    public NodeType NTSwitch(NodeType AExpression, List<NodeType> ASwitchBody) => new SwitchNode(
        TryCast<IExpressionNode>(AExpression), ASwitchBody.Select(TryCast<ExprSectionCombinedNode>).ToList());

    [Production($"{nameof(NTSwitchBody)}: {nameof(NTExpression)} Colon [d] {nameof(NTStatement)}")]
    public NodeType NTSwitchBody(NodeType AExpr, NodeType AStatement) => new ExprSectionCombinedNode(TryCast<IExpressionNode>(AExpr, nameof(NTSwitchBody)),
    ToSection<IStatementNode>(AStatement, nameof(NTSwitchBody)));
    [Production($"{nameof(NTFunction)}: {nameof(NTType)} Identifier OpenParen [d] {nameof(NTTypeAndIdentifierCSVElement)}* CloseParen [d] {nameof(NTStatement)}")]
    public NodeType NTFunction(NodeType AType, LyToken Ident, List<NodeType> TICSV, NodeType Statement)
    {
        var OutSection = ToSection<IStatementNode>(Statement, nameof(NTFunction));
        List<IStatementNode> Copies = [];

        foreach (var Element in TICSV)
        {
            var TypeAndIdentifier = TryCast<TypeAndIdentifierCSVElementNode>(Element);
            if (TypeAndIdentifier.FunctionArgDeclModifiersCombined.FunctionArgDeclModifierss.Any(x => x.Data.TT == TokenType.Copy))
            {
                Copies.Add(new BinaryExpressionNode(
                    Data: IToken.NewToken(TokenType.Equals, "Inserted Assignment at function definition of Copy parameter", -1, null, -1),
                    Left: new IdentifierNode(TypeAndIdentifier.Data),
                    Right: new CopyExprNode(new IdentifierNode(TypeAndIdentifier.Data)) // different object to emulate normal behavior more closely
                ));
            }
        }


        OutSection = OutSection with { Statements = [.. Copies, .. OutSection.Statements] };


        return new FunctionNode(FromToken(Ident), TryCast<ITypeNode>(AType, nameof(NTFunction)), TICSV.Select(TryCast<TypeAndIdentifierCSVElementNode>(nameof(NTFunction))).ToList(), OutSection);
    }
    [Production($"{nameof(NTTypeAndIdentifierCSVElement)}: Comma? {nameof(NTFunctionArgDeclModifiersCombined)} {nameof(NTType)} Identifier")]
    public NodeType NTTypeAndIdentifierCSVElement(LyToken _, NodeType Modifiers, NodeType AType, LyToken Ident) => new TypeAndIdentifierCSVElementNode(FromToken(Ident), TryCast<FunctionArgDeclModifiersCombinedNode>(Modifiers), TryCast<ITypeNode>(AType));
    [Production($"{nameof(NTBlock)}: OpenCurly [d] {nameof(NTSection)} CloseCurly [d]")]
    public NodeType NTBlock(NodeType ASection) => ASection;
    [Production($"{nameof(NTExpression)}: {nameof(NTAliasExpr)}")]
    public NodeType NTExpression(NodeType pass) => pass;
    [Production($"{nameof(NTAliasExpr)}: [{nameof(NTAliasExpr1)} | {nameof(NTAliasExpr2)} | {nameof(NTAliasExpr3)} | {nameof(NTDeclarationExpr)}]")]
    public NodeType NTAliasExpr(NodeType Node) => Node;
    [Production($"{nameof(NTAliasExpr1)}: Identifier As [d] Identifier")]
    public NodeType NTAliasExpr1(LyToken Ident, LyToken Ident2) => new AliasExprNode(FromToken(Ident), new IdentifierNode(FromToken(Ident2)));
    [Production($"{nameof(NTAliasExpr2)}: Identifier As [d] {nameof(NTType)} Identifier")]
    public NodeType NTAliasExpr2(LyToken Ident, NodeType AType, LyToken Ident2) => new ReTypingAliasNode(FromToken(Ident), TryCast<ITypeNode>(AType), new IdentifierNode(FromToken(Ident2)));
    [Production($"{nameof(NTAliasExpr3)}: Identifier As [d] {nameof(NTType)}")]
    public NodeType NTAliasExpr3(LyToken Ident, NodeType Type) => new ReTypeOriginalNode(FromToken(Ident), TryCast<ITypeNode>(Type));
    [Production($"{nameof(NTDeclarationExpr)}: [{nameof(NTDeclarationExpr1)} | {nameof(NTAssignmentExpr)}]")]
    public NodeType NTDeclarationExpr(NodeType Node) => Node;
    [Production($"{nameof(NTDeclarationExpr1)}: {nameof(NTDeclarationModifiersCombined)}? {nameof(NTType)} Identifier {nameof(NTAssignmentPrime)}?")]
    public NodeType NTDeclarationExpr1(ValueOption<NodeType> Modifiers, NodeType AType, LyToken Ident, ValueOption<NodeType> AAssignmentPrime) => new DeclarationNode(FromToken(Ident), TryCast<ITypeNode>(AType), TryCast<DeclarationModifiersCombinedNode>(Modifiers) ?? new DeclarationModifiersCombinedNode([]), TryCast<AssignmentPrimeNode>(AAssignmentPrime));
    [Production($"{nameof(NTDeclarationModifiersCombined)}: {nameof(NTDeclarationModifier)}*")]
    public NodeType NTDeclarationModifiersCombined(List<NodeType> Modifiers) => new DeclarationModifiersCombinedNode(Modifiers.Select(TryCast<DeclarationModifierNode>).ToList());
    [Production($"{nameof(NTDeclarationModifier)}: [Ref | Readonly | Frozen | Immut]")]
    public NodeType NTDeclarationModifier(LyToken Mod) => new DeclarationModifierNode(FromToken(Mod));
    [Production($"{nameof(NTFunctionArgDeclModifier)}: [Ref | Readonly | Frozen | Immut | Copy]")]
    public NodeType NTFunctionArgDeclModifier(LyToken Mod) => new FunctionArgDeclModifiersNode(FromToken(Mod));
    [Production($"{nameof(NTFunctionArgDeclModifiersCombined)}: {nameof(NTFunctionArgDeclModifier)}*")]
    public NodeType NTFunctionArgDeclModifiersCombined(List<NodeType> Modifiers) => new FunctionArgDeclModifiersCombinedNode(Modifiers.Select(TryCast<FunctionArgDeclModifiersNode>).ToList());
    [Production($"{nameof(NTAssignmentPrime)}: Equals {nameof(NTExpression)}")]
    public NodeType NTAssignmentPrime(LyToken EQ, NodeType Expr) => new AssignmentPrimeNode(FromToken(EQ), TryCast<IExpressionNode>(Expr));
    [Production($"{nameof(NTAssignmentExpr)}: {nameof(NTAssignmentExpr1)}")]
    public NodeType NTAssignmentExpr(NodeType Node) => Node;
    [Production($"{nameof(NTAssignmentExpr1)}: SmallLangParser_expressions")]
    public NodeType NTAssignmentExpr1(NodeType P1) => P1;
    [Operand]
    [Production($"{nameof(NTPrimary)}: {nameof(NTLPrimary)}")]
    public NodeType NTPrimary(NodeType Node) => Node;

    [Operand]
    [Production($"{nameof(NTPrimary)}: {nameof(NTLPrimary)} OpenSquare {nameof(NTExpression)} CloseSquare")]
    public NodeType NTPrimary(NodeType Node, LyToken Open, NodeType Expr, LyToken Close)
    {
        return new IndexNode(TryCast<IExpressionNode>(Node), TryCast<IExpressionNode>(Expr));
    }

    [Operand]
    [Production($"{nameof(NTPrimary)}: {nameof(NTLPrimary)}  OpenParen {nameof(NTArgListElement)}* CloseParen")]
    public NodeType NTPrimary(NodeType Node, LyToken Open, List<NodeType> Expression, LyToken Close)
    {

        return new FunctionCallNode(TryCast<IExpressionNode>(Node), Expression.Select(TryCast<ArgListElementNode>).ToList());

    }


    [Production($"{nameof(NTLPrimary)}: [{nameof(NTNewExpr)} | {nameof(NTLPrimary1)} | {nameof(NTLPrimary2)} | {nameof(NTLPrimary3)}]")]
    public NodeType NTLPrimary(NodeType Node) => Node;
    [Production($"{nameof(NTLPrimary1)}: OpenParen [d] {nameof(NTExpression)} CloseParen [d]")]
    public NodeType NTLPrimary1(NodeType Expr) => Expr;
    [Production($"{nameof(NTLPrimary2)}: Copy [d] {nameof(NTExpression)}")]
    public NodeType NTLPrimary2(NodeType Expr) => new CopyExprNode(TryCast<IExpressionNode>(Expr));
    [Production($"{nameof(NTLPrimary3)}: [Identifier | Number | String | TrueLiteral | FalseLiteral]")]
    public NodeType NTLPrimary3(LyToken Token)
    {
        IToken oT = FromToken(Token)!;
        return oT.TT == TokenType.Identifier ? new IdentifierNode(oT) : new PrimaryNode(oT);
    }
    [Production($"{nameof(NTNewExpr)}: New [d] {nameof(NTType)} OpenParen [d] {nameof(NTArgListElement)}* CloseParen [d]")]
    public NodeType NTNewExpr(NodeType AType, List<NodeType> AArgList) => new NewExprNode(TryCast<ITypeNode>(AType), AArgList.Select(TryCast<ArgListElementNode>).ToList());
    [Production($"{nameof(NTArgListElement)}: Comma? {nameof(NTArgumentLabel)}? {nameof(NTExpression)}")]
    public NodeType NTArgListElement(LyToken _, ValueOption<NodeType> Label, NodeType Expr) => new ArgListElementNode(TryCast<IExpressionNode>(Expr), TryCast<IdentifierNode>(Label));
    public NodeType NTArgListPrime(NodeType Element) => Element;
    [Production($"{nameof(NTArgumentLabel)}: Identifier Colon [d]")]
    public NodeType NTArgumentLabel(LyToken Ident) => new IdentifierNode(FromToken(Ident));
    [Production($"{nameof(NTTypeCSV)}: {nameof(NTType)} (Comma [d] {nameof(NTType)})*")]
    public NodeType NTTypeCSV(NodeType AType, List<Group<TokenType, NodeType>> OtherTypes)
    {
        return new TypeCSVNode([TryCast<ITypeNode>(AType), .. OtherTypes.Select(x => x.Items.First().Value).Select(TryCast<ITypeNode>)]);
    }
    [Production($"{nameof(NTType)}: [{nameof(NTBaseType)} | {nameof(NTGenericType)}]")]
    public NodeType NTType(NodeType Node) => Node;
    [Production($"{nameof(NTGenericType)}: [TypeArray | TypeList | TypeSet | TypeDict | TypeCollection] OpenAngleSquare [d] {nameof(NTTypeCSV)} CloseAngleSquare [d]")]
    public NodeType NTGenericType(LyToken TypeToken, NodeType TypeArgs) => new GenericTypeNode(FromToken(TypeToken), TryCast<TypeCSVNode>(TypeArgs).Types);
    [Production($"{nameof(NTBaseType)}: [TypeByte | TypeShort | TypeInt | TypeLong | TypeLongInt | TypeFloat | TypeDouble | TypeRational | TypeNumber | TypeString | TypeChar | TypeVoid]")]
    public NodeType NTBaseType(LyToken TypeToken) => new BaseTypeNode(FromToken(TypeToken));
}