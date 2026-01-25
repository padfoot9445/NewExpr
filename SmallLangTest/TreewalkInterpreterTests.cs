using Common.Tokens;
using SmallLang.Drivers;
using SmallLang.IR.AST.Generated;
using SmallLang.TreeWalkInterpreter.RuntimeObjects;
using SmallLangTest.Generated;
using Interpreter = SmallLang.TreeWalkInterpreter.TreeWalkInterpreter;

namespace SmallLang.TreeWalkInterpreterTests;

[TestFixture]
public class TreeWalkInterpreterTests
{

  private Interpreter Interpreter { get; set; }

  [SetUp]
  public void Init()
  {

    Interpreter = new Interpreter();
  }

  [Test]
  public void VisitPrimary__Manually_Instantiated_Primary_Node__Modifies_State_Correctly()
  {
    IEnumerable<PrimaryNode> PrimaryNodeTestCases =
    [
      new PrimaryNode(IToken.NewToken(TokenType.Number,
          "1.234",
          -1)),
        new PrimaryNode(IToken.NewToken(TokenType.String,
          "\"abc\"",
          -1,
          "abc")),
        new PrimaryNode(IToken.NewToken(TokenType.TrueLiteral, "", -1)),
        new PrimaryNode(IToken.NewToken(TokenType.FalseLiteral, "", -1))
    ];

    foreach (var i in PrimaryNodeTestCases)
    {
      Interpreter.Visit(null, i);
    }

    object[] Expected = [false, true, "abc", 1.234];
    IRunTimeObject[] Results = Interpreter.State.Stack.ToArray();
    Assert.Multiple((() =>
    {
      Assert.That(Results[0].AsVal<bool>(), Is.EqualTo(false));
      Assert.That(Results[1].AsVal<bool>(), Is.EqualTo(true));
      Assert.That(Results[2].AsRef<string>(), Is.EqualTo("abc"));
      Assert.That(Results[3].AsVal<double>(), Is.EqualTo(1.234));
    }));
  }

  private static IEnumerable<string> GetTestCases()
  {
    return ExamplePrograms.AllPrograms;
  }

  [TestCaseSource(nameof(GetTestCases))]
  public void Interpret_All_Programs__Does_Not_Throw(string program)
  {
    var ast = new Parser.Parser(program).Parse<SectionNode>();
    Assert.That(() => Interpreter.Interpret(ast), Throws.Nothing);
  }

}