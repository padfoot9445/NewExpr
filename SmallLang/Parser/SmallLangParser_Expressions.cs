using Common.Tokens;
using sly.parser.generator;
using LyToken = sly.lexer.Token<Common.Tokens.TokenType>;
using NodeType = Common.AST.DynamicASTNode<SmallLang.ASTNodeType, SmallLang.Attributes>;
namespace SmallLang.Parser;
public partial class SmallLangParser
{
    #region ComparisonExpressions
    const int EqualToPrecedence = NotExprPrecedence + 1;
    const int NotEqualToPrecedence = EqualToPrecedence;
    const int GreaterThanPrecedence = NotEqualToPrecedence;
    const int LessThanPrecedence = GreaterThanPrecedence;
    const int GreaterThanOrEqualToPrecedence = LessThanPrecedence;
    const int LessThanOrEqualToPrecedence = GreaterThanOrEqualToPrecedence;
    [Infix((int)TokenType.EqualTo, Associativity.Right, EqualToPrecedence)]
    [Infix((int)TokenType.NotEqualTo, Associativity.Right, NotEqualToPrecedence)]
    [Infix((int)TokenType.GreaterThan, Associativity.Right, GreaterThanPrecedence)]
    [Infix((int)TokenType.LessThan, Associativity.Right, LessThanPrecedence)]
    [Infix((int)TokenType.GreaterThanOrEqualTo, Associativity.Right, GreaterThanOrEqualToPrecedence)]
    [Infix((int)TokenType.LessThanOrEqualTo, Associativity.Right, LessThanOrEqualToPrecedence)]
    public NodeType ComparisonExpressions(NodeType Left, LyToken Operator, NodeType Right)
    {
        var OpToken = FromToken(Operator);
        List<NodeType> rc = [Left];
        if (Right.NodeType == ASTNodeType.ComparisionExpression)
        {
            rc.AddRange([
                new(OpToken, [Right.Children[0]], ASTNodeType.OperatorExpressionPair),
                ..Right.Children.Skip(1)
            ]);
        }
        else
        {
            rc.Add(new(OpToken, [Right], ASTNodeType.OperatorExpressionPair));
        }
        return new(null, rc, ASTNodeType.ComparisionExpression);
    }
    #endregion
    #region OtherExpressions
    const int AssignmentExprPrecedence = 1;
    const int ImpliesExprPrecedence = AssignmentExprPrecedence + 1;
    const int OrExprPrecedence = ImpliesExprPrecedence + 1;

    const int XorExprPrecedence = OrExprPrecedence + 1;

    const int AndExprPrecedence = XorExprPrecedence + 1;

    const int NotExprPrecedence = AndExprPrecedence + 1;
    const int AdditionPrecedence = EqualToPrecedence + 1;
    const int SubtractionPrecedence = AdditionPrecedence;

    const int MultiplicationPrecedence = SubtractionPrecedence + 1;
    const int DivisionPrecedence = MultiplicationPrecedence;

    const int PowerPrecedence = DivisionPrecedence + 1;

    const int NegationPrecedence = PowerPrecedence + 1;

    const int FactorialPrecedence = NegationPrecedence + 1;
    const int RShiftPrecedence = FactorialPrecedence + 1;
    const int LShiftPrecedence = RShiftPrecedence + 1;
    const int BitwiseOrExprPrecedence = LShiftPrecedence + 1;

    const int BitwiseXorExprPrecedence = BitwiseOrExprPrecedence + 1;

    const int BitwiseAndExprPrecedence = BitwiseXorExprPrecedence + 1;

    const int BitwiseNotExprPrecedence = BitwiseAndExprPrecedence + 1;
    #endregion

    [Postfix((int)TokenType.Factorial, Associativity.Left, FactorialPrecedence)]
    public NodeType Factorial(NodeType Left, LyToken Op)
    {
        if (Left.NodeType == ASTNodeType.FactorialExpression)
        {
            Left.Children.AddRange(BuildChildren(Op));
            return Left;
        }
        return new(null, BuildChildren(Op, Left), ASTNodeType.FactorialExpression);
    }
    [Prefix((int)TokenType.LogicalNot, Associativity.Right, NotExprPrecedence)]
    [Prefix((int)TokenType.BitwiseNegation, Associativity.Right, BitwiseNotExprPrecedence)]
    [Prefix((int)TokenType.Subtraction, Associativity.Right, NegationPrecedence)]
    public NodeType Prefix(LyToken Op, NodeType Right) => new(FromToken(Op), [Right], ASTNodeType.UnaryExpression);
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
        return new NodeType(FromToken(Op), [left, right], ASTNodeType.BinaryExpression);
    }
}