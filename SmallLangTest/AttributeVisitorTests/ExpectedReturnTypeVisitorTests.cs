using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, Timeout(5000)]
public class ExpectedReturnTypeVisitorTests
{

    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases()
    => TypeLiteralTypeTests.GetNewTestCases(new TypeLiteralTypeVisitor(), TypeLiteralTypeTests.GetTestCases());

    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__Expected_Return_Type_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new ExpectedReturnTypeVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases)), Timeout(5000)]
    public void All_Programs__Expected_Return_Type_Visitor__BeginVisiting__No_Target_Loop_GUID_Is_Null((ISmallLangNode ast, string program) input)
    {
        new ExpectedReturnTypeVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeExpectedReturnType>().All(x => x.ExpectedReturnType is not null), Is.True, message: input.program);

    }

}