using Common.Tokens;
using Common.AST;
using System.Linq.Expressions;
using Common.Logger;
namespace Parser;
public class Parser
{
    public List<IToken> Input { get; set; } //assume it ends in EOF
    int Current = 0;
    Stack<int> State = new();
    ILogger Log { get; init; }

    private int Position { get => Current; }
    readonly HashSet<TokenType> RecoveryTokens = [TokenType.Semicolon];
    delegate bool ParsingFunction(out ASTNode? Node);
    #region SafeParse
    void SaveState()
    {
        State.Push(Current);
    }
    void RollBack()
    {
        Current = State.Pop();
    }
    bool SafeParse(ParsingFunction fn, out ASTNode? K, bool Suppress = true) //return true on successful parse, else false. Node is undefined on faliure.
    {
        SaveState();
        if (Suppress)
        {
            Log.SuppressLog();
        }
        bool Result = fn(out ASTNode? Node);
        Log.EnableLog();
        if (Result)
        {
            State.Pop();
            K = Node;
            return true;
        }
        else
        {
            RollBack();
            K = null;
            return false;
        }
    }
    #endregion
    public Parser(IEnumerable<IToken> tokens, ILogger? logger = null)
    {
        this.Input = tokens.ToList();
        if (Input.Count > 0 && this.Input[^1].TT != TokenType.EOF)
        {
            Input.Add(IToken.NewToken(TokenType.EOF, "EOF", -1));
        }
        Log = logger ?? new Logger();
    }
    #region InstanceAndStaticParse
    public bool Parse(out ASTNode? node)
    {
        //returns true if parse success; node will be of type AST. If parse failure, returns false; node is undefined
        if (Input.Count == 0)
        {
            node = null;
            return false;
        }
        else if (!SafeParse(Program, out node))
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

    public static bool Parse(IEnumerable<IToken> Input, out ASTNode? Node, ILogger? Log = null)
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
    #region TCMP
    bool TCmp(TokenType tokenType, int offset = 0)
    {
        if (offset < Input.Count - 1)
        {
            return Input[Current + offset].TT == tokenType;
        }
        else
        {
            return false;
        }
    }
    bool TCmp(IEnumerable<TokenType> set, int offset = 0)
    {
        return set.Select(x => TCmp(x, offset)).Contains(true);
    }
    bool ICmp(TokenType tokenType, int offset = 0) // increments the index on cmp success
    {
        if (TCmp(tokenType, offset))
        {
            Current++;
            return true;
        }
        return false;
    }
    #endregion
    #region GenericParsingMethods
    /// <summary>
    /// <Operation> ::= <NextInPriority> <OperationPrime>;
    /// Example:
    /// <Addition> ::= <Multiplication> <AdditionPrime>;
    /// Current Production Name is for naming the AST Node generated
    /// </summary>
    /// <param name="NextInPriority"><NextInPriority></param>
    /// <param name="BinaryPrime"><OperationPrime></param>
    /// <param name="CurrentProductionName"><Operation></param>
    /// <param name="Node">Out</param>
    /// <returns></returns>
    bool PrimedBinary(ParsingFunction NextInPriority, ParsingFunction BinaryPrime, string CurrentProductionName, out ASTNode? Node, Func<int, string>? ErrorMessage = null)
    {
        if (SafeParse(NextInPriority, out ASTNode? Neg, Suppress: false) && SafeParse(BinaryPrime, out ASTNode? MulP, Suppress: false))
        {
            Node = ASTNode.PrimedBinary(Neg!, MulP!, CurrentProductionName);
            return true;
        }
        Log.Log((ErrorMessage ?? ((x) => $"Error in PrimedBinary at {x}"))(Position));
        Node = null;
        return false;
        //no need for error message here as the explanation of why NextInPriority or BinaryPrime failed to parse will be handled by those methods
    }
    /// <summary>
    /// <OperationPrime> ::= <Operator> <paramref name="NextInPriority"/> <OperationPrime> |
    /// <Operator2> <paramref name="NextInPriority"/> <OperationPrime> | ... |
    /// <Empty>;
    /// Example:
    /// <AdditionPrime> ::=
    ///     "+" <Multiplication> <AdditionPrime> |
    ///     "-" <Multiplication> <AdditionPrime> |
    ///     <Empty>
    ///;
    /// </summary>
    /// <param name="NextInPriority"></param>
    /// <param name="Operators">Set of all valid operators</param>
    /// <param name="CurrentProductionName"></param>
    /// <param name="Node"></param>
    /// <returns></returns>
    bool BinaryPrime(ParsingFunction NextInPriority, ICollection<TokenType> Operators, string CurrentProductionName, out ASTNode? Node, Func<int, string>? MessageOnError = null)
    {
        bool Self(out ASTNode? node) => BinaryPrime(NextInPriority, Operators, CurrentProductionName, out node); //function representing recursive call on self; i.e. the BinaryPrime part of the paths where this is not empty
        if (Operators.Contains(Input[Current].TT))
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(NextInPriority, out ASTNode? ParentPrimedNode, Suppress: false) && SafeParse(Self, out ASTNode? PrimeNode, Suppress: false))
            {
                Node = ASTNode.BinaryPrime(Operator: Operator, Right: ParentPrimedNode!, Repeat: PrimeNode!, CurrentProductionName);
                return true;
            }
            else
            {
                Log.Log((MessageOnError ?? ((x) => $"Error in BinaryPrime at {x}"))(Position));
                Node = null;
                return false;
                //similarly, faliure to parse ParentPrimedNode or NextInPriority is handled by those methods
            }
        }

        //if not **  must be empty
        Node = ASTNode.Empty(CurrentProductionName);
        return true;
    }
    #endregion
    bool Program(out ASTNode? Node)
    {
        if (!SafeParse(Expression, out ASTNode? Expr))
        {
            Node = null;
            return false;
        }
        //check if repeat(Semicolon is there); success is not valid in any case
        if (Input[Current].TT == TokenType.Semicolon)
        {
            Current++;
            if (SafeParse(Program, out ASTNode? Repeat))
            {
                Node = ASTNode.Repeating(Expr!, Repeat!, nameof(Program));
                return true;
            }
            else
            {
                //check if EOF
                //if no repeat, must be EOF
                if (Input[Current].TT != TokenType.EOF)
                {
                    Log.Log($"Expected EOF at Token Position {Position} but got \"{Input[Current].Lexeme}\"");
                    Node = null;
                    return false;
                }
                IToken OperatorSemicolon = Input[Current];
                Node = new ASTNode([ASTLeafType.NonTerminal, ASTLeafType.Terminal], [Expr!, OperatorSemicolon], nameof(Program));
                return true;
            }
        }

        else
        {
            Log.Log($"Expected \";\" at {Position}");
            Node = null;
            return false;
        }
    }
    bool Expression(out ASTNode? Node)
    {
        if (SafeParse(Declaration, out ASTNode? Add, Suppress: false)) //no additional context to add here so we get the context from safeparse
        {
            Node = ASTNode.NonTerminal(Add!, nameof(Expression));
            return true;
        }
        Node = null;
        return false;
    }
    bool Declaration(out ASTNode? Node)
    {
        if (SafeParse(Type, out ASTNode? TNode))
        {
            if (!ICmp(TokenType.Identifier))
            {
                Log.Log($"Expected Identifier after Type at position {Position}");
                Node = null;
                return false;
            }
            if (SafeParse(AssignmentPrime, out ASTNode? ANode))
            {
                Node = ASTNode.Binary(TNode!, Input[Current - 1], ANode!, nameof(Declaration));
                return true;
            }
            else
            {
                Log.Log($"Impossible path in {nameof(Declaration)}");
                Node = null;
                return false;
            }
        }
        else if (SafeParse(Addition, out ASTNode? Add, Suppress: false))
        {
            Node = ASTNode.NonTerminal(Add!, nameof(Declaration));
            return true;
        }
        Node = null;
        Log.Log($"Expected addition or declaration (Type) at position {Position}");
        return false;
    }
    bool AssignmentPrime(out ASTNode? Node)
    => BinaryPrime(Addition, [TokenType.Equals], nameof(AssignmentPrime), out Node, (Pos) => $"Impossible Path in {nameof(AssignmentPrime)}");

    bool Addition(out ASTNode? Node) => PrimedBinary(Multiplication, AdditionPrime, nameof(Addition), out Node, (_) => "");
    bool AdditionPrime(out ASTNode? Node) => BinaryPrime(Multiplication, [TokenType.Addition, TokenType.Subtraction], nameof(AdditionPrime), out Node, (_) => "");

    bool Multiplication(out ASTNode? Node) => PrimedBinary(Power, MultiplicationPrime, nameof(Multiplication), out Node, (_) => "");
    bool MultiplicationPrime(out ASTNode? Node) => BinaryPrime(Power, [TokenType.Multiplication, TokenType.Division], nameof(MultiplicationPrime), out Node, (_) => "");


    bool Power(out ASTNode? Node) => PrimedBinary(Negation, PowerPrime, nameof(Power), out Node, (_) => "");
    bool PowerPrime(out ASTNode? Node) => BinaryPrime(Negation, [TokenType.Exponentiation], nameof(PowerPrime), out Node, (_) => "");
    bool Negation(out ASTNode? Node)
    {
        if (Input[Current].TT == TokenType.Subtraction)
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(Expression, out ASTNode? Expr, Suppress: false))
            {
                Node = ASTNode.Unary(Operator: Operator, Operand: Expr!, nameof(Negation));
                return true;
            }
        }
        else if (SafeParse(Primary, out ASTNode? PrimaryNode, Suppress: false))
        {
            Node = ASTNode.NonTerminal(PrimaryNode!, nameof(Negation));
            return true;
        }
        //we can use current here because being here means primary also failed, and thus current is rolled back
        Log.Log($"Expected \"-\" or Primary at Token Position {Position}, but got \"{Input[Current].Lexeme}\"; Error may be here, or at Primary:");

        Node = null;
        return false;
    }
    bool Primary(out ASTNode? Node) //assuming scanner can currently only scan floats; TODO: Upgrade scanner
    {
        if (Input[Current].TT == TokenType.OpenParen)
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(Expression, out ASTNode? Expr, Suppress: false))
            {
                if (Input[Current].TT == TokenType.CloseParen)
                {
                    Node = ASTNode.Parenthesized(Open: Operator, Center: Expr!, Close: Input[Current], nameof(Primary));
                    Current++;
                    return true;
                }
                else
                {
                    Log.Log($"Expected Close Parenthesis at position {Position}");
                    Node = null;
                    return false;
                }
            }
            else
            {
                //no message as not suppressed
                Node = null;
                return false;
            }
        }
        else if (TCmp(TokenType.Identifier))
        {
            ASTNode IdentifierNode = ASTNode.Terminal(Input[Current++], nameof(Primary));
            if (SafeParse(AssignmentPrime, out ASTNode? AssP))
            {
                Node = ASTNode.PrimedBinary(IdentifierNode, AssP!, nameof(Primary));
                return true;
            }
            Node = null;
            return false;
        }
        else if (TCmp(TokenType.Number))
        {
            ASTNode NumberNode = ASTNode.Terminal(Input[Current++], nameof(Primary));
            Node = NumberNode;
            return true;
        }
        Log.Log($"Expected Open Parenthesis ( or Number or identifier at token position {Position}, but got \"{Input[Current].Lexeme}\"");
        Node = null;
        return false;
    }
    bool Type(out ASTNode? Node)
    {
        if (TCmp([TokenType.TypeByte, TokenType.TypeDouble, TokenType.TypeInt, TokenType.TypeLong, TokenType.TypeLongInt, TokenType.TypeFloat, TokenType.TypeNumber]))
        {
            Node = ASTNode.Terminal(Input[Current], nameof(Type));
            Current++;
            return true;
        }
        Log.Log($"Expected valid Type at {Position}");
        Node = null;
        return false;
    }

}