using System.Reflection;
using Common.Tokens;
using SmallLang.Parser.Lexing;

namespace SmallLangTest;

[TestFixture]
public class LexerTest
{
    private static Lexer GetLexer(string input)
    {
        return new Lexer(input);
    }

    public static IEnumerable<(string, TokenType)> GetAllValidValues()
    {
        return (IEnumerable<(string, TokenType)>)typeof(Lexer).GetProperty("MCTTM",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(GetLexer(""))!;
    }

    [TestCaseSource(nameof(GetAllValidValues))]
    public void Lex__Valid_Inputs__Returns_Correct_TokenType((string, TokenType) tuple)
    {
        var lexer = GetLexer(tuple.Item1);
        Assert.That(lexer.Scan().ToArray()[0].TT, Is.EqualTo(tuple.Item2));
    }

    [Test]
    [Combinatorial]
    public void Lex__Valid_Combinations__Returns_Correct_Token_Sequence(
        [ValueSource(nameof(GetAllValidValues))]
        (string, TokenType) t1,
        [ValueSource(nameof(GetAllValidValues))]
        (string, TokenType) t2
    )
    {
        var lexer = GetLexer($"{t1.Item1} {t2.Item1}");
        Assert.That(lexer.Scan().Select(t => t.TT).ToArray(), Is.EquivalentTo(new[] { t1.Item2, t2.Item2 }));
    }

    [Test]
    public void EOL_Comments_Skips_Properly()
    {
        var lexer = GetLexer("# + - This is a comment\n123");
        Assert.That(lexer.Scan().Select(t => t.TT).ToArray(), Is.EquivalentTo(new[] { TokenType.Number }));
    }

    [Test]
    public void Multiline_Comments_Skips_Properly()
    {
        var lexer = GetLexer("1 /* + - * / */ and");
        Assert.That(lexer.Scan().Select(t => t.TT).ToArray(),
            Is.EquivalentTo(new[] { TokenType.Number, TokenType.LogicalAnd }));
    }

    [Test]
    public void ChainedComments_Skips_Properly()
    {
        var lexer = GetLexer("1 /* + - * / */ /* ! */ #or\n and");
        Assert.That(lexer.Scan().Select(t => t.TT).ToArray(),
            Is.EquivalentTo(new[] { TokenType.Number, TokenType.LogicalAnd }));
    }

    [TestCase("_asdDE", TokenType.Identifier)]
    [TestCase("12345", TokenType.Number)]
    [TestCase("\"abc\"", TokenType.String)]
    [TestCase("\"\"", TokenType.String)]
    [TestCase("\"\\n\"", TokenType.String)]
    [TestCase("\"\n\"", TokenType.String)]
    [TestCase("'\n'", TokenType.String)]
    [TestCase("''", TokenType.String)]
    public void Special_Tokens_Lexed(string i, TokenType exp)
    {
        Assert.That(GetLexer(i).Scan().Select(t => t.TT).ToArray(), Is.EquivalentTo(new[] { exp }));
    }

    [Test]
    public void Lex__Escaped_String__Literal_Is_Replaced_Properly()
    {
        Assert.That(GetLexer("\"\\n\"").Scan().Select(t => t.Literal).ToArray(), Is.EquivalentTo(new[] { "\n" }));
    }
}