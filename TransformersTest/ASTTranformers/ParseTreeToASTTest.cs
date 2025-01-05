using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ASTTest;
using Common.AST;
using Common.Tokens;
using NUnit.Framework.Interfaces;
using Transformers.ASTTransformers;

namespace TransformersTest.ASTTransformers;
[TestFixture]
public class ParseTreeToASTTest
{
    static IToken Token1 { get; } = IToken.NewToken(TokenType.NotEqualTo, "-1", -1);
    static ASTNode Terminal1 { get; } = ASTNode.Terminal(Token1, "term");
    static ASTNode TwoTerminal { get; } = new ASTNode([ASTLeafType.Terminal, ASTLeafType.Terminal], [Token1, Token1], "twoterm");
    static ASTNode NT1 { get; } = ASTNode.NonTerminal(Terminal1, "nt");
    static ASTNode TWONT { get; } = new ASTNode([ASTLeafType.NonTerminal, ASTLeafType.NonTerminal], [NT1, NT1], "twont");
    static ASTNode Binary { get; } = ASTNode.Binary(Terminal1, Token1, NT1, "bin");
    static ASTNode BINP { get; } = ASTNode.BinaryPrime(Token1, Terminal1, NT1, "BP");
    static ASTNode EmptyDescend { get; } = new ASTNode([], [], "e");
    static ASTNode StackedEmpty { get; } = ASTNode.NonTerminal(ASTNode.NonTerminal(EmptyDescend, "se"), "");
    static ASTNode StackedNonEmpty { get; } = ASTNode.NonTerminal(ASTNode.NonTerminal(Terminal1, "sne"), "");
    Func<IValidASTLeaf, bool> TokenOrNonRedundant = x => (x is IToken || (x is ASTNode node && node.Children.Length > 1));
    [Test]
    public void MinimizeTreeTest__Does_Not_Throw()
    {
        Assert.That(() => { AstNodeTest.Node1.MinimizeRemoveUnnecessaryNodes(); }, Throws.Nothing);
    }
    [Test]
    public void MinimizeTreeTest__CorrectlyMinimizes__OnlyRedundantRemoved_And_AllRedundantRemoved()
    {
        Collection<IValidASTLeaf> Flat1 = AstNodeTest.Node1.Flatten();
        Assert.Multiple(() =>
        {
            ASTNode SimpleNode = AstNodeTest.Node1.MinimizeRemoveUnnecessaryNodes();
            Collection<IValidASTLeaf> Flat2 = SimpleNode.Flatten();
            Assert.That(Flat2, Does.Not.Matches(new Predicate<IValidASTLeaf>(x => !TokenOrNonRedundant(x)))); //assert that all redundant nodes have been removed
            Assert.That(Flat1.Select(x => x).Where(TokenOrNonRedundant).Count(), Is.EqualTo(Flat2.Count)); //assert that no extra has been removed
        });
    }
    [TestCaseSource(nameof(RedundantTests))]
    public void RedundantTest__Correctly_Identifies_Redundancies(IValidASTLeaf leaf, bool IsRedundant)
    {
        Assert.That(leaf.IsRedundant(), Is.EqualTo(IsRedundant));
    }
    static IEnumerable<TestCaseData> RedundantTests()
    {
        yield return new TestCaseData(Token1, false);
        yield return new TestCaseData(Terminal1, true);
        yield return new TestCaseData(TwoTerminal, false);
        yield return new TestCaseData(NT1, true);
        yield return new TestCaseData(TWONT, false);
        yield return new TestCaseData(BINP, false);
        yield return new TestCaseData(Binary, false);
        yield return new TestCaseData(StackedEmpty, true);
        yield return new TestCaseData(StackedNonEmpty, true);
        yield return new TestCaseData(EmptyDescend, true);
    }
    [TestCaseSource(nameof(DescendCases))]
    public void DescendTest__CorrectlyDescends(IValidASTLeaf leaf, IValidASTLeaf? ExpectedDescendant)
    {
        IValidASTLeaf? x = leaf.Descend();
        Assert.That((x is IValidASTLeaf node && ExpectedDescendant is IValidASTLeaf node2) ? (node.IsEquivalentTo(node2)) : (x is null && ExpectedDescendant is null), Is.True);
    }
    static IEnumerable<TestCaseData> DescendCases()
    {
        yield return new TestCaseData(Token1, Token1);
        yield return new TestCaseData(Terminal1, Token1);
        yield return new TestCaseData(TwoTerminal, TwoTerminal);
        yield return new TestCaseData(NT1, Token1);
        yield return new TestCaseData(TWONT, TWONT);
        yield return new TestCaseData(BINP, BINP);
        yield return new TestCaseData(Binary, Binary);
        yield return new(StackedEmpty, null);
        yield return new(StackedNonEmpty, Token1);
        yield return new(EmptyDescend, null);
    }
}