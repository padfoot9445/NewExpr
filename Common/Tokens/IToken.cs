namespace Common.Tokens;
using Common.AST;
public interface IToken : IValidASTLeaf
{
    public TokenType TT { get; }
    public string Lexeme { get; }
    public string Literal { get; }
    public int Position { get; }
    public static IToken NewToken(TokenType type, string Lexeme, int position, string? literal = null) => Token.NewToken(type, Lexeme, position, literal);
}