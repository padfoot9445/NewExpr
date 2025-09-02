using Common.AST;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest.Generated;

namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, Timeout(5000)]
public class TypeLiteralTypeTests
{
    internal static IEnumerable<(ISmallLangNode, string)> GetNewTestCases(BaseASTVisitor visitor, IEnumerable<(ISmallLangNode, string)> Src)
    {
        foreach (var (Node, Program) in Src)
        {
            visitor.BeginVisiting(Node);

            yield return (Node, Program);
        }
    }

    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases() => ExamplePrograms.AllPrograms.Select(x => ((ISmallLangNode)ParserTest.Parse(x), x));
    [TestCaseSource(nameof(GetTestCases))]

    public void All_Programs__Type_Literal_Type_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new TypeLiteralTypeVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases))]
    public void All_Programs__Type_Literal_Type_Visitor__BeginVisiting__No_VariableName_Is_Null((ISmallLangNode ast, string program) input)
    {
        new TypeLiteralTypeVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeTypeLiteralType>().All(x => x.TypeLiteralType is not null), Is.True, message: input.program);

    }
}