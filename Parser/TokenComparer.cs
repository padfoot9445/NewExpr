namespace Parser;
using Common.Tokens;
static class TokenComparer
{
    public static bool TCmp(this IToken? CurrentToken, TokenType tokenType)
    {
        return CurrentToken is not null && CurrentToken.TT == tokenType;

    }
    public static bool TCmp(this IToken? CurrentToken, IEnumerable<TokenType> set)
    {

        return CurrentToken is not null && set.Select(CurrentToken.TCmp).Contains(true);
    }

}