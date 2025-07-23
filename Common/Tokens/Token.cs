using Common.AST;

namespace Common.Tokens;

record class Token : IToken
{
    public required TokenType TT { get; init; }

    public required string Lexeme { get; init; }
    public required string Literal { get; init; }
    public required int Position { get; init; }
    public required int Line { get; init; }
    public static Token NewToken(TokenType type, string lexeme, int Position, int Line, string? literal = null)
    {
        return new Token() { TT = type, Lexeme = lexeme, Position = Position, Literal = literal ?? lexeme, Line = Line };
    }
}