using Common.Tokens;
using sly.parser.generator;
using SmallLang.IR.AST.Generated;
using LyToken = sly.lexer.Token<Common.Tokens.TokenType>;
using NodeType = SmallLang.IR.AST.ISmallLangNode;

namespace SmallLang.Parser;

public partial class SmallLangParser
{
    [Postfix((int)TokenType.Factorial, Associativity.Left, FactorialPrecedence)]
    public NodeType Factorial(NodeType Left, LyToken Op)
    {
        if (Left is FactorialExpressionNode Factorial)
            Factorial.FactorialSymbols.Add(new FactorialSymbolNode(FromToken(Op)));
        return new FactorialExpressionNode(TryCast<IExpressionNode>(Left), [new FactorialSymbolNode(FromToken(Op))]);
    }

    [Prefix((int)TokenType.LogicalNot, Associativity.Right, NotExprPrecedence)]
    [Prefix((int)TokenType.BitwiseNegation, Associativity.Right, BitwiseNotExprPrecedence)]
    [Prefix((int)TokenType.Subtraction, Associativity.Right, NegationPrecedence)]
    public NodeType Prefix(LyToken Op, NodeType Right)
    {
        return new UnaryExpressionNode(FromToken(Op), TryCast<IExpressionNode>(Right));
    }

    [Infix((int)TokenType.Equals, Associativity.Right, AssignmentExprPrecedence)]
    [Infix((int)TokenType.LogicalOr, Associativity.Left, OrExprPrecedence)]
    [Infix((int)TokenType.LogicalXor, Associativity.Left, XorExprPrecedence)]
    [Infix((int)TokenType.LogicalAnd, Associativity.Left, AndExprPrecedence)]
    [Infix((int)TokenType.Addition, Associativity.Left, AdditionPrecedence)]
    [Infix((int)TokenType.Subtraction, Associativity.Left, SubtractionPrecedence)]
    [Infix((int)TokenType.Division, Associativity.Left, DivisionPrecedence)]
    [Infix((int)TokenType.Multiplication, Associativity.Left, MultiplicationPrecedence)]
    [Infix((int)TokenType.Exponentiation, Associativity.Right, PowerPrecedence)]
    [Infix((int)TokenType.Subtraction, Associativity.Left, NegationPrecedence)]
    [Infix((int)TokenType.BitwiseOr, Associativity.Left, BitwiseOrExprPrecedence)]
    [Infix((int)TokenType.BitwiseXor, Associativity.Left, BitwiseXorExprPrecedence)]
    [Infix((int)TokenType.BitwiseAnd, Associativity.Left, BitwiseAndExprPrecedence)]
    [Infix((int)TokenType.BitwiseLeftShift, Associativity.Left, LShiftPrecedence)]
    [Infix((int)TokenType.BitwiseRightShift, Associativity.Left, RShiftPrecedence)]
    public NodeType BinOp(NodeType left, LyToken Op, NodeType right)
    {
        return new BinaryExpressionNode(FromToken(Op), TryCast<IExpressionNode>(left), TryCast<IExpressionNode>(right));
    }

    [Infix((int)TokenType.PlusEquals, Associativity.Right, PlusEqualsExprPrecedence)]
    [Infix((int)TokenType.MinusEquals, Associativity.Right, MinusEqualsExprPrecedence)]
    [Infix((int)TokenType.MultiplicationEquals, Associativity.Right, MultiplicationEqualsExprPrecedence)]
    [Infix((int)TokenType.DivideEquals, Associativity.Right, DivisionEqualsExprPrecedence)]
    [Infix((int)TokenType.PowerEquals, Associativity.Right, PowerPrecedence)]
    [Infix((int)TokenType.BitwiseAndEquals, Associativity.Right, BitwiseAndEqualsPrecedence)]
    [Infix((int)TokenType.BitwiseOrEquals, Associativity.Right, BitwiseOrEqualsPrecedence)]
    [Infix((int)TokenType.BitwiseXorEquals, Associativity.Right, BitwiseXorEqualsPrecedence)]
    [Infix((int)TokenType.BitwiseNegateEquals, Associativity.Right, BitwiseNotEqualsPrecedence)]
    [Infix((int)TokenType.LeftShiftEquals, Associativity.Right, BitwiseLeftShiftEqualsPrecedence)]
    [Infix((int)TokenType.RightShiftEquals, Associativity.Right, BitwiseRightShiftEqualsPrecedence)]
    public static NodeType AssignmentOp(NodeType left, LyToken Op, NodeType right)
    {
        TokenType NewType;
        switch (Op.TokenID)
        {
            case TokenType.PlusEquals:
                NewType = TokenType.Addition;
                break;
            case TokenType.MinusEquals:
                NewType = TokenType.Subtraction;
                break;
            case TokenType.MultiplicationEquals:
                NewType = TokenType.Multiplication;
                break;
            case TokenType.DivideEquals:
                NewType = TokenType.Division;
                break;
            case TokenType.PowerEquals:
                NewType = TokenType.Exponentiation;
                break;
            case TokenType.BitwiseAndEquals:
                NewType = TokenType.BitwiseAnd;
                break;
            case TokenType.BitwiseOrEquals:
                NewType = TokenType.BitwiseOr;
                break;
            case TokenType.BitwiseXorEquals:
                NewType = TokenType.BitwiseXor;
                break;
            case TokenType.BitwiseNegateEquals:
                NewType = TokenType.BitwiseNegation;
                break;
            case TokenType.LeftShiftEquals:
                NewType = TokenType.BitwiseLeftShift; break;
            case TokenType.RightShiftEquals:
                NewType = TokenType.BitwiseRightShift; break;
            default:
                throw new Exception("Unexpected");
        }

        return new BinaryExpressionNode(
            IToken.NewToken(TokenType.Equals, "Autogenerated equals", Op.Position.Index, Line: Op.Position.Line),
            TryCast<IExpressionNode>(left),
            new BinaryExpressionNode(
                IToken.NewToken(NewType, "InsertedOp", Op.Position.Index, Line: Op.Position.Line),
                TryCast<IExpressionNode>(left),
                TryCast<IExpressionNode>(right)
            ));
    }

    [Prefix((int)TokenType.Increment, Associativity.Left, IncrementExprPrecedence)]
    [Prefix((int)TokenType.Decrement, Associativity.Right, DecrementExprPrecedence)]
    public static NodeType PreCrementOp(LyToken left, NodeType right)
    {
        var RExp = TryCast<IExpressionNode>(right);
        var newToken = left.TokenID switch
        {
            TokenType.Increment => TokenType.Addition,
            TokenType.Decrement => TokenType.Subtraction,
            _ => throw new InvalidOperationException("Invalid token type for post increment/decrement")
        };
        return new BinaryExpressionNode
        (
            IToken.NewToken(TokenType.Equals, "InsertedPostcrement", left.Position.Index, Line: left.Position.Line),
            RExp,
            new BinaryExpressionNode
            (
                IToken.NewToken(newToken, "InsertedPostcrement", left.Position.Index, Line: left.Position.Line),
                RExp,
                new PrimaryNode(IToken.NewToken(TokenType.Number, "1", left.Position.Index, Line: left.Position.Line))
            ));
    }

    #region ComparisonExpressions

    private const int EqualToPrecedence = PlusEqualsExprPrecedence + 1;
    private const int NotEqualToPrecedence = EqualToPrecedence;
    private const int GreaterThanPrecedence = NotEqualToPrecedence;
    private const int LessThanPrecedence = GreaterThanPrecedence;
    private const int GreaterThanOrEqualToPrecedence = LessThanPrecedence;
    private const int LessThanOrEqualToPrecedence = GreaterThanOrEqualToPrecedence;

    [Infix((int)TokenType.EqualTo, Associativity.Right, EqualToPrecedence)]
    [Infix((int)TokenType.NotEqualTo, Associativity.Right, NotEqualToPrecedence)]
    [Infix((int)TokenType.GreaterThan, Associativity.Right, GreaterThanPrecedence)]
    [Infix((int)TokenType.LessThan, Associativity.Right, LessThanPrecedence)]
    [Infix((int)TokenType.GreaterThanOrEqualTo, Associativity.Right, GreaterThanOrEqualToPrecedence)]
    [Infix((int)TokenType.LessThanOrEqualTo, Associativity.Right, LessThanOrEqualToPrecedence)]
    public NodeType ComparisonExpressions(NodeType Left, LyToken Operator, NodeType Right)
    {
        var OpToken = FromToken(Operator);

        var Expr = TryCast<IExpressionNode>(Left);

        if (Right is ComparisonExpressionNode RC)
            return new ComparisonExpressionNode(Expr, [
                new OperatorExpressionPairNode(OpToken, RC.Expression),
                ..RC.OperatorExpressionPairs
            ]);

        return new ComparisonExpressionNode(Expr,
            [new OperatorExpressionPairNode(OpToken, TryCast<IExpressionNode>(Right))]);
    }

    #endregion

    #region OtherExpressions

    private const int AssignmentExprPrecedence = 1;
    private const int ImpliesExprPrecedence = AssignmentExprPrecedence + 1;
    private const int OrExprPrecedence = ImpliesExprPrecedence + 1;

    private const int XorExprPrecedence = OrExprPrecedence + 1;

    private const int AndExprPrecedence = XorExprPrecedence + 1;

    private const int NotExprPrecedence = AndExprPrecedence + 1;
    private const int PlusEqualsExprPrecedence = NotExprPrecedence + 1;
    private const int MinusEqualsExprPrecedence = PlusEqualsExprPrecedence;
    private const int MultiplicationEqualsExprPrecedence = MinusEqualsExprPrecedence;
    private const int DivisionEqualsExprPrecedence = MultiplicationEqualsExprPrecedence;
    private const int BitwiseAndEqualsPrecedence = DivisionEqualsExprPrecedence;
    private const int BitwiseOrEqualsPrecedence = BitwiseAndEqualsPrecedence;
    private const int BitwiseXorEqualsPrecedence = BitwiseOrEqualsPrecedence;
    private const int BitwiseNotEqualsPrecedence = BitwiseXorEqualsPrecedence;

    private const int BitwiseLeftShiftEqualsPrecedence = BitwiseNotEqualsPrecedence;
    private const int BitwiseRightShiftEqualsPrecedence = BitwiseLeftShiftEqualsPrecedence;
    private const int AdditionPrecedence = EqualToPrecedence + 1;
    private const int SubtractionPrecedence = AdditionPrecedence;

    private const int MultiplicationPrecedence = SubtractionPrecedence + 1;
    private const int DivisionPrecedence = MultiplicationPrecedence;

    private const int PowerPrecedence = DivisionPrecedence + 1;

    private const int NegationPrecedence = PowerPrecedence + 1;

    private const int FactorialPrecedence = NegationPrecedence + 1;
    private const int RShiftPrecedence = FactorialPrecedence + 1;
    private const int LShiftPrecedence = RShiftPrecedence + 1;
    private const int BitwiseOrExprPrecedence = LShiftPrecedence + 1;

    private const int BitwiseXorExprPrecedence = BitwiseOrExprPrecedence + 1;

    private const int BitwiseAndExprPrecedence = BitwiseXorExprPrecedence + 1;

    private const int BitwiseNotExprPrecedence = BitwiseAndExprPrecedence + 1;
    private const int IncrementExprPrecedence = BitwiseNotExprPrecedence + 1;
    private const int DecrementExprPrecedence = IncrementExprPrecedence;

    #endregion
}