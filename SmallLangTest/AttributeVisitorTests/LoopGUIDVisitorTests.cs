using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, Timeout(5000)]
public class LoopGUIDVisitorTest
{

    [Test, Timeout(5000)]
    public void All_Programs__LoopGUID_BeginVisiting__Does_Not_Throw()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var LoopGUIDVisitor = new LoopGUIDVisitor();
            Assert.That(() => LoopGUIDVisitor.BeginVisiting(ParserTest.Parse(program)), Throws.Nothing, message: program);
        }
    }
    [Test]
    public void All_Programs__LoopGUID_BeginVisiting__No_Scope_Is_Null()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var LoopGUIDVisitor = new LoopGUIDVisitor();
            var ast = ParserTest.Parse(program);
            LoopGUIDVisitor.BeginVisiting(ast);

            Assert.That(ast.Flatten().OfType<IHasAttributeLoopGUID>().Select(x => x.LoopGUID is null), Does.Not.Contain(true));
        }
    }

}