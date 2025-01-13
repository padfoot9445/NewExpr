namespace Common.Tokens;
using Common.AST;
public interface IToken : IValidASTLeaf
{
    public TokenType TT { get; }
    public string Lexeme { get; }
    public static IToken NewToken(TokenType type, string Lexeme, int position) => Token.NewToken(type, Lexeme);
}