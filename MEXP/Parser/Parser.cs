using Common.Tokens;
using Common.AST;
using System.Linq.Expressions;
using Common.Logger;
using System.Diagnostics;
using MEXP.Parser.Internals;
namespace MEXP.Parser;
class Parser : IParser
{
    public List<IToken> Input { get; set; } //assume it ends in EOF
    public int Current = 0;
    public ILogger Log { get; init; }
    public int Position { get => Current; }
    readonly HashSet<TokenType> RecoveryTokens = [TokenType.Semicolon];
    public ITypeProvider TP { get; }
    public SafeParser SP { get; init; }
    public IToken? Advance()
    {
        Current++;
        return CurrentToken(-1);
    }
    public IToken? CurrentToken(int offset = 0, bool Inc = false)
    {
        if (Current + offset < Input.Count)
        {
            if (Inc)
            {
                return Input[Current++ + offset]; //generalization of Current++ not needed, I think
            }
            return Input[Current + offset];
        }
        return null;
    }
    public Parser(IEnumerable<IToken> tokens, ILogger? logger = null, ITypeProvider? TP = null)
    {
        this.Input = tokens.ToList();
        if (Input.Count > 0 && this.Input[^1].TT != TokenType.EOF)
        {
            Input.Add(IToken.NewToken(TokenType.EOF, "EOF", -1));
        }
        Log = logger ?? new Logger();
        SP = new(Log);
        this.TP = TP ?? new TypeProvider();
        TypeParser = new TypeParser(this);
        PrimaryParser = new PrimaryParser(this);
        NegationParser = new NegationParser(this);
        PowerParser = new PowerParser(this);
        PowerPrimeParser = new PowerPrimeParser(this);
        MultiplicationParser = new MultiplicationParser(this);
        MultiplicationPrimeParser = new MultiplicationPrimeParser(this);
        AdditionParser = new AdditionParser(this);
        AdditionPrimeParser = new AdditionPrimeParser(this);
        AssignmentPrimeParser = new AssignmentPrimeParser(this);
        DeclarationParser = new DeclarationParser(this);
        ExpressionParser = new ExpressionParser(this);
        ProgramParser = new ProgramParser(this);
    }
    #region InstanceAndStaticParse
    public bool Parse(out AnnotatedNode<Annotations>? node)
    {
        //returns true if parse success; node will be of type AST. If parse failure, returns false; node is undefined
        if (Input.Count == 0)
        {
            node = null;
            return false;
        }
        else if (!SP.SafeParse(Program, out node, Current: ref Current))
        {
            if (Recover())
            {
                Parse(out node);
                //if we skipped forwards, it means that we are parsing from a new location, so we can continue
                //but we still failed so
                return false;
            }
            else
            {
                //no skip forwards, we've discovered as many syntax errors as we can.
                node = null;
                return false;
            }
        }
        return true;
    }

    public static bool Parse(IEnumerable<IToken> Input, out AnnotatedNode<Annotations>? Node, ILogger? Log = null)
    {
        Parser parser;
        if (Log is not null)
        {
            parser = new Parser(Input, Log);
        }
        else
        {
            parser = new Parser(Input);
        }
        return parser.Parse(out Node);
    }
    #endregion
    bool Recover()
    {
        //skip forwards to the next token in RecoveryTokens. If we reach EOF before finding the next token, return false; else true
        while (Input[Current].TT != TokenType.EOF)
        {
            if (RecoveryTokens.Contains(Input[Current++].TT)) //current++ because no matter what we want to focus on the token after the one we are focusing on now
            {
                return true;
            }
        }
        return false;
    }
    public Annotations GetFromChildIndex(ASTNode node, int index)
    {
        if (node.Children.Length <= index)
        {
            throw new ArgumentOutOfRangeException($"Index out of range, {index}, {node.Children.Length}");
        }
        return ((AnnotatedNode<Annotations>)node.Children[index]).Attributes;
    }
    private InternalParserBase ProgramParser;
    public ParsingFunction Program => ProgramParser.Parse;
    private InternalParserBase ExpressionParser;
    public ParsingFunction Expression => ExpressionParser.Parse;
    private InternalParserBase DeclarationParser;
    public ParsingFunction Declaration => DeclarationParser.Parse;
    //<AssignmentPrime> ::= "=" <Addition> <AssignmentPrime> | <Empty>;
    private InternalParserBase AssignmentPrimeParser;
    public ParsingFunction AssignmentPrime => AssignmentPrimeParser.Parse;

    private InternalParserBase AdditionParser;
    public ParsingFunction Addition => AdditionParser.Parse;
    private InternalParserBase AdditionPrimeParser;
    public ParsingFunction AdditionPrime => AdditionPrimeParser.Parse;
    // AdditionPrime ::= ("-" | "+") Multiplication AdditionPrime | Empty
    private InternalParserBase MultiplicationParser;
    public ParsingFunction Multiplication => MultiplicationParser.Parse;

    private InternalParserBase MultiplicationPrimeParser;
    public ParsingFunction MultiplicationPrime => MultiplicationPrimeParser.Parse;
    private InternalParserBase PowerParser;
    public ParsingFunction Power => PowerParser.Parse;
    private InternalParserBase PowerPrimeParser;
    public ParsingFunction PowerPrime => PowerPrimeParser.Parse;
    private InternalParserBase NegationParser;
    public ParsingFunction Negation => NegationParser.Parse;
    private InternalParserBase PrimaryParser;
    public ParsingFunction Primary => PrimaryParser.Parse;
    private InternalParserBase TypeParser;
    public ParsingFunction Type => TypeParser.Parse;

}