using System.Diagnostics;
using Common.Tokens;
using NUnit.Framework.Constraints;
using SmallLang;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;
using SmallLang.Parser;

namespace SmallLangTest;

using Node = Common.AST.DynamicASTNode<ImportantASTNodeType, Attributes>;
using NodeType = ImportantASTNodeType;
[TestFixture]
public class ParserTest
{
    T Parse<T>(string input) => new Parser(input).Parse<T>();
    SectionNode Parse(string input) => Parse<SectionNode>(input);
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
            var res = new Parser("frozen readonly int i = 0; Function(1, 2, 3); if(i == 2){return 1;} while(true){i =  i + 1;} return 2; break ident;").Parse<SectionNode>();

            //var res = new Parser("1").Parse();
            Assert.That(res, Is.InstanceOf<SectionNode>());
            Assert.That(res.Statements, Has.Count.EqualTo(6));
            Assert.That(res.Statements[0], Is.InstanceOf<DeclarationNode>());
            Assert.That(res.Statements[1], Is.InstanceOf<FunctionCallNode>());
            Assert.That(res.Statements[2], Is.InstanceOf<IfNode>());
            Assert.That(res.Statements[3], Is.InstanceOf<WhileNode>());
            Assert.That(res.Statements[4], Is.InstanceOf<ReturnNode>());
            Assert.That(res.Statements[5], Is.InstanceOf<LoopCTRLNode>());
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
            var res1 = new Parser($"x {Operator} 1.2;").Parse<SectionNode>();
            Assert.That(res1, Is.InstanceOf<SectionNode>());
            Assert.That(res1.Statements, Has.Count.EqualTo(1));
            var _res = res1.Statements[0];
            Assert.That(_res, Is.InstanceOf<BinaryExpressionNode>());
            var res = (BinaryExpressionNode)_res;
            Assert.That(res.Data, Is.Not.Null);
            Assert.That(res.Data!.Lexeme, Is.EqualTo(Operator));
            Assert.That(res.Left, Is.InstanceOf<IdentifierNode>());
            Assert.That(res.Right, Is.InstanceOf<PrimaryNode>());
        });
    }
    public static IEnumerable<TestCaseData> GetComparisonOperator()
    {
        return new string[] { "==", "!=", "<", "<=", ">=", ">" }.Select(x => new TestCaseData(x));
    }
    void AssertOpExprPair<T>(OperatorExpressionPairNode res, string OpLexeme, TokenType OpTT)
    {
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.InstanceOf<OperatorExpressionPairNode>());
            Assert.That(res.Expression, Is.InstanceOf<T>());
            Assert.That(res.Data.TT, Is.EqualTo(OpTT));
            Assert.That(res.Data.Lexeme, Is.EqualTo(OpLexeme));
        });
    }
    [Test]
    public void Parse__Comparisons__Returns_Correct()
    {
        var res1 = new Parser("1 == x != (int i = 0) < Func(1,2, 3) <= 1 + 2 ** 3 >= Array[8 + 7] > Final;").Parse<SectionNode>();
        Assert.Multiple(() =>
        {
            var _res = res1.Statements[0];
            Assert.That(_res, Is.InstanceOf<ComparisonExpressionNode>());

            var res = (ComparisonExpressionNode)_res;

            Assert.That(res.OperatorExpressionPairs, Has.Count.EqualTo(6));

            Assert.That(res.Expression, Is.InstanceOf<PrimaryNode>());
            Assert.That(((PrimaryNode)res.Expression).Data!.Lexeme, Is.EqualTo("1"));
            Assert.That(((PrimaryNode)res.Expression).Data!.TT, Is.EqualTo(TokenType.Number));

            AssertOpExprPair<IdentifierNode>(res.OperatorExpressionPairs[0], "==", TokenType.EqualTo);
            AssertOpExprPair<DeclarationNode>(res.OperatorExpressionPairs[1], "!=", TokenType.NotEqualTo);
            AssertOpExprPair<FunctionCallNode>(res.OperatorExpressionPairs[2], "<", TokenType.LessThan);
            AssertOpExprPair<BinaryExpressionNode>(res.OperatorExpressionPairs[3], "<=", TokenType.LessThanOrEqualTo);
            AssertOpExprPair<IndexNode>(res.OperatorExpressionPairs[4], ">=", TokenType.GreaterThanOrEqualTo);
            AssertOpExprPair<IdentifierNode>(res.OperatorExpressionPairs[5], ">", TokenType.GreaterThan);
        });
    }
    [Test]
    public void Parse__Return_Statement__Returns_Correct()
    {
        var res1 = Parse<SectionNode>("return 1 + 1 - 2 * FunctionCall(1, 2, 3, 4, 5);");
        var _res = res1.Statements[0];
        Assert.That(_res, Is.InstanceOf<ReturnNode>());

        var res = _res as ReturnNode;

        Assert.That(res!.Expression, Is.InstanceOf<BinaryExpressionNode>());
    }
    [TestCase("break", TokenType.Break)]
    [TestCase("continue", TokenType.Continue)]
    public void Parse__LCTRLStatements__Returns_Correct(string i, TokenType e)
    {
        var res = Parse<SectionNode>($"{i};").Statements.First() as LoopCTRLNode;
        Assert.That(res, Is.InstanceOf<LoopCTRLNode>());
        Assert.That(res.Data!.TT, Is.EqualTo(e));
        var res2 = Parse<SectionNode>($"{i} ident;").Statements[0] as LoopCTRLNode;
        Assert.That(res2, Is.InstanceOf<LoopCTRLNode>());
        Assert.That(res2.Data!.TT, Is.EqualTo(e));
        Assert.That(res2.Identifier, Is.InstanceOf<IdentifierNode>());
    }
    [Test]
    public void Parse__For_Loop__Returns_Correct()
    {
        var res = Parse($"for (int i = 0; i < Func(); i = i + 1) as Label \n{{\t j = j + 1; \n\t DoSomething(i, j);\n}} else \n{{\n\tDoElse();}}").Statements.Single() as ForNode;
        Assert.That(res, Is.InstanceOf<ForNode>());
        Assert.That(res.InitializingExpression, Is.InstanceOf<DeclarationNode>());
        Assert.That(res.ConditionExpression, Is.InstanceOf<ComparisonExpressionNode>());
        Assert.That(res.PostLoopExpression, Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(res.LoopLabel, Is.InstanceOf<LoopLabelNode>());
        Assert.That(res.LoopBody, Is.InstanceOf<SectionNode>());
        Assert.That(((SectionNode)res.LoopBody).Statements[0], Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(((SectionNode)res.LoopBody).Statements[1], Is.InstanceOf<FunctionCallNode>());

        Assert.That(res.Else, Is.Not.Null);
        Assert.That(res.Else!.Statement, Is.InstanceOf<SectionNode>());
        Assert.That(((SectionNode)res.Else.Statement).Statements[0], Is.InstanceOf<FunctionCallNode>());
    }
    [Test]
    public void Parse__While_Loop__Returns_Correct()
    {
        var res = Parse($"while (Func()) {{\n\t j = j + 1;\n\t DoSomething(i, j);\n}} else {{}}").Statements.Single() as WhileNode;
        Assert.That(res, Is.InstanceOf<WhileNode>());
        Assert.That(res.ConditionExpression, Is.InstanceOf<FunctionCallNode>());
        Assert.That(res.LoopBody, Is.InstanceOf<SectionNode>());
        var LoopBody = res.LoopBody as SectionNode;
        Assert.That(LoopBody.Statements[0], Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(LoopBody.Statements[1], Is.InstanceOf<FunctionCallNode>());
    }
    [TestCase("for (int i = 0; i < 1; i = i + 1){}", false, false)]
    [TestCase("for (int i = 0; i < 1; i = i + 1) as Label{}", true, false)]
    [TestCase("for (int i = 0; i < 1; i = i + 1){} else {}", false, true)]
    [TestCase("for (int i = 0; i < 1; i = i + 1) as Label {} else {}", true, true)]
    [TestCase("for (int i = 0; i < 1; i = i + 1){}", false, false)]
    [TestCase("while (i < 1) as Label{}", true, false)]
    [TestCase("while (i < 1){} else {}", false, true)]
    [TestCase("while (i < 1) as Label {} else {}", true, true)]
    [TestCase("while (i){}", false, false)]
    public void Parse__Loop__Returns_Correct_Length_Children(string loop, bool LabelExists, bool ElseExists)
    {
        var res = Parse(loop).Statements.Single() as ILoopNode;
        Assert.That(res, Is.InstanceOf<ILoopNode>());
        if (res is ForNode For)
        {
            Assert.That(For.LoopLabel == null, Is.EqualTo(!LabelExists));
            Assert.That(For.Else == null, Is.EqualTo(!ElseExists));
        }
        else if (res is WhileNode While)
        {

            Assert.That(While.LoopLabel == null, Is.EqualTo(!LabelExists));
            Assert.That(While.Else == null, Is.EqualTo(!ElseExists));
        }
        else
        {
            Assert.Fail();
        }
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
        var res = Parse($"x {i} 1;").Statements.First() as BinaryExpressionNode;
        Assert.That(res, Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(res.Data!.TT, Is.EqualTo(TokenType.Equals));
        Assert.That(res.Left, Is.InstanceOf<IdentifierNode>());
        Assert.That(((IdentifierNode)res.Left).Data!.Lexeme, Is.EqualTo("x"));
        var add = res.Right as BinaryExpressionNode;
        Assert.That(add, Is.InstanceOf<BinaryExpressionNode>());

        Assert.That(((IdentifierNode)add.Left).Data!.Lexeme, Is.EqualTo("x"));
        Assert.That(((PrimaryNode)add.Right).Data!.TT, Is.EqualTo(TokenType.Number));
        Assert.That(add.Data!.TT, Is.EqualTo(expected));
    }
    [TestCase("++", TokenType.Addition)]
    [TestCase("--", TokenType.Subtraction)]
    public void Parse__Precrement_Assignment_Operators__Returns_Correct(string i, TokenType expected)
    {
        var res = Parse($"{i}x;").Statements.Single() as BinaryExpressionNode;
        Assert.That(res, Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(res.Data!.TT, Is.EqualTo(TokenType.Equals));
        Assert.That(res.Left, Is.InstanceOf<IdentifierNode>());
        Assert.That(((IdentifierNode)res.Left).Data!.Lexeme, Is.EqualTo("x"));
        var add = res.Right as BinaryExpressionNode;
        Assert.That(add, Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(((IdentifierNode)add.Left).Data!.Lexeme, Is.EqualTo("x"));
        Assert.That(((PrimaryNode)add.Right).Data!.TT, Is.EqualTo(TokenType.Number));
        Assert.That(((PrimaryNode)add.Right).Data!.Lexeme, Is.EqualTo("1"));
        Assert.That(add.Data!.TT, Is.EqualTo(expected));
    }
    [Test]
    public void Parse__If__Returns_Correct()
    {
        var res = Parse("if(i){} else if (i < 2)i + 1; else {}").Statements.Single() as IfNode;
        Assert.That(res, Is.InstanceOf<IfNode>());
        Assert.That(res.ExprStatementCombineds, Has.All.InstanceOf<ExprSectionCombinedNode>()); // redundant
        Assert.That(res.Else, Is.InstanceOf<ElseNode>());
        Assert.That(res.ExprStatementCombineds[1].Expression, Is.InstanceOf<ComparisonExpressionNode>());
        Assert.That(res.ExprStatementCombineds[0].Expression, Is.InstanceOf<IdentifierNode>());
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
        var res = Parse(i).Statements.Single() as SwitchNode;
        Assert.That(res, Is.InstanceOf<SwitchNode>());
        Assert.That(res.Expression, Is.InstanceOf<FunctionCallNode>());
        Assert.That(res.ExprStatementCombineds[0].Expression, Is.InstanceOf<PrimaryNode>());
        Assert.That(res.ExprStatementCombineds[0].Section.Statements[0], Is.InstanceOf<FunctionCallNode>());
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


    [TestCase("1;", TokenType.Number)]
    [TestCase("\"abc\";", TokenType.String)]
    [TestCase("\"a\";", TokenType.String)]
    [TestCase("1.1;", TokenType.Number)]
    public void Test__Primary__Returns_Correct(string Primary, TokenType Expected)
    {
        var res = Parse(Primary);
        Assert.That(res.Statements.First(), Is.InstanceOf<PrimaryNode>());
        Assert.That(((PrimaryNode)res.Statements[0]).Data.TT, Is.EqualTo(Expected));
    }

    [Test]
    public void Test__Index__Returns_Correct()
    {
        var res = Parse("(1 + 1)[x];").Statements.First() as IndexNode;
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Expression1, Is.InstanceOf<BinaryExpressionNode>());
        Assert.That(res.Expression2, Is.InstanceOf<IdentifierNode>());
    }

    [Test]
    public void Test__FunctionCall__Returns_Correct()
    {
        var res = Parse("foo();").Statements.First() as FunctionCallNode;
        Assert.That(res, Is.Not.Null);
    }
}