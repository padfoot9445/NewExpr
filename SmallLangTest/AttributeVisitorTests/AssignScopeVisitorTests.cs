using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture]
public class AssignScopeVisitorTest
{

    [Test, Timeout(5000)]
    public void All_Programs__AssignScope_BeginVisiting__Does_Not_Throw()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AssignScopeVisitor = new AssignScopeVisitor();
            Assert.That(() => AssignScopeVisitor.BeginVisiting(ParserTest.Parse(program)), Throws.Nothing, message: program);
        }
    }
    [Test]
    public void All_Programs__AssignScope_BeginVisiting__No_Scope_Is_Null()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AssignScopeVisitor = new AssignScopeVisitor();
            var ast = ParserTest.Parse(program);
            AssignScopeVisitor.BeginVisiting(ast);

            Assert.That(ast.Flatten().Select(x => x.Scope is null), Does.Not.Contain(true));
        }
    }

}