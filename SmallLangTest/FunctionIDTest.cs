using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, Timeout(5000)]
public class FunctionIDTests
{

    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AST = ParserTest.Parse(program);
            var AssignScopeVisitor = new AssignScopeVisitor();

            AssignScopeVisitor.BeginVisiting(AST);

            var TLTVisitor = new TypeLiteralTypeVisitor();

            TLTVisitor.BeginVisiting(AST);

            yield return (AST, program);
        }
    }

    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__FunctionID_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new FunctionIDVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__FunctionID_Visitor__BeginVisiting__No_VariableName_Is_Null((ISmallLangNode ast, string program) input)
    {
        new FunctionIDVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeFunctionID>().All(x => x.FunctionID is not null), Is.True, message: $"{input.program}\n\n{string.Join('\n', input.ast.Flatten().OfType<IHasAttributeFunctionID>().Where(x => x.FunctionID is null).Select(x => x.ToString()))}");

    }

}