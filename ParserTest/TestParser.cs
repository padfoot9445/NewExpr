using Common.AST;
using Common.Tokens;

namespace ParserTest;
[TestFixture]
public class TestParser
{
    static IEnumerable<IToken> Lex(string input)
    {
        var s = Common.Lexer.IScanner.NewScanner(input);
        return s.Scan();
    }
    #region string representation builders, unused
    static string Program(params string[] Children) => Build("Program", Children);
    static string Expression(params string[] Children) => Build("Expression", Children);
    static string Addition(params string[] Children) => Build("Addition", Children);
    static string AdditionPrime(params string[] Children) => Build("AdditionPrime", Children);
    static string Multiplication(params string[] Children) => Build("Multiplication", Children);
    static string MultiplicationPrime(params string[] Children) => Build("MultiplicationPrime", Children);
    static string Negation(params string[] Children) => Build("Negation", Children);
    static string Primary(params string[] Children) => Build("Primary", Children);
    static string PrimaryTerminal(params string[] Children) => Build("Primary-Terminal", Children);
    static string Build(string grammarname, string[] Children)
    {
        return $"{grammarname}: ({string.Join(", ", Children)})";
    }
    #endregion
    #region ParseValidCases
    [TestCase("5")]
    [TestCase("-10")]
    [TestCase("(3)")]
    [TestCase("(-42)")]
    [TestCase("5 + 3")]
    [TestCase("10 - 7")]
    [TestCase("(3 + 5) - 2")]
    [TestCase("1 + 2 - 3 + 4")]
    [TestCase("6 * 7")]
    [TestCase("8 / 4")]
    [TestCase("9 * (2 + 3)")]
    [TestCase("(6 / 3) * 4")]
    [TestCase("10 * 2 / 5 * 3")]
    [TestCase("1 + 2 * 3")]
    [TestCase("(1 + 2) * 3")]
    [TestCase("4 * (5 + 6 - 7)")]
    [TestCase("3 + 4 * 5 - 6 / 2")]
    [TestCase("10 - (3 * (2 + 1))")]
    [TestCase("-5")]
    [TestCase("-(3 + 2)")]
    [TestCase("-4 * 5")]
    [TestCase("6 + -(7 - 2)")]
    [TestCase("-(-3)")]
    [TestCase("0")]
    [TestCase("(-0)")]
    [TestCase("1 + (-2) * (3 - 4 / (5 + 6))")]
    [TestCase("((((1))))")]
    [TestCase("-(-(-1))")]
    [TestCase("5 + 3 10 - 7")]
    [TestCase("1 + 2 * 3 4 / 2")]
    [TestCase("10 - (3 * (2 + 1)) 7 * 8")]
    #endregion
    public void Parse_Valid_Strings__Returns_True_And_Node_Is_Type_AST(string input)
    {
        Assert.Multiple(() =>
        {
            Assert.That(Parser.Parser.Parse(Lex(input), out ASTNode? node), Is.True);

            Assert.That(node, Is.TypeOf<ASTNode>());
        });
    }
    #region Invalid Cases
    [TestCase("", 0)]
    [TestCase("5;", 1)]
    [TestCase("5 + 3; 10 - 7;", 1)]
    [TestCase("1 + 2 * 3; -4 / 2;", 1)]
    [TestCase("10 - (3 * (2 + 1)); 7 * 8;", 1)]
    #endregion
    public void Invalid_Cases__Returns_False_And_Prints_Error(string input, int MinimumErrorMessageCount) //do not assert for null because that is UB
    {
        var Logger = new MockLogger();
        Assert.Multiple(() =>
        {
            Assert.That(Parser.Parser.Parse(Lex(input), out ASTNode? node, Logger), Is.False);
            Assert.That(Logger.LogRecord, Has.Count.GreaterThanOrEqualTo(MinimumErrorMessageCount));
        });
    }

    [Test]
    public void tets()
    {
        Parser.Parser.Parse(Lex("5 + 3 10 - 7"), out ASTNode? node);
        Console.WriteLine(node!.Print());
    }
}
