using System.Collections.ObjectModel;
using System.Linq;
using Common.AST;
using Common.Tokens;
namespace CommonTests.ASTTest;
[TestFixture]
public class AstNodeTest
{

    public static IToken Node6 { get; } = IToken.NewToken(TokenType.Identifier, "x", -1);
    public static ASTNode Node3 { get; } = ASTNode.Terminal(Node6, "xAST");
    public static IToken Node4 { get; } = IToken.NewToken(TokenType.Addition, "+", -1);
    public static IToken Node5 { get; } = IToken.NewToken(TokenType.Number, "12", -1);
    public static ASTNode Node2 { get; } = ASTNode.Terminal(Node5, "12AST");
    public static ASTNode Node1 { get; } = ASTNode.Binary(Node2, Node4, Node3, "12 + x");
#pragma warning disable NUnit2009
#pragma warning disable NUnit2010
    [Test]
    public void TestREFEQ()
    {
        Assert.That(ReferenceEquals(Node1, Node1));
    }
    [Test]
    public void TestContains()
    {
        var c = new Collection<IValidASTLeaf>([Node1]);
        var k = c.Select(x => x).Where(x => ReferenceEquals(x, Node1));
        Assert.That(c.Contains(Node1));
    }
    [Test]
    public void TestContainsNUnit()
    {
        Assert.That(new Collection<IValidASTLeaf>([Node1]), Contains.Item(Node1));
    }
#pragma warning restore NUnit2009
#pragma warning restore NUnit2010

    [Test]
    public void TestFlatten()
    {
        ICollection<IValidASTLeaf> flat = Node1.Flatten();
        Assert.Multiple(() =>
        {
            Assert.That(flat, Has.Count.EqualTo(6));
            Assert.That(flat, Contains.Item(Node1));
            Assert.That(flat, Contains.Item(Node2));
            Assert.That(flat, Contains.Item(Node3));
            Assert.That(flat, Contains.Item(Node4));
            Assert.That(flat, Contains.Item(Node5));
            Assert.That(flat, Contains.Item(Node6));
        });
    }
    [Test]
    public void TestIsEquivalentTo__Same__ReturnsTrue()
    {
        Assert.That(Node1.IsEquivalentTo(Node1));
    }
    [Test]
    public void TestIsEquivalentTo__Annotated__ReturnsFalse()
    {
        Assert.That(Node1.IsEquivalentTo(AnnotatedNode<AnnotationContainerMock>.FromNodeRecursive(new(), Node1)), Is.False);
    }
    [Test]
    public void TestIsEquivalentTo__Different__ReturnsFalse()
    {
        Assert.That(Node1.IsEquivalentTo(Node2), Is.False);
    }
}