using Common.Tokens;

namespace Common.Lexer;
abstract class Scanner : IScanner
{
    public Scanner(string input)
    {
        this.input = input;
    }
    public string input { get; set; }
    int Current = 0;
    int Start = 0;
    protected virtual ICollection<char> WhiteSpace { get; } = new HashSet<char>() { ' ', '\t', '\n', '\r' };
    public IEnumerable<(string, TokenType)> SymbolTTMappingSortedReverse => MCTTM.OrderBy(x => x.Item1.Length).Reverse();
    protected abstract IEnumerable<(string, TokenType)> MCTTM { get; }
    bool GetTT(out TokenType TT, out int Length)
    {
        foreach (var kvp in SymbolTTMappingSortedReverse)
        {//for every keyword, we see if we can match the input string to it. We start with the longer keywords(SymbolTTMappingSortedReverse is, as its name suggests, sorted in reverse), so we definitely match the longest valid kw.
            string keyword = kvp.Item1;
            if (StrEq(Start, Current + keyword.Length, keyword))
            {
                TT = kvp.Item2;
                Length = keyword.Length;
                return true;
            }
        }
        TT = (TokenType)(-1);
        Length = -1;
        return false;

    }
    void SkipWhiteSpace()
    {
        while (Current < input.Length && WhiteSpace.Contains(input[Current]))
        {
            Current++;
        }
    }
    public IEnumerable<IToken> Scan()
    {
        while (Current < input.Length)
        {
            SkipWhiteSpace();
            Start = Current;
            if (GetTT(out TokenType TT, out int length))
            {
                //add keyword tokens
                Current += length;
                yield return IToken.NewToken(TT, input[Start..Current], Current);
                continue;
            }
            //or else, get the literal - number, identifier, string etc
            yield return GetLiteral();
        }
    }
    IToken GetLiteral()
    {
        bool ValidForNum = IsNum(input[Current]);
        bool ValidForIdent = IsValidFirstIdentChar(input[Current]);
        while (Current < input.Length && IsNum(input[Current]))
        {
            ValidForNum &= IsNum(input[Current]);
            Current++;
        }
        while (Current < input.Length && IsValidFirstIdentChar(input[Current]))
        {
            ValidForIdent &= IsValidFirstIdentChar(input[Current]);
            Current++;
        }
        if (ValidForNum)
        {
            return IToken.NewToken(TokenType.Number, input[Start..Current], Current);
        }
        else if (ValidForIdent)
        {
            return IToken.NewToken(TokenType.Identifier, input[Start..Current], Current);
        }
        else
        {
            throw new Exception($"Invalid character string {input[Start..Current]} parsed at before {Current}");
        }
    }
    static bool IsNum(char c)
    {
        return c >= '0' && c <= '9';
    }
    static bool IsValidFirstIdentChar(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }
    bool StrEq(int start, int end, string value)
    {
        if (input.Length >= end)
        {
            return input[start..end] == value;
        }
        return false;
    }
}