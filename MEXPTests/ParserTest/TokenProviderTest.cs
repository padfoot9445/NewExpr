using Common.Tokens;
using Parser;

namespace MEXPTests.ParserTest;
[TestFixture]
public class TokenProviderTest
{
    static IEnumerable<TestCaseData> TCMP_SingleArg_Cases()
    {
        yield return new TestCaseData(IToken.NewToken(TokenType.Subtraction, "-1", -1), TokenType.Subtraction, true);
        yield return new TestCaseData(IToken.NewToken(TokenType.NotEqualTo, "-1", -1), TokenType.Exponentiation, false);
        yield return new TestCaseData(IToken.NewToken(TokenType.NotEqualTo, "-1", -1), TokenType.GreaterThan, false);
        yield return new TestCaseData(IToken.NewToken(TokenType.Semicolon, "1", -1), TokenType.Semicolon, true);
        yield return new TestCaseData(IToken.NewToken(TokenType.NotEqualTo, "1", -1), TokenType.Addition, false);
        yield return new TestCaseData(IToken.NewToken(TokenType.NotEqualTo, "1", -1), TokenType.NotEqualTo, true);
    }
    [TestCaseSource(nameof(TCMP_SingleArg_Cases))]
    public void Test__TCmp__SingleArg__ReturnsCorrect(IToken token, TokenType type, bool expected)

    {
        Assert.That(token.TCmp(type), Is.EqualTo(expected));
    }
    static IEnumerable<TestCaseData> TCMP_MultiArg_Cases()
    {
        yield return new TestCaseData(IToken.NewToken(TokenType.Addition, "-1", 1), new TokenType[3] { TokenType.Addition, TokenType.Semicolon, TokenType.NotEqualTo }, true);
        yield return new TestCaseData(IToken.NewToken(TokenType.Addition, "-1", 1), new TokenType[3] { TokenType.Number, TokenType.Semicolon, TokenType.NotEqualTo }, false);
    }
    [TestCaseSource(nameof(TCMP_MultiArg_Cases))]
    public void Test__TCmp__MultiArg__ReturnsCorrect(IToken token1, IEnumerable<TokenType> types, bool expected)
    {
        Assert.That(token1.TCmp(types), Is.EqualTo(expected));
    }
}