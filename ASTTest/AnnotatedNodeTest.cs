using Common.AST;
using Common.Tokens;

namespace ASTTest;
[TestFixture]
public class AnnotatedNodeTest
{
    static Predicate<IValidASTLeaf> IsAnnotatedOrToken = x => x is AnnotatedNode<AnnotationContainerMock> || x is IToken;
    [Test]
    public void Test__FromNodeRecursive__NoNodesAreLost() //some may be gained if the tree has terminals as those will be boxed
    {
        Assert.That(AnnotatedNode<AnnotationContainerMock>.FromNodeRecursive(new(), AstNodeTest.Node1).Flatten(), Has.Count.GreaterThanOrEqualTo(AstNodeTest.Node1.Flatten().Count));
    }
    [Test]
    public void Test__FromNodeRecursive__FlattenedHasOnlyTokensAndAnnotatedNodes()
    {
        Assert.That(AnnotatedNode<AnnotationContainerMock>.FromNodeRecursive(new(), AstNodeTest.Node1).Flatten(), Has.Some.Matches(IsAnnotatedOrToken));
    }
    [Test]
    public void Test__FromNodeRecursive__AttributeIsParameter()
    {
        AnnotationContainerMock anno = new();
        Assert.That(AnnotatedNode<AnnotationContainerMock>.FromNodeRecursive(anno, AstNodeTest.Node1).Attributes, Is.EqualTo(anno));
    }
}