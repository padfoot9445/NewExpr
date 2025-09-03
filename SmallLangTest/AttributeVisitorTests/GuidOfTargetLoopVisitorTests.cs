using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, Timeout(5000)]
public class GUIDOfTargetLoopVisitorTests
{

    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases()
    => TypeLiteralTypeTests.GetNewTestCases(new TypeLiteralTypeVisitor(), TypeLiteralTypeTests.GetTestCases());

    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__GUID_Of_Target_Loop_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new GUIDOfTargetLoopVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__GUID_Of_Target_Loop_Visitor__BeginVisiting__No_Target_Loop_GUID_Is_Null((ISmallLangNode ast, string program) input)
    {
        new GUIDOfTargetLoopVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeGUIDOfTargetLoop>().All(x => x.GUIDOfTargetLoop is not null), Is.True, message: input.program);

    }

}