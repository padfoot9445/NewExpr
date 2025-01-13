namespace Common.Lexer;
using Common.Tokens;
public interface IScanner
{
    public IEnumerable<IToken> Scan();
    public static IScanner NewScanner(string input) => new NumberAndBooleanOnlyScanner(input);
}