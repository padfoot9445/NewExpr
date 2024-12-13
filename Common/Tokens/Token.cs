namespace Common.Tokens;
record class Token : IToken
{
    public required TokenType TT { get; init; }

    public required string Lexeme { get; init; }

    public static Token NewToken(TokenType type, string lexeme)
    {
        return new Token() { TT = type, Lexeme = lexeme };
    }
}