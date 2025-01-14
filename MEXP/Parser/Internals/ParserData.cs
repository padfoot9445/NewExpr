using Common.Tokens;
using Common.AST;
using System.Linq.Expressions;
using Common.Logger;
using System.Diagnostics;
using MEXP.Parser.Internals;
namespace MEXP.Parser.Internals;
class ParserData
{
    public List<IToken> Input { get; set; } //assume it ends in EOF
    public int Current = 0;
    public ILogger Log { get; init; }
    public int Position { get => Current; }
    public readonly HashSet<TokenType> RecoveryTokens = [TokenType.Semicolon];
    public ITypeProvider TP { get; }
    public SafeParser SP { get; init; }
    public IToken? Advance()
    {
        Current++;
        return CurrentToken(-1);
    }
    public IToken? CurrentToken(int offset = 0, bool Inc = false)
    {
        if (Current + offset < Input.Count)
        {
            if (Inc)
            {
                return Input[Current++ + offset]; //generalization of Current++ not needed, I think
            }
            return Input[Current + offset];
        }
        return null;
    }
    public ParserData(IEnumerable<IToken> tokens, ILogger? logger = null, ITypeProvider? TP = null)
    {
        this.Input = tokens.ToList();
        if (Input.Count > 0 && this.Input[^1].TT != TokenType.EOF)
        {
            Input.Add(IToken.NewToken(TokenType.EOF, "EOF", -1));
        }
        Log = logger ?? new Logger();
        SP = new(Log);
        this.TP = TP ?? new TypeProvider();

    }
}