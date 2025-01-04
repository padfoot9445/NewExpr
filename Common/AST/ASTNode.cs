using Common.Tokens;

namespace Common.AST;
public class ASTNode<T> : IValidASTLeaf where T : IMetadata
{
    public ASTLeafType Type => ASTLeafType.NonTerminal;
    public ASTLeafType[] Pattern { get; init; }
    public IValidASTLeaf[] Children { get; init; }
    public string Name { get; init; }
    public T? Attributes { get; private set; } = default;
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
        return $"{Name}: {Attributes}, ({string.Join(", ", Children.Select(x => x is ASTNode<T> k ? k.Print() : ((IToken)x).Lexeme))})";
    }
    public string LongNamePrint(string CallerLongName = "")
    {
        string LongName = $"{CallerLongName}-{Name}";
        return $"{LongName}: ({string.Join(", ", Children.Select(x => x is ASTNode<T> k ? k.LongNamePrint(LongName) : ((IToken)x).Lexeme))})";
    }
    private ASTNode(IValidASTLeaf[] children, string Name) : this(
    children.Select(x => x.Type).ToArray(), children, Name)
    {
    }
    public static ASTNode<T> Binary(ASTNode<T> Left, IToken Mid, ASTNode<T> Right, string Name) => new ASTNode<T>([Left, Mid, Right], Name);
    public static ASTNode<T> PrimedBinary(ASTNode<T> Left, ASTNode<T> Prime, string Name) => new ASTNode<T>([Left, Prime], Name);
    public static ASTNode<T> BinaryPrime(IToken Operator, ASTNode<T> Right, ASTNode<T> Repeat, string Name) => new([Operator, Right, Repeat], Name);
    public static ASTNode<T> Repeating(ASTNode<T> Subject, ASTNode<T> Repeat, string Name) => new ASTNode<T>([Subject, Repeat], Name);
    public static ASTNode<T> NonTerminal(ASTNode<T> NT, string Name) => new ASTNode<T>([NT], Name);
    public static ASTNode<T> Terminal(IToken T, string Name) => new ASTNode<T>([T], Name);
    public static ASTNode<T> Unary(IToken Operator, ASTNode<T> Operand, string Name) => new ASTNode<T>([Operator, Operand], Name);
    public static ASTNode<T> Empty() => new ASTNode<T>([], "Empty");
    public static ASTNode<T> Parenthesized(IToken Open, ASTNode<T> Center, IToken Close, string Name)
    {
        if (Open.TT != TokenType.OpenParen || Close.TT != TokenType.CloseParen)
        {
            throw new ArgumentException("Open and close parentheses must match and must be parentheses.");
        }
        return new ASTNode<T>([Open, Center, Close], Name);
    }
}