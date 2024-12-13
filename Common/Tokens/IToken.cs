namespace Common.Tokens;
public interface IToken
{
    public TokenType TT { get; }
    public string Lexeme { get; }
    public static IToken NewToken(TokenType type, string Lexeme, int position) => Token.NewToken(type, Lexeme);
}