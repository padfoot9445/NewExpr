using Common.AST;

namespace Common.Tokens;
record class Token : IToken
{
    public required TokenType TT { get; init; }

    public required string Lexeme { get; init; }
    public required string Literal { get; init; }
    public required int Position { get; init; }
    public ASTLeafType Type => ASTLeafType.Terminal;

    public static Token NewToken(TokenType type, string lexeme, int Position, string? literal = null)
    {
        return new Token() { TT = type, Lexeme = lexeme, Position = Position, Literal = literal ?? lexeme };
    }

    public ICollection<IValidASTLeaf> Flatten()
    {
        return [this];
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