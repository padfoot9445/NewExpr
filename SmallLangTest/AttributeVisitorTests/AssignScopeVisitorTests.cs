using System.Collections.Immutable;
using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.Metadata;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture]
public class AssignScopeVisitorTest
{

    [Test, CancelAfter(5000)]
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
    [Test]
    public void All_Programs__AssignScope_BeginVisiting__Scopes_Are_Unique_By_Name()
    {
        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AssignScopeVisitor = new AssignScopeVisitor();
            var ast = ParserTest.Parse(program);
            AssignScopeVisitor.BeginVisiting(ast);
            var result = ast.Flatten()
                .Select(x => x.Scope)
                .GroupBy(x => x!.FullScopeName)
                .Select(x => (
                    x.Key,
                    x.Select(y => ReferenceEquals(x.First(), y)).All(y => y),
                    x.Select(y => x.First() == y).All(y => y)
                )).ToImmutableArray();

            var result2 = result.Select(x => (x.Item2, x.Item3))
                .Aggregate((x, y) => (x.Item1 && y.Item1, x.Item2 && y.Item2));

            var message = string.Join('\n', result.Select(x => $"{x.Key}, {x.Item2}, {x.Item3}"));
            Assert.Multiple((() =>
            {
                Assert.That(result2.Item1, Is.True, message: message);
                Assert.That(result2.Item2, Is.True, message: message);
            }));
        }
    }
}