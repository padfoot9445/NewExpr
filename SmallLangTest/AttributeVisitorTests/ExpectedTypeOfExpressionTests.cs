using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;

namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, CancelAfter(5000)]
public class ExpectedTypeOfExpressionTests
{
    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases() =>
        TypeLiteralTypeTests.GetNewTestCases(new GenericSLTypeVisitor(),
            GenericSmallLangTypeExpressionTests.GetTestCases());

    [TestCaseSource(nameof(GetTestCases)), CancelAfter(5000)]
    public void All_Programs__ExpectedTypeOfExpression_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new ExpectedTypeOfExpressionVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases)), CancelAfter(5000)]
    public void All_Programs__ExpectedTypeOfExpression_Visitor__BeginVisiting__No_TypeOfExpression_Is_Null((ISmallLangNode ast, string program) input)
    {
        new ExpectedTypeOfExpressionVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeExpectedTypeOfExpression>().All(x => x.ExpectedTypeOfExpression is not null), Is.True, message: $"{input.program}\n\n{string.Join('\n', input.ast.Flatten().OfType<IHasAttributeExpectedTypeOfExpression>().Where(x => x.ExpectedTypeOfExpression is null).Select(x => x.ToString() + $"\n\t{((SmallLangNode)x).GetParent(input.ast)?.ToString()}"))}");

    }

}