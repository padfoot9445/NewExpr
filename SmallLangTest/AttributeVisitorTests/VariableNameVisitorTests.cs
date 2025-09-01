using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture]
public class VariableNameVisitorTests
{

    private static IEnumerable<ISmallLangNode> GetTestCases()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AST = ParserTest.Parse(program);
            var AssignScopeVisitor = new AssignScopeVisitor();

            AssignScopeVisitor.BeginVisiting(AST);

            yield return AST;
        }
    }

    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__Variable_Name_Visitor__BeginVisiting__Does_Not_Throw(ISmallLangNode ast)
    {

        Assert.That(() => new VariableNameVisitor().BeginVisiting(ast), Throws.Nothing);

    }
    [TestCaseSource(nameof(GetTestCases))]
    public void All_Programs__Variable_Name_Visitor__BeginVisiting__No_VariableName_Is_Null(ISmallLangNode ast)
    {
        new VariableNameVisitor().BeginVisiting(ast);

        Assert.That(ast.Flatten().OfType<IHasAttributeVariableName>().All(x => x.VariableName is not null), Is.True);

    }

}