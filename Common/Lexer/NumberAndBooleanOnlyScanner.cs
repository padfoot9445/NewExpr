using Common.Tokens;
namespace Common.Lexer;
class NumberAndBooleanOnlyScanner : Scanner
{
    public NumberAndBooleanOnlyScanner(string input) : base(input)
    {
    }
    protected override (string, TokenType)[] MCTTM => [
        ("int", TokenType.Int),
        ("float", TokenType.Float),
        ("and", TokenType.LogicalAnd),
        (">=", TokenType.GreaterThanOrEqualTo),
        ("<=", TokenType.LessThanOrEqualTo),
        ("<<", TokenType.BitwiseLeftShift),
        (">>", TokenType.BitwiseRightShift),
        ("==", TokenType.EqualTo),
        ("!=", TokenType.NotEqualTo),
        ("is", TokenType.ComparisonIs),
        ("in", TokenType.In),
        ("implies", TokenType.LogicalImplies),
        ("or", TokenType.LogicalOr),
        ("xor", TokenType.LogicalXor),
        ("not", TokenType.LogicalNot),
        ("(", TokenType.OpenParen),
        (")", TokenType.CloseParen),
        (",", TokenType.Comma),
        (";", TokenType.Semicolon),
        ("+", TokenType.Addition),
        ("-", TokenType.Subtraction),
        ("*", TokenType.Multiplication),
        ("**", TokenType.Exponentiation),
        ("/", TokenType.Division),
        ("~", TokenType.BitwiseNegation),
        ("^", TokenType.BitwiseXor),
        ("&", TokenType.BitwiseAnd),
        ("|", TokenType.BitwiseOr),
        (">", TokenType.GreaterThan),
        ("<", TokenType.LessThan),
        ("=", TokenType.Equals) ];
}