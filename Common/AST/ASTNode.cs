using System.Collections.ObjectModel;
using Common.Tokens;

namespace Common.AST;
public class ASTNode : IValidASTLeaf
{
    public virtual ASTLeafType Type => ASTLeafType.NonTerminal;
    public ASTLeafType[] Pattern { get; init; }
    public IValidASTLeaf[] Children { get; init; }
    public string Name { get; init; }
    public ASTNode(IEnumerable<ASTLeafType> pattern, IEnumerable<IValidASTLeaf> children, string name)
    {
        Pattern = pattern.ToArray();
        Children = children.ToArray();
        for (int i = 0; i < Children.Length; i++)
        {
            if (Children[i].Type != Pattern[i] && !(Children[i].Type == ASTLeafType.AnnotatedNode && Pattern[i] == ASTLeafType.NonTerminal))
            {
                throw new ArgumentException("Children must match pattern");
            }
        }
        Name = name;
    }

    ICollection<IValidASTLeaf> IValidASTLeaf.Flatten() => Flatten();
    /// <summary>
    /// Checks if all properties are equal. __cinv controls whether or not to also check inverse equality; this is advisable as you will usually wish to return false if, say, other were to be an annotated node and this was not.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="__cinv"></param>
    /// <returns></returns>
    bool IIsEquivalentTo<IValidASTLeaf>.IsEquivalentTo(IValidASTLeaf other) => IsEquivalentTo(other, true);
    public virtual bool IsEquivalentTo(IValidASTLeaf other, bool __cinv = true)
    {
        if (other is not ASTNode node)
        { return false; }

        return
            Pattern.SequenceEqual(node.Pattern) &&
            Children.Length == node.Children.Length &&
            Children.Select((x, ind) => node.Children[ind].IsEquivalentTo(x)).All(x => x) &&
            Name == node.Name &&
            (__cinv ? node.IsEquivalentTo(this, false) : true)
        ;
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
    public Collection<IValidASTLeaf> Flatten()
    {
        Collection<IValidASTLeaf> rv = [this];
        foreach (IValidASTLeaf child in Children)
        {
            if (child is ASTNode Node)
            {
                rv = [.. rv.Union(Node.Flatten())];
            }
            else if (child is IToken Token)
            {
                rv.Add(child);
            }
            else
            {
                throw new Exception("Unexpected Type");
            }
        }
        return rv;
    }
    private ASTNode(IValidASTLeaf[] children, string Name) : this(
    children.Select(x => x.Type).ToArray(), children, Name)
    {
    }
    public static ASTNode Binary(ASTNode Left, IToken Mid, ASTNode Right, string Name) => new ASTNode([Left, Mid, Right], Name);
    public static ASTNode PrimedBinary(ASTNode Left, ASTNode Prime, string Name) => new ASTNode([Left, Prime], Name);
    public static ASTNode BinaryPrime(IToken Operator, ASTNode Right, ASTNode Repeat, string Name) => new([Operator, Right, Repeat], Name);
    public static ASTNode Repeating(ASTNode Subject, IToken Delimiter, ASTNode Repeat, string Name) => new ASTNode([Subject, Delimiter, Repeat], Name);
    public static ASTNode NonTerminal(ASTNode NT, string Name) => new ASTNode([NT], Name);
    public static ASTNode Terminal(IToken T, string Name) => new ASTNode([T], Name);
    public static ASTNode Unary(IToken Operator, ASTNode Operand, string Name) => new ASTNode([Operator, Operand], Name);
    public static ASTNode Empty(string Name = "Empty") => new ASTNode([], Name);
    public static ASTNode Parenthesized(IToken Open, ASTNode Center, IToken Close, string Name)
    {
        if (Open.TT != TokenType.OpenParen || Close.TT != TokenType.CloseParen)
        {
            throw new ArgumentException("Open and close parentheses must match and must be parentheses.");
        }
        return new ASTNode([Open, Center, Close], Name);
    }
}