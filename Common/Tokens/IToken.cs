namespace Common.Tokens;

public interface IToken
{
    public TokenType TT { get; }
    public string Lexeme { get; }
    public string Literal { get; }
    public int Position { get; }
    public int Line { get; }

    public static IToken NewToken(TokenType type, string Lexeme, int position, string? literal = null, int Line = -1)
    {
        return Token.NewToken(type, Lexeme, position, Line, literal);
    }
}