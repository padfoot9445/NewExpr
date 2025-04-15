using SmallLang.Evaluator;
using Node = Common.AST.DynamicASTNode<SmallLang.ImportantASTNodeType, SmallLangTest.EvaluatorTests.Attributes>;
namespace SmallLangTest.EvaluatorTests;
[TestFixture]
public class EvaluatorTests
{
    Visitor V => new();
    void AssertNode(Node node, double RootVal, double Value, bool IsLiteral)
    {
        var att = node.Attributes;
        Assert.Multiple(() =>
        {
            Assert.That(att.RootValue, Is.EqualTo(RootVal));
            Assert.That(att.Value, Is.EqualTo(Value));
            Assert.That(att.IsLiteral, Is.EqualTo(IsLiteral));
        });
    }

    [Test]
    public void TestEvaluateAdditionReturnsCorrect()
    {
        var node = ASTSrc.Addition<Attributes>();
        new DynamicASTEvaluator().Evaluate(node, V);
        Assert.Multiple(() =>
        {
            AssertNode(node, 1.1, 1.1, false);
            AssertNode(node.Children[0], 1.1, 1, true);
            AssertNode(node.Children[1], 1.1, 0.1, true);
        });
    }

    [Test]
    public void TestNumberOneCorrect()
    {
        var node = ASTSrc.Number<Attributes>();
        new DynamicASTEvaluator().Evaluate(node, V);
        AssertNode(node, 1, 1, true);
    }
    [Test]
    public void TestNumberTwoCorrect()
    {
        var node = ASTSrc.NumberTwo<Attributes>();
        new DynamicASTEvaluator().Evaluate(node, V);
        AssertNode(node, 0.1, 0.1, true);
    }
    [Test]
    public void TestMultiplicationCorrect()
    {
        var node = ASTSrc.Multiplication<Attributes>();
        new DynamicASTEvaluator().Evaluate(node, V);
        Assert.Multiple(() =>
        {
            AssertNode(node, 0.1, 0.1, false);
            AssertNode(node.Children[1], 0.1, 1, true);
            AssertNode(node.Children[0], 0.1, 0.1, true);
        });
    }

    [Test]
    public void TestAddMulCorrect()
    {
        var node = ASTSrc.AddMul<Attributes>();
        new DynamicASTEvaluator().Evaluate(node, V);
        Assert.Multiple(() =>
        {
            AssertNode(node, 0.2, 0.2, false);
            AssertNode(node.Children[0], 0.2, 0.1, false);
            AssertNode(node.Children[0].Children[0], 0.2, 0.1, true);
            AssertNode(node.Children[0].Children[1], 0.2, 1, true);
            AssertNode(node.Children[1], 0.2, 0.1, true);
        });
    }
}