using Common.AST;
using Common.Logger;
using Common.Tokens;
using MEXP.IRs.ParseTree;
using MEXP.Parser;

namespace MEXPTests.ParserTest;
[TestFixture]
public class TestParser
{
    static IEnumerable<IToken> Lex(string input)
    {
        var s = Common.Lexer.IScanner.NewScanner(input);
        return s.Scan();
    }
    #region string representation builders, unused
    static string Program(params string[] Children) => Build("Program", Children);
    static string Expression(params string[] Children) => Build("Expression", Children);
    static string Addition(params string[] Children) => Build("Addition", Children);
    static string AdditionPrime(params string[] Children) => Build("AdditionPrime", Children);
    static string Multiplication(params string[] Children) => Build("Multiplication", Children);
    static string MultiplicationPrime(params string[] Children) => Build("MultiplicationPrime", Children);
    static string Negation(params string[] Children) => Build("Negation", Children);
    static string Primary(params string[] Children) => Build("Primary", Children);
    static string PrimaryTerminal(params string[] Children) => Build("Primary-Terminal", Children);
    static string Build(string grammarname, string[] Children)
    {
        return $"{grammarname}: ({string.Join(", ", Children)})";
    }
    #endregion
    #region ParseValidCases
    [TestCase("5;")]
    [TestCase("-10;")]
    [TestCase("(3);")]
    [TestCase("(-42);")]
    [TestCase("5 + 3;")]
    [TestCase("10 - 7;")]
    [TestCase("(3 + 5) - 2;")]
    [TestCase("1 + 2 - 3 + 4;")]
    [TestCase("6 * 7;")]
    [TestCase("8 / 4;")]
    [TestCase("9 * (2 + 3);")]
    [TestCase("(6 / 3) * 4;")]
    [TestCase("10 * 2 / 5 * 3;")]
    [TestCase("1 + 2 * 3;")]
    [TestCase("(1 + 2) * 3;")]
    [TestCase("4 * (5 + 6 - 7);")]
    [TestCase("3 + 4 * 5 - 6 / 2;")]
    [TestCase("10 - (3 * (2 + 1));")]
    [TestCase("-5;")]
    [TestCase("-(3 + 2);")]
    [TestCase("-4 * 5;")]
    [TestCase("6 + -(7 - 2);")]
    [TestCase("-(-3);")]
    [TestCase("0;")]
    [TestCase("(-0);")]
    [TestCase("1 + (-2) * (3 - 4 / (5 + 6));")]
    [TestCase("((((1))));")]
    [TestCase("-(-(-1));")]
    [TestCase("5 + 3; 10 - 7;")]
    [TestCase("1 + 2 * 3;4 / 2;")]
    [TestCase("10 - (3 * (2 + 1)) ; 7 * 8;")]
    [TestCase("10 ** 2;")]
    [TestCase("1.3 ** 3.4;")]
    [TestCase("1.4 ** 3 * 2;")]
    [TestCase("-34 ** 5;")]
    [TestCase("2 - 234 ** 7 + 23;")]
    [TestCase("int i = 12;")]
    [TestCase("long i;")]
    [TestCase("_ = 1;")]
    [TestCase("int a234_; a234_;")]
    [TestCase("number x; x;")]
    [TestCase("1 + 2; 3 + 4;")]
    [TestCase("5;")]
    [TestCase("float i; int j; i = (j = 2);")]
    [TestCase("number i; longint j; i = 4 * (j = 2) + 2;")]
    [TestCase("int i = (long j = 2);")]
    [TestCase("int i = 2 + (long j = 4) * 3;")]
    #endregion
    public void Parse_Valid_Strings__Returns_True_And_Node_Is_Type_AST(string input)
    {
        Assert.Multiple(() =>
        {
            ILogger? Logger = null; //new MockLogger();
            Assert.That(Parser.Parse(Lex(input), out AnnotatedNode<Annotations>? node, Log: Logger), Is.True);
            Assert.That(node, Is.TypeOf<AnnotatedNode<Annotations>>());
            //Assert.That(Logger.LogRecord, Is.Empty);
        });
    }
    #region Invalid Cases
    [TestCase("", 0)]
    [TestCase("5,", 1)]
    [TestCase("5 + 3, 10 - 7", 1)]
    [TestCase("1 + 2 * 3, -4 / 2,", 1)]
    [TestCase("10 - (3 * (2 + 1)), 7 * 8,", 1)]
    [TestCase("5 + 3,; 10 - 7,", 2)]
    [TestCase("1 + 2 * 3,; -4 /, 2,", 3)]
    [TestCase("10 - (3 * (2 + 1)), 7 * 8,", 1)]
    #endregion
    public void Invalid_Cases__Returns_False_And_Prints_Error(string input, int MinimumErrorMessageCount) //do not assert for null because that is UB
    {
        bool OutMessages = false;
        var Logger = new MockLogger();
        Assert.Multiple(() =>
        {
            Assert.That(Parser.Parse(Lex(input), out AnnotatedNode<Annotations>? node, Logger), Is.False);
            Assert.That(Logger.LogRecord, Has.Count.GreaterThanOrEqualTo(MinimumErrorMessageCount));
        });

        // if (OutMessages)
        // {
        //     Console.WriteLine(string.Join("; ", Logger.LogRecord));
        // }
    }
    static IEnumerable<TestCaseData> AttributeTestData()
    {
        var T1 = new TypeProvider();
        yield return new("1;", T1, (AnnotatedNode<Annotations> x) => (AnnotatedNode<Annotations>)x.Children[0], new Annotations(TypeCode: T1.ByteTypeCode), (Annotations x) => x.TypeCode);
        var T2 = new TypeProvider();
        yield return new("1.;", T2, (AnnotatedNode<Annotations> x) => (AnnotatedNode<Annotations>)x.Children[0], new Annotations(TypeCode: T2.FloatTypeCode), (Annotations x) => x.TypeCode);
        T1 = new TypeProvider();
        yield return new("1 + 2;", T1, (AnnotatedNode<Annotations> x) => (AnnotatedNode<Annotations>)x.Children[0], new Annotations(TypeCode: T1.ByteTypeCode), (Annotations x) => x.TypeCode);
        T1 = new TypeProvider();
        yield return new("1 * 0.3;", T1, (AnnotatedNode<Annotations> x) => (AnnotatedNode<Annotations>)x.Children[0], new Annotations(TypeCode: T1.FloatTypeCode), (Annotations x) => x.TypeCode);
        T1 = new TypeProvider();
        yield return new("int x;", T1, (AnnotatedNode<Annotations> x) => (AnnotatedNode<Annotations>)(((AnnotatedNode<Annotations>)((AnnotatedNode<Annotations>)x.Children[0]).Children[0]).Children[0]), new Annotations(TypeDenotedByIdentifier: T1.IntTypeCode), (Annotations x) => x.TypeDenotedByIdentifier);
        yield return new("number x; x;", T1, (AnnotatedNode<Annotations> x) => ((AnnotatedNode<Annotations>)((AnnotatedNode<Annotations>)x.Children[2]).Children[0]), new Annotations(TypeCode: T1.NumberTypeCode), (Annotations x) => x.TypeCode);
        yield return new("number x; x;", T1, (AnnotatedNode<Annotations> x) => ((AnnotatedNode<Annotations>)((AnnotatedNode<Annotations>)x.Children[2]).Children[0]), new Annotations(CanBeResolvedToAssignable: true), (Annotations x) => x.CanBeResolvedToAssignable);
    }
    [TestCaseSource(nameof(AttributeTestData))]
    public void Attribute_Test<T>(string input, TypeProvider typeProvider, Func<AnnotatedNode<Annotations>, AnnotatedNode<Annotations>> Accessor, Annotations expectedAnnotations, Func<Annotations, T> AnnotationsAccessor)
    {

        Assert.Multiple(() =>
        {
            Parser p = new(Lex(input), null, typeProvider);
            Assert.That(p.Parse(out AnnotatedNode<Annotations>? node), Is.True);
            Assert.That(node, Is.Not.Null);
            Assert.That(Accessor(node!), Is.Not.Null);
            Assert.That(AnnotationsAccessor(Accessor(node!)?.Attributes!), Is.EqualTo(AnnotationsAccessor(expectedAnnotations)));
        });
    }
    //     //[TestCase("5 + 3, 10 - 7")]
    //     //[TestCase("10 - (3 * (2 + 1)) , 7 * 8")]
    //     // [Ignore("Only For Printing AST Easily; no printing necessary now")]
    //     // //[TestCase("1.4 ** 3 * 2")]
    //     // public void Tets(string inp)
    //     // {
    //     //     Parser.Parser.Parse(Lex(inp), out AnnotatedNode<Annotations>? node);
    //     //     Console.WriteLine(node!.Print());
    //     // }
}
