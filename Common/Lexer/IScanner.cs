using Common.Tokens;

namespace Common.Lexer;

public interface IScanner
{
    public IEnumerable<IToken> Scan();
}