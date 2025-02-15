using Common.Logger;
using Common.Tokens;

namespace Common.Parser;
public class ParserData(int start, int current, List<IToken> tokens, ILogger logger)
{
    public ParserData Copy()
    {
        return new ParserData(start, current, tokens.Select(x => x).ToList(), logger);
    }
    public int Start => start;
    public int Current => current;
    public bool AtEnd => Current == tokens.Count;
    public List<IToken> Tokens => tokens.Select(x => x).ToList();
    public ILogger Logger => logger;
    public IToken CurrentToken => GetToken();
    public IToken? SafeCurrentToken => SafeGetToken();
    public void BringStart()
    {
        start = current;
    }
    public IToken Advance(int Inc = 1)
    {
        current += Inc;
        return GetToken(-1);
    }
    public IToken Peek(int Inc = 1) => GetToken(Inc);
    public IToken GetToken(int rel = 0)
    {
        return tokens[Current + rel];
    }
    public IToken? SafeGetToken(int rel = 0)
    {
        if (Current + rel < tokens.Count)
        {
            return tokens[Current + rel];
        }
        return null;
    }

    public bool TCmp(params TokenType[] TTs) => TCmp(x => x.TT, (x, y) => x == y, TTs);
    public bool TCmp(Func<TokenType, TokenType, bool> Predicate, params TokenType[] TTs) => TCmp(x => x.TT, Predicate, TTs);
    public bool TCmp<T>(Func<IToken, T> Accessor, Func<T, T, bool> Comparer, params T[] TTs)
    {
        for (int i = 0; i < TTs.Length; i++)
        {
            var t = SafeGetToken(i);
            if (t == null)
            {
                return false;
            }
            if (!Comparer(Accessor(t), TTs[i]))
            {
                return false;
            }
        }
        return true;
    }
}