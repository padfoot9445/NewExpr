using Common.AST;
using sly.parser;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;
using SmallLangTest;
using SmallLangTest.Generated;
namespace SmallLangTest.AttributeVisitorTests;

[TestFixture, CancelAfter(5000)]
public class GenericSmallLangTypeExpressionTests
{

    internal static IEnumerable<(ISmallLangNode, string)> GetTestCases()
    {
        List<BaseASTVisitor> Evaluators = [new AssignScopeVisitor(), new VariableNameVisitor(), new TypeLiteralTypeVisitor(), new FunctionIDVisitor()];

        foreach (var program in ExamplePrograms.AllPrograms)
        {
            var AST = ParserTest.Parse(program);
            foreach (var Evaluator in Evaluators)
            {
                Evaluator.BeginVisiting(AST);
            }
            yield return (AST, program);
        }
    }

    [TestCaseSource(nameof(GetTestCases)), CancelAfter(5000)]
    public void All_Programs__Generic_SLT_Visitor__BeginVisiting__Does_Not_Throw((ISmallLangNode ast, string program) input)
    {

        Assert.That(() => new GenericSLTypeVisitor().BeginVisiting(input.ast), Throws.Nothing, message: input.program);

    }
    [TestCaseSource(nameof(GetTestCases)), CancelAfter(5000)]
    public void All_Programs__Generic_SLT_Visitor__BeginVisiting__No_GenericSLT_Is_Null((ISmallLangNode ast, string program) input)
    {
        new GenericSLTypeVisitor().BeginVisiting(input.ast);

        Assert.That(input.ast.Flatten().OfType<IHasAttributeGenericSLType>().All(x => x.GenericSLType is not null), Is.True, message: $"{input.program}\n\n{string.Join('\n', input.ast.Flatten().OfType<IHasAttributeGenericSLType>().Where(x => x.GenericSLType is null).Select(x => x.ToString() + $"\n\t{((SmallLangNode)x).GetParent(input.ast)?.ToString()}"))}");

    }

}