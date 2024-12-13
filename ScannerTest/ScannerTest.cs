using System;
using System.Collections.Generic;
using System.Linq;
using Common.Lexer;
using Common.Tokens;
using NUnit.Framework;

namespace Common.Tests.ScannerTest;
[TestFixture]
public class ScannerTests
{
    private IScanner _scanner = IScanner.NewScanner("");

    public void Setup(string input)
    {
        _scanner = IScanner.NewScanner(input);
    }

    // Parameterized test for single-token inputs
    // [TestCase("a", TokenType.Identifier, "a")]
    [TestCase("float", TokenType.Float, "float")]
    [TestCase("int", TokenType.Int, "int")]
    [TestCase("42", TokenType.Number, "42")]
    [TestCase("myVar", TokenType.Identifier, "myVar")]
    [TestCase("+", TokenType.Addition, "+")]
    [TestCase("-", TokenType.Subtraction, "-")]
    [TestCase("*", TokenType.Multiplication, "*")]
    [TestCase("/", TokenType.Division, "/")]
    [TestCase("~", TokenType.BitwiseNegation, "~")]
    [TestCase("&", TokenType.BitwiseAnd, "&")]
    [TestCase("|", TokenType.BitwiseOr, "|")]
    [TestCase("^", TokenType.BitwiseXor, "^")]
    [TestCase("<<", TokenType.BitwiseLeftShift, "<<")]
    [TestCase(">>", TokenType.BitwiseRightShift, ">>")]
    [TestCase("and", TokenType.LogicalAnd, "and")]
    [TestCase("or", TokenType.LogicalOr, "or")]
    [TestCase("not", TokenType.LogicalNot, "not")]
    [TestCase("xor", TokenType.LogicalXor, "xor")]
    [TestCase("implies", TokenType.LogicalImplies, "implies")]
    [TestCase("==", TokenType.EqualTo, "==")]
    [TestCase("!=", TokenType.NotEqualTo, "!=")]
    [TestCase(">", TokenType.GreaterThan, ">")]
    [TestCase("<", TokenType.LessThan, "<")]
    [TestCase(">=", TokenType.GreaterThanOrEqualTo, ">=")]
    [TestCase("<=", TokenType.LessThanOrEqualTo, "<=")]
    [TestCase("is", TokenType.ComparisonIs, "is")]
    [TestCase("in", TokenType.In, "in")]
    [TestCase(";", TokenType.Semicolon, ";")]
    [TestCase("(", TokenType.OpenParen, "(")]
    [TestCase(")", TokenType.CloseParen, ")")]
    [TestCase(",", TokenType.Comma, ",")]
    public void Scan_SingleToken_ReturnsCorrectToken(string input, TokenType expectedType, string expectedLexeme)
    {
        Setup(input);
        var tokens = _scanner.Scan().ToList();

        Assert.That(tokens, Has.Count.EqualTo(1));
        AssertToken(tokens[0], expectedType, expectedLexeme);
    }

    // Complex expression tests
    private static IEnumerable<TestCaseData> ComplexExpressions()
    {
        yield return new TestCaseData(
            "a + b * c",
            new List<(TokenType, string)>
            {
                    (TokenType.Identifier, "a"),
                    (TokenType.Addition, "+"),
                    (TokenType.Identifier, "b"),
                    (TokenType.Multiplication, "*"),
                    (TokenType.Identifier, "c")
            }
        );
        yield return new TestCaseData(
            "a+b<=c",
            new List<(TokenType, string)>
            {
                    (TokenType.Identifier, "a"),
                    (TokenType.Addition, "+"),
                    (TokenType.Identifier, "b"),
                    (TokenType.LessThanOrEqualTo, "<="),
                    (TokenType.Identifier, "c")
            }
        );
        yield return new TestCaseData(
            "foo(42, 3)",
            new List<(TokenType, string)>
            {
                    (TokenType.Identifier, "foo"),
                    (TokenType.OpenParen, "("),
                    (TokenType.Number, "42"),
                    (TokenType.Comma, ","),
                    (TokenType.Number, "3"),
                    (TokenType.CloseParen, ")")
            }
        );
        yield return new TestCaseData(
            "(1, 3, and is 4)",
            new List<(TokenType, string)>
            {
                    (TokenType.OpenParen, "("),
                    (TokenType.Number, "1"),
                    (TokenType.Comma, ","),
                    (TokenType.Number, "3"),
                    (TokenType.Comma, ","),
                    (TokenType.LogicalAnd, "and"),
                    (TokenType.ComparisonIs, "is"),
                    (TokenType.Number, "4"),
                    (TokenType.CloseParen, ")")
            }
        );
        yield return new TestCaseData(
            "_a = 1123;",
            new List<(TokenType, string)>
            {
                (TokenType.Identifier, "_a"),
                (TokenType.Equals, "="),
                (TokenType.Number, "1123"),
                (TokenType.Semicolon, ";")
            }
        );
    }

    [TestCaseSource(nameof(ComplexExpressions))]
    public void Scan_ComplexExpression_ReturnsCorrectTokens(string input, List<(TokenType, string)> expectedTokens)
    {
        Setup(input);
        var tokens = _scanner.Scan().ToList();

        Assert.That(expectedTokens, Has.Count.EqualTo(tokens.Count));
        for (int i = 0; i < expectedTokens.Count; i++)
        {
            AssertToken(tokens[i], expectedTokens[i].Item1, expectedTokens[i].Item2);
        }
    }

    // Granular tests for error handling
    [Test]
    public void Scan_InvalidCharacter_ThrowsException()
    {
        var input = "$invalid$";
        Setup(input);

        var ex = Assert.Throws<Exception>(() => _scanner.Scan().ToList());
    }

    [Test]
    public void Scan_SingleExclamationMark_ThrowsException()
    {
        var input = "!";
        Setup(input);

        Exception? exception = Assert.Throws<Exception>(() => _scanner.Scan().ToList());
        var ex = exception;
    }

    // Helper method for assertions
    private void AssertToken(IToken token, TokenType expectedType, string expectedLexeme)
    {
        Assert.Multiple(() =>
        {
            Assert.That(expectedType, Is.EqualTo(token.TT));
            Assert.That(expectedLexeme, Is.EqualTo(token.Lexeme));
        });
    }
}

