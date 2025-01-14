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
    bool Program(out AnnotatedNode<Annotations>? Node)
    {
        if (!SP.SafeParse(Expression, out AnnotatedNode<Annotations>? Expr, Current: ref Current))
        {
            Node = null;
            return false;
        }
        //assert semicolon
        IToken C = CurrentToken(Inc: true)!;
        if (C.TT != TokenType.Semicolon)
        {
            Log.Log($"Expected \";\" at {Position}");
            Node = null;
            return false;
        }
        //check for repeat(is not EOF)
        if (!CurrentToken().TCmp(TokenType.EOF))
        {
            if (SP.SafeParse(Program, out AnnotatedNode<Annotations>? Repeat, Current: ref Current))
            {
                Node = new(ASTNode.Repeating(Expr!, C, Repeat!, nameof(Program)));
                return true;
            }
            else
            {
                Log.Log($"Expected EOF at Token Position {Position} but got \"{Input[Current].Lexeme}\"");
                Node = null;
                return false;
            }
        }
        else
        {
            Node = new(new(IsEmpty: false), [ASTLeafType.NonTerminal, ASTLeafType.Terminal], [Expr!, C], nameof(Program));
            return true;
        }
    }

    public bool Expression(out AnnotatedNode<Annotations>? Node)
    {
        if (SP.SafeParse(Declaration, out AnnotatedNode<Annotations>? Add, Suppress: false, Current: ref Current)) //no additional context to add here so we get the context from safeparse
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, nameof(Expression))); //TypeCode <- Addition.TypeCode
            return true;
        }
        Node = null;
        return false;
    }
    public bool Declaration(out AnnotatedNode<Annotations>? Node)
    {
        if (SP.SafeParse(Type, out AnnotatedNode<Annotations>? TNode, Current: ref Current))
        {
            IToken IdentToken = CurrentToken(Inc: true)!;
            if (!IdentToken.TCmp(TokenType.Identifier))
            {
                Log.Log($"Expected Identifier after Type at position {Position}");
                Node = null;
                return false;
            }
            if (SP.SafeParse(AssignmentPrime, out AnnotatedNode<Annotations>? ANode, Current: ref Current))
            {
                Debug.Assert(IdentToken.TT == TokenType.Identifier);
                Debug.Assert(TNode!.Attributes.TypeDenotedByIdentifier is not null);
                //verify type safety if AssignmentPrime exists
                if (ANode!.Attributes.IsEmpty is false && !TP.CanBeDeclaredTo(TNode!.Attributes.TypeDenotedByIdentifier!, ANode!.Attributes.TypeCode!))
                {
                    //if there exists an assignmentprime and the declaration is not type-safe then we have an issue; if there does not exist an assignmentprime the declaration DNE so we don't care about types
                    Log.Log($"Type mismatch at position {Position}; Cannot assign {ANode!.Attributes.TypeCode} to {TNode!.Attributes.TypeDenotedByIdentifier}"); //TODO: Reverse typecodes for better error reporting
                    Node = null;
                    return false;
                }
                else
                {
                    TP.StoreIdentifierType(IdentToken.Lexeme, (uint)TNode!.Attributes.TypeDenotedByIdentifier); //store identifier type in type table upon declaration
                    Node = new(new(
                        TypeCode: ANode!.Attributes.IsEmpty is true ? null : TNode!.Attributes.TypeDenotedByIdentifier, //if AssignmentPrime is empty then we cannot give any type to this declaration as an expression
                        IsEmpty: false
                    ), ASTNode.Binary(TNode!, Input[Current - 1], ANode!, nameof(Declaration)));
                    return true;
                }
            }
            else
            {
                Log.Log($"Impossible path in {nameof(Declaration)}");
                Node = null;
                return false;
            }
        }
        else if (SP.SafeParse(Addition, out AnnotatedNode<Annotations>? Add, Suppress: false, Current: ref Current))
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, nameof(Declaration)));
            return true;
        }
        Node = null;
        Log.Log($"Expected addition or declaration (Type) at position {Position}");
        return false;
    }

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