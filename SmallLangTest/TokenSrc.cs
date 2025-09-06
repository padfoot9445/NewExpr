using Common.Tokens;

namespace SmallLangTest;

internal static class TokenSrc
{
    public static IToken Addition => IToken.NewToken(TokenType.Addition, "+", -1);
    public static IToken Number => IToken.NewToken(TokenType.Number, "1", -2, "1");
    public static IToken Ident => IToken.NewToken(TokenType.Identifier, "RepOfI", -3, "RepOfI");
    public static IToken Number2 => IToken.NewToken(TokenType.Number, "0.1", -4, "0.1");
    public static IToken Multiplication => IToken.NewToken(TokenType.Multiplication, "*", -5);
}