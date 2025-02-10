using System.Text;
using Common.Tokens;

namespace Common.Lexer;
public abstract class Scanner : IScanner
{
    public Scanner(string input)
    {
        this.input = input.Trim();
    }
    public string input { get; set; }
    int Current = 0;
    int Start = 0;
    protected virtual ICollection<char> WhiteSpace { get; } = new HashSet<char>() { ' ', '\t', '\n', '\r' };
    public IEnumerable<(string, TokenType)> SymbolTTMappingSortedReverse => MCTTM.OrderBy(x => x.Item1.Length).Reverse();
    protected abstract IEnumerable<(string, TokenType)> MCTTM { get; }
    protected virtual ICollection<string> EOLCommentBegin { get; } = [];
    protected virtual IList<string> StartEndCommentBegin { get; } = [];
    protected virtual IList<string> StartEndCommentEnd { get; } = [];
    protected virtual ICollection<string> StringQuotes { get; } = [];
    protected virtual ICollection<char> EscapeChars { get; } = [];
    protected virtual IList<(string, char)> EscapeSequenceToStringValue { get; } = [];

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
    bool SkipComment() //returns true if skipped comment
    {
        bool Skipped = false;
        //skip eol comments
        foreach (var i in EOLCommentBegin)
        {
            if (StrEq(Current, Current + i.Length, i))
            {
                while (Current < input.Length && input[Current] != '\n')
                {
                    Current++;
                }
                Current++; //skip \n
                return true;
            }
        }
        //startendcomments
        for (int i = 0; i < StartEndCommentBegin.Count; i++)
        {
            if (StrEq(Current, Current + StartEndCommentBegin[i].Length, StartEndCommentBegin[i]))
            {
                Current += StartEndCommentBegin[i].Length;
                while (Current < input.Length && !StrEq(Current, Current + StartEndCommentEnd[i].Length, StartEndCommentEnd[i]))
                {
                    Current++;
                }
                Current += StartEndCommentEnd[i].Length; //skip end comment
                return true;
            }
        }
        return Skipped;
    }
    public IEnumerable<IToken> Scan()
    {
        while (Current < input.Length)
        {
            SkipWhiteSpace();
            Start = Current;
            if (SkipComment())
            {
                continue;
            }
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
        Start = Current;
        //Check if it's a number first
        if (IsNum(input[Current]))
        {
            bool SeenDot = false;
            while (Current < input.Length)
            {
                if (input[Current] == '.')
                {
                    if (SeenDot)
                    {
                        throw new Exception($"Number ${input[Start..(Current + 1)]} cannot have more than one dot at pos {Current}");
                    }
                    else
                    {
                        SeenDot = true;
                    }
                }
                else if (IsNum(input[Current]))
                {

                }
                else
                {
                    break;
                }
                Current++;
            }
            return IToken.NewToken(TokenType.Number, input[Start..Current], Current);
        }
        else if (IsValidFirstIdentChar(input[Current]))
        {
            while (Current < input.Length && (IsValidFirstIdentChar(input[Current]) || IsNum(input[Current])))
            {
                Current++;
            }
            return IToken.NewToken(TokenType.Identifier, input[Start..Current], Current);
        }
        else if (IsQuote(out var quoteLiteral))
        {
            Current += quoteLiteral.Length; //skip opening quote
            StringBuilder Literal = new();
            while (!StrEq(Current, Current + quoteLiteral.Length, quoteLiteral))
            {
                if (Current >= input.Length)
                {
                    throw new Exception($"Reached EOF whilst consuming string at {Current}");
                }
                if (EscapeChars.Contains(input[Current]))
                {
                    Literal.Append(ConsumeEscape());
                    continue;
                }
                Literal.Append(input[Current++]);
            }
            Current += quoteLiteral.Length; //skip closing quote
            return IToken.NewToken(TokenType.String, input[Start..Current], Current, Literal.ToString());
        }
        else
        {
            throw new Exception($"Unexpected character ${input[Current]} at pos {Current}");
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
    bool IsQuote(out string QuoteLiteral)
    {
        foreach (var i in StringQuotes)
        {
            if (StrEq(Current, Current + i.Length, i))
            {
                QuoteLiteral = i;
                return true;
            }
        }
        QuoteLiteral = "";
        return false;
    }
    char ConsumeEscape()
    {
        //first character is escape symbol
        Current++;
        foreach ((string s, char c) in EscapeSequenceToStringValue)
        {
            if (StrEq(Current, Current + s.Length, s))
            {
                Current += s.Length; //skip escape sequence
                return c;
            }
        }
        throw new Exception($"Invalid escape sequence at pos {Current}");
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