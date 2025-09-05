using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLangTest.Generated;

namespace SmallLangTest.AttributeVisitorTests;

[TestFixture]
[CancelAfter(5000)]
public class AttributeEvaluatorTests
{
    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases()
    {
        return ExamplePrograms.AllPrograms.Select(program => (ParserTest.Parse(program) as ISmallLangNode, program));
    }

    [TestCaseSource(nameof(GetTestCases))]
    [CancelAfter(5000)]
    public void All_Programs__AttributeVisitor__BeginVisiting__Does_Not_Throw(
        (ISmallLangNode ast, string program) input)
    {
        Assert.That(() => new AttributeEvaluator().BeginVisiting(input.ast), Throws.Nothing, input.program);
    }
}