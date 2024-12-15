using Common.Tokens;

namespace Common.AST;
public class ASTNode : IValidASTLeaf
{
    public ASTLeafType Type => ASTLeafType.NonTerminal;
    public ASTLeafType[] Pattern { get; init; }
    public IValidASTLeaf[] Children { get; init; }
    public string Name { get; init; }
    public ASTNode(ASTLeafType[] pattern, IValidASTLeaf[] children, string name)
    {
        Pattern = pattern;
        Children = children;
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].Type != pattern[i])
            {
                throw new ArgumentException("Children must match pattern");
            }
        }
        Name = name;
    }
    public string Print()
    {
        return $"{Name}: ({string.Join(", ", Children.Select(x => x is ASTNode k ? k.Print() : ((IToken)x).Lexeme))})";
    }
    public string LongNamePrint(string CallerLongName = "")
    {
        string LongName = $"{CallerLongName}-{Name}";
        return $"{LongName}: ({string.Join(", ", Children.Select(x => x is ASTNode k ? k.LongNamePrint(LongName) : ((IToken)x).Lexeme))})";
    }
    private ASTNode(IValidASTLeaf[] children, string Name) : this(
    children.Select(x => x.Type).ToArray(), children, Name)
    {
    }
    public static ASTNode Binary(ASTNode Left, IToken Mid, ASTNode Right, string Name) => new ASTNode([Left, Mid, Right], Name);
    public static ASTNode PrimedBinary(ASTNode Left, ASTNode Prime, string Name) => new ASTNode([Left, Prime], Name);
    public static ASTNode BinaryPrime(IToken Operator, ASTNode Right, ASTNode Repeat, string Name) => new([Operator, Right, Repeat], Name);
    public static ASTNode Repeating(ASTNode Subject, ASTNode Repeat, string Name) => new ASTNode([Subject, Repeat], Name);
    public static ASTNode NonTerminal(ASTNode NT, string Name) => new ASTNode([NT], Name);
    public static ASTNode Terminal(IToken T, string Name) => new ASTNode([T], Name);
    public static ASTNode Unary(IToken Operator, ASTNode Operand, string Name) => new ASTNode([Operator, Operand], Name);
    public static ASTNode Empty() => new ASTNode([], "Empty");
    public static ASTNode Parenthesized(IToken Open, ASTNode Center, IToken Close, string Name)
    {
        if (Open.TT != TokenType.OpenParen || Close.TT != TokenType.CloseParen)
        {
            throw new ArgumentException("Open and close parentheses must match and must be parentheses.");
        }
        return new ASTNode([Open, Center, Close], Name);
    }
}