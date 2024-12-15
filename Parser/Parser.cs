using Common.Tokens;
using Common.AST;
using System.Linq.Expressions;

namespace Parser;
public class Parser
{
    public List<IToken> Input { get; set; } //assume it ends in EOF
    int Current = 0;
    Stack<int> State = new();
    ILogger Log { get; init; }
    void SaveState()
    {
        State.Push(Current);
    }
    void RollBack()
    {
        Current = State.Pop();
    }
    public Parser(IEnumerable<IToken> tokens, ILogger? logger = null)
    {
        this.Input = tokens.ToList();
        if (Input.Count > 0 && this.Input[^1].TT != TokenType.EOF)
        {
            Input.Add(IToken.NewToken(TokenType.EOF, "EOF", -1));
        }
        Log = logger ?? new Logger();
    }
    public bool Parse(out ASTNode? node)
    {
        //returns true if parse success; node will be of type AST. If parse failure, returns false; node is undefined
        if (Input.Count == 0)
        {
            node = null;
            return false;
        }
        if (!SafeParse(Program, out node))
        {
            return false;
        }
        if (Input[Current].TT != TokenType.EOF)
        {
            Log.Log($"Unexpected Tokens {string.Join(' ', Input[Current..].Select(x => x.Lexeme.ToString()))}");
            return false;
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
    delegate bool ParsingFunction(out ASTNode? Node);
    bool SafeParse(ParsingFunction fn, out ASTNode? K) //return true on successful parse, else false. Node is undefined on faliure.
    {
        SaveState();
        bool Result = fn(out ASTNode? Node);
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
    bool Program(out ASTNode? Node)
    {
        if (!SafeParse(Expression, out ASTNode? Expr))
        {
            Node = null;
            return false;
        }
        //check if repeat(comma is there); success is valid in any case
        if (Input[Current].TT == TokenType.Comma)
        {
            Current++;
            if (SafeParse(Program, out ASTNode? Repeat))
            {
                Node = ASTNode.Repeating(Expr!, Repeat!, nameof(Program));
                return true;
            }
            else
            {
                //cannot have comma but no repeat
                Node = null;
                return false;
            }
        }

        else
        {
            Node = ASTNode.NonTerminal(Expr!, nameof(Program));
            return true;
        }
    }
    bool Expression(out ASTNode? Node)
    {
        if (SafeParse(Addition, out ASTNode? Add))
        {
            Node = ASTNode.NonTerminal(Add!, nameof(Expression));
            return true;
        }
        Node = null;
        return false;
    }
    bool Addition(out ASTNode? Node)
    {
        if (!SafeParse(Multiplication, out ASTNode? Mul) || !SafeParse(AdditionPrime, out ASTNode? AddP)) //if either mul or add fails, fail the parse
        {
            Node = null;
            return false;
        }
        Node = ASTNode.PrimedBinary(Mul!, AddP!, nameof(Addition));
        return true;
    }
    bool Multiplication(out ASTNode? Node)
    {
        if (SafeParse(Negation, out ASTNode? Neg) && SafeParse(MultiplicationPrime, out ASTNode? MulP))
        {
            Node = ASTNode.PrimedBinary(Neg!, MulP!, nameof(Multiplication));
            return true;
        }
        Node = null;
        return false;
    }
    bool MultiplicationPrime(out ASTNode? Node)
    {
        if (Input[Current].TT == TokenType.Multiplication || Input[Current].TT == TokenType.Division)
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(Negation, out ASTNode? Neg) && SafeParse(MultiplicationPrime, out ASTNode? MulP))
            {
                Node = ASTNode.BinaryPrime(Operator: Operator, Right: Neg!, Repeat: MulP!, nameof(MultiplicationPrime));
                return true;
            }
            else
            {
                Node = null;
                return false;
            }
        }

        //if neither * or /, must be empty
        Node = ASTNode.NonTerminal(ASTNode.Empty(), nameof(MultiplicationPrime));
        return true;
    }
    bool Negation(out ASTNode? Node)
    {
        if (Input[Current].TT == TokenType.Subtraction)
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(Expression, out ASTNode? Expr))
            {
                Node = ASTNode.Unary(Operator: Input[Current], Operand: Expr!, nameof(Negation));
                return true;
            }
        }
        else if (SafeParse(Primary, out ASTNode? PrimaryNode))
        {
            Node = ASTNode.NonTerminal(PrimaryNode!, nameof(Negation));
            return true;
        }
        Node = null;
        return false;
    }
    bool Primary(out ASTNode? Node) //assuming scanner can currently only scan floats; TODO: Upgrade scanner
    {
        if (Input[Current].TT == TokenType.OpenParen)
        {
            IToken Operator = Input[Current];
            Current++;
            if (SafeParse(Expression, out ASTNode? Expr) && Input[Current].TT == TokenType.CloseParen)
            {
                Node = ASTNode.Parenthesized(Open: Operator, Center: Expr!, Close: Input[Current], nameof(Primary));
                Current++;
                return true;
            }
        }
        else if (Input[Current].TT == TokenType.Number)
        {
            Node = ASTNode.Terminal(Input[Current], nameof(Primary));
            Current++;
            return true;
        }
        Node = null;
        return false;
    }
    bool AdditionPrime(out ASTNode? Node)
    {
        if (Input[Current].TT == TokenType.Addition || Input[Current].TT == TokenType.Subtraction)
        {
            IToken Operator = Input[Current];

            Current++;
            if (SafeParse(Multiplication, out ASTNode? Mul) && SafeParse(AdditionPrime, out ASTNode? AddP))
            {
                Node = ASTNode.BinaryPrime(Operator: Operator, Right: Mul!, Repeat: AddP!, nameof(AdditionPrime));
                return true;
            }
            else
            {
                Node = null;
                return false;
            }
        }
        //if neither + or -, must be empty
        Node = ASTNode.NonTerminal(ASTNode.Empty(), nameof(AdditionPrime));
        return true;
    }
}