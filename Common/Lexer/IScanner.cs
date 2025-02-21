namespace Common.Lexer;
using Common.Tokens;
public interface IScanner
{
    public IEnumerable<IToken> Scan();
}