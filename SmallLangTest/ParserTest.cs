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
    Node Parse(string input) => new Parser(input).Parse();
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
            var res = new Parser("int i = 0; Function(1, 2, 3); if(i == 2){return 1;} while(true){i =  i + 1;} return 2; break ident;").Parse();

            //var res = new Parser("1").Parse();
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
            Assert.That(res.Data!.TT, Is.EqualTo(OpTT));
            Assert.That(res.Data!.Lexeme, Is.EqualTo(OpLexeme));
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
                Assert.That(res.Children[0].Data!.TT, Is.EqualTo(TokenType.Number));
            });
            AssertOpExprPair(res.Children[1], "==", TokenType.EqualTo, NodeType.Identifier);
            AssertOpExprPair(res.Children[2], "!=", TokenType.NotEqualTo, NodeType.Declaration);
            AssertOpExprPair(res.Children[3], "<", TokenType.LessThan, NodeType.FunctionCall);
            AssertOpExprPair(res.Children[4], "<=", TokenType.LessThanOrEqualTo, NodeType.BinaryExpression);
            AssertOpExprPair(res.Children[5], ">=", TokenType.GreaterThanOrEqualTo, NodeType.Index);
            AssertOpExprPair(res.Children[6], ">", TokenType.GreaterThan, NodeType.Identifier);
        });
    }
    [Test]
    public void Parse__Return_Statement__Returns_Correct()
    {
        var res1 = Parse("return 1 + 1 - 2 * FunctionCall(1, 2, 3, 4, 5);");
        var res = res1.Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.Return));
        Assert.That(res.Children.Count, Is.EqualTo(1));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.BinaryExpression));
    }
    [TestCase("break", TokenType.Break)]
    [TestCase("continue", TokenType.Continue)]
    public void Parse__LCTRLStatements__Returns_Correct(string i, TokenType e)
    {
        var res = Parse($"{i};").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.LoopCTRL));
        Assert.That(res.Data!.TT, Is.EqualTo(e));
        var res2 = Parse($"{i} ident;").Children[0];
        Assert.That(res2.NodeType, Is.EqualTo(NodeType.LoopCTRL));
        Assert.That(res2.Data!.TT, Is.EqualTo(e));
        Assert.That(res2.Children[0].NodeType, Is.EqualTo(NodeType.ValInLCTRL));
    }
    [Test]
    public void Parse__For_Loop__Returns_Correct()
    {
        var res = Parse($"for (int i = 0; i < Func(); i = i + 1) as Label \n{{\t j = j + 1; \n\t DoSomething(i, j);\n}} else \n{{\n\tDoElse();}}").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.For));
        Assert.That(res.Children, Has.Count.EqualTo(6));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Declaration));
        Assert.That(res.Children[1].NodeType, Is.EqualTo(NodeType.ComparisionExpression));
        Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Children[3].NodeType, Is.EqualTo(NodeType.LoopLabel));
        Assert.That(res.Children[4].NodeType, Is.EqualTo(NodeType.Section));
        Assert.That(res.Children[4].Children[0].NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Children[4].Children[1].NodeType, Is.EqualTo(NodeType.FunctionCall));
        Assert.That(res.Children[5].NodeType, Is.EqualTo(NodeType.Section));
        Assert.That(res.Children[5].Children[0].NodeType, Is.EqualTo(NodeType.FunctionCall));
    }
    [Test]
    public void Parse__While_Loop__Returns_Correct()
    {
        var res = Parse($"while (Func()) {{\n\t j = j + 1;\n\t DoSomething(i, j);\n}} else {{}}").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.While));
        Assert.That(res.Children, Has.Count.EqualTo(3));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.FunctionCall));
        Assert.That(res.Children[1].NodeType, Is.EqualTo(NodeType.Section));
        Assert.That(res.Children[1].Children[0].NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Children[1].Children[1].NodeType, Is.EqualTo(NodeType.FunctionCall));
        Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.Section));
    }
    [TestCase("for (int i = 0; i < 1; i = i + 1){}", 4)]
    [TestCase("for (int i = 0; i < 1; i = i + 1) as Label{}", 5)]
    [TestCase("for (int i = 0; i < 1; i = i + 1){} else {}", 5)]
    [TestCase("for (int i = 0; i < 1; i = i + 1) as Label {} else {}", 6)]
    [TestCase("for (int i = 0; i < 1; i = i + 1){}", 4)]
    [TestCase("while (i < 1) as Label{}", 3)]
    [TestCase("while (i < 1){} else {}", 3)]
    [TestCase("while (i < 1) as Label {} else {}", 4)]
    [TestCase("while (i){}", 2)]
    public void Parse__Loop__Returns_Correct_Length_Children(string loop, int expected)
    {
        Assert.That(Parse(loop).Children[0].Children, Has.Count.EqualTo(expected));
    }
    [TestCase("+=", TokenType.Addition)]
    [TestCase("-=", TokenType.Subtraction)]
    [TestCase("*=", TokenType.Multiplication)]
    [TestCase("/=", TokenType.Division)]
    [TestCase("**=", TokenType.Exponentiation)]
    [TestCase("&=", TokenType.BitwiseAnd)]
    [TestCase("|=", TokenType.BitwiseOr)]
    [TestCase("^=", TokenType.BitwiseXor)]
    [TestCase("<<=", TokenType.BitwiseLeftShift)]
    [TestCase(">>=", TokenType.BitwiseRightShift)]
    [TestCase("~=", TokenType.BitwiseNegation)]
    public void Parse__Immediate_Assignment_Operators__Returns_Correct(string i, TokenType expected)
    {
        var res = Parse($"x {i} 1;").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Data!.TT, Is.EqualTo(TokenType.Equals));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Identifier));
        Assert.That(res.Children[0].Data!.Lexeme, Is.EqualTo("x"));
        var add = res.Children[1];
        Assert.That(add.NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(add.Children[0].Data!.Lexeme, Is.EqualTo("x"));
        Assert.That(add.Children[1].Data!.TT, Is.EqualTo(TokenType.Number));
        Assert.That(add.Data!.TT, Is.EqualTo(expected));
    }
    [TestCase("++", TokenType.Addition)]
    [TestCase("--", TokenType.Subtraction)]
    public void Parse__Precrement_Assignment_Operators__Returns_Correct(string i, TokenType expected)
    {
        var res = Parse($"{i}x;").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Data!.TT, Is.EqualTo(TokenType.Equals));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Identifier));
        Assert.That(res.Children[0].Data!.Lexeme, Is.EqualTo("x"));
        var add = res.Children[1];
        Assert.That(add.NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(add.Children[0].Data!.Lexeme, Is.EqualTo("x"));
        Assert.That(add.Children[1].Data!.TT, Is.EqualTo(TokenType.Number));
        Assert.That(add.Children[1].Data!.Lexeme, Is.EqualTo("1"));
        Assert.That(add.Data!.TT, Is.EqualTo(expected));
    }
    [Test]
    public void Parse__If__Returns_Correct()
    {
        var res = Parse("if(i){} else if (i < 2)i + 1; else {}").Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.If));
        Assert.That(res.Children, Has.Count.EqualTo(3));
        Assert.That(res.Children[1].NodeType == res.Children[0].NodeType);
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.ExprStatementCombined));
        Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.Section));
        Assert.That(res.Children[1].Children[1].NodeType, Is.EqualTo(NodeType.BinaryExpression));
        Assert.That(res.Children[0].Children[0].NodeType, Is.EqualTo(NodeType.Identifier));
    }
    [Test]
    public void Parse__Switch__Returns_Correct()
    {
        string i = @"
        switch (FunctionCall())
        {
            1: DoOne();
            2: {
            ++j;}
            hree(): D1();
            our(): {D2();}
        }";
        var res = Parse(i).Children[0];
        Assert.That(res.NodeType, Is.EqualTo(NodeType.Switch));
        Assert.That(res.Children, Has.Count.EqualTo(5));
        Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.FunctionCall));
        Assert.That(res.Children[1].Children[0].NodeType, Is.EqualTo(NodeType.Primary));
        Assert.That(res.Children[1].Children[1].NodeType, Is.EqualTo(NodeType.FunctionCall));
        Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.ExprStatementCombined));
        //if does not throw, good enough tbh
    }
    [Test]
    public void Test__Function_Definition__Does_Not_Throw()
    {
        string i = @"
        collection<[list<[int]>, number]> Transform(copy ref immut readonly frozen number x, 
        int y, frozen readonly 
        immut int z)
        {
            return new collection<[list<[int]>, number]>(x, y: y, z);
            return new int(1);
        }";
        Parse(i);
        Assert.DoesNotThrow(() => Parse(i));
    }
}