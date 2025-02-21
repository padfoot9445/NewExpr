using System.Diagnostics;
using Common.Tokens;
using SmallLang;
using SmallLang.Parser;

namespace SmallLangTest;
using Node = Common.AST.DynamicASTNode<SmallLang.ASTNodeType, SmallLang.Attributes>;
using NodeType = SmallLang.ASTNodeType;
[TestFixture]
public class ParserTest
{
    [Test]
    public void Ctor__Any_Input__Does_Not_Throw()
    {
        Assert.That(() => new Parser(""), Throws.Nothing);
    }
    [Test]
    public void Parse__Section__Returns_Correct()
    {
        Assert.Multiple(() =>
        {
            //var res = new Parser("int i = 0; Function(1, 2, 3); if(i == 2){return 1;} while(true){i += 1;} return 2; break ident;").Parse();
            new Parser("1;").Parse();
            return;

            var res = new Parser("1").Parse();
            Assert.That(res.NodeType, Is.EqualTo(NodeType.Section));
            Assert.That(res.Children, Has.Count.EqualTo(6));
            Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Declaration));
            Assert.That(res.Children[1].NodeType, Is.EqualTo(NodeType.FunctionCall));
            Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.If));
            Assert.That(res.Children[3].NodeType, Is.EqualTo(NodeType.While));
            Assert.That(res.Children[4].NodeType, Is.EqualTo(NodeType.Return));
            Assert.That(res.Children[5].NodeType, Is.EqualTo(NodeType.LoopCTRL));
        });
    }
    public static IEnumerable<TestCaseData> GetInfixOperators()
    {
        return new string[] { "+", "-", "*", "/", "**", "or", "xor", "and", "&", "|", "^", "<<", ">>" }.Select(x => new TestCaseData(x));
    }
    [TestCaseSource(nameof(GetInfixOperators))]
    public void Parse__Expressions__Returns_Correct(string Operator)
    {
        Assert.Multiple(() =>
        {
            var res1 = new Parser($"x {Operator} 1.2;").Parse();
            Assert.That(res1.NodeType, Is.EqualTo(NodeType.Section));
            Assert.That(res1.Children, Has.Count.EqualTo(1));
            var res = res1.Children[0];
            Assert.That(res.NodeType, Is.EqualTo(NodeType.BinaryExpression));
            Assert.That(res.Data, Is.Not.Null);
            Assert.That(res.Data!.Lexeme, Is.EqualTo(Operator));
            Assert.That(res.Children, Has.Count.EqualTo(2));
            Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Identifier));
            Assert.That(res.Children[1].NodeType, Is.EqualTo(NodeType.Primary));
        });
    }
    public static IEnumerable<TestCaseData> GetComparisonOperator()
    {
        return new string[] { "==", "!=", "<", "<=", ">=", ">" }.Select(x => new TestCaseData(x));
    }
    void AssertOpExprPair(Node res, string OpLexeme, TokenType OpTT, NodeType ExprType)
    {
        Assert.Multiple(() =>
        {
            Assert.That(res.NodeType, Is.EqualTo(NodeType.OperatorExpressionPair));
            Assert.That(res.Children[0].NodeType, Is.EqualTo(ExprType));
            Assert.That(res.Children[0].Data!.TT, Is.EqualTo(OpTT));
            Assert.That(res.Children[0].Data!.Lexeme, Is.EqualTo(OpLexeme));
        });
    }
    [Test]
    public void Parse__Comparisons__Returns_Correct()
    {
        var res1 = new Parser("1 == x != (int i = 0) < Func(1,2, 3) <= 1 + 2 ** 3 >= Array[8 + 7] > Final;").Parse();
        var res = res1.Children[0];
        Assert.Multiple(() =>
        {
            Assert.Multiple(() =>
        {
            Assert.That(res.NodeType, Is.EqualTo(NodeType.ComparisionExpression));
            Assert.That(res.Children, Has.Count.EqualTo(7));
        });
            Assert.Multiple(() =>
            {
                Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Primary));
                Assert.That(res.Children[0].Data!.Lexeme, Is.EqualTo("1"));
                Assert.That(res.Children[1].Data!.TT, Is.EqualTo(TokenType.Number));
            });
            AssertOpExprPair(res.Children[1], "==", TokenType.EqualTo, NodeType.Identifier);
            AssertOpExprPair(res.Children[2], "!=", TokenType.NotEqualTo, NodeType.Declaration);
            AssertOpExprPair(res.Children[3], "<", TokenType.LessThan, NodeType.FunctionCall);
            AssertOpExprPair(res.Children[4], "<=", TokenType.LessThanOrEqualTo, NodeType.BinaryExpression);
            AssertOpExprPair(res.Children[5], ">=", TokenType.GreaterThanOrEqualTo, NodeType.Index);
            AssertOpExprPair(res.Children[6], ">", TokenType.GreaterThan, NodeType.Identifier);
        });
    }
}