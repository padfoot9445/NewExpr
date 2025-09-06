using Common.AST;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLangTest.Generated;

namespace SmallLangTest;

[TestFixture]
[CancelAfter(5000)]
public class FlattenTests
{
    private static IEnumerable<string> GetTestCases()
    {
        foreach (var i in ExamplePrograms.AllPrograms) yield return i;
    }

    private static int CountAST(ISmallLangNode ast)
    {
        var returnValue = 0;
        foreach (var i in ((SmallLangNode)ast).ChildNodes)
        {
            returnValue++;
            returnValue += CountAST(i);
        }

        return returnValue;
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Flatten_Count__All_Programs__Matches_Manual_Count(string Program)
    {
        var ast = ParserTest.Parse(Program);
        Assert.That(ast.Flatten().Count(), Is.EqualTo(CountAST(ast) + 1));
    }

    private static IToken GetToken(TokenType TT)
    {
        return IToken.NewToken(TT, "", -1);
    }

    [Test]
    public void Flatten_Count__Custom_AST__Matches_Manual_Count()
    {
        var ast = new SectionNode(
            [
                new BinaryExpressionNode(
                    GetToken(TokenType.Addition), new PrimaryNode(GetToken(TokenType.Number)),
                    new PrimaryNode(GetToken(TokenType.String))
                ),
                new WhileNode(
                    new UnaryExpressionNode(GetToken(TokenType.LogicalNot),
                        new PrimaryNode(GetToken(TokenType.Number))), null, new SectionNode([]), null)
            ]
        );

        Assert.That(ast.Flatten().Count(), Is.EqualTo(8));
    }
}