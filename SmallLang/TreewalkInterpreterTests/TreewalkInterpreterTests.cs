using Common.Tokens;
using SmallLang.IR.AST.Generated;
using SmallLang.TreeWalkInterpreter.RuntimeObjects;

namespace SmallLang.TreeWalkInterpreterTests;

[TestFixture]
public class TreeWalkInterpreterTests
{
  [Test]
  public void VisitPrimary__Manually_Instantiated_Primary_Node__Modifies_State_Correctly()
  {
    Assert.Multiple((() =>
    {
      var visitor = new TreeWalkInterpreter.TreeWalkInterpreter();
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
        visitor.Visit(null, i);
      }

      object[] Expected = [false, true, "abc", 1.234];
      Assert.That(visitor.State.Stack.Select(x => x.As<object>()), Is.EquivalentTo(Expected));
    }));
  }
}