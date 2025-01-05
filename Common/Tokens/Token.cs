using Common.AST;

namespace Common.Tokens;
record class Token : IToken
{
    public required TokenType TT { get; init; }

    public required string Lexeme { get; init; }

    public ASTLeafType Type => ASTLeafType.Terminal;

    public static Token NewToken(TokenType type, string lexeme)
    {
        return new Token() { TT = type, Lexeme = lexeme };
    }

    public bool IsEquivalentTo(IValidASTLeaf other)
    {
        return
            other is IToken t &&
            t.TT == TT &&
            Lexeme == t.Lexeme &&
            Type == t.Type
        ;
    }
}