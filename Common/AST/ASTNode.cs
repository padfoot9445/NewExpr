using Common.Tokens;

namespace Common.AST;
public class ASTNode : IValidASTLeaf
{
    public ASTLeafType Type => ASTLeafType.NonTerminal;
    public ASTLeafType[] Pattern { get; init; }
    public IValidASTLeaf[] Children { get; init; }
    public ASTNode(ASTLeafType[] pattern, IValidASTLeaf[] children)
    {
        Pattern = pattern;
        Children = children;
    }
    public ASTNode(IValidASTLeaf[] children)
    {
        Children = children;
        Pattern = children.Select(x => x.Type).ToArray();
    }
    public static ASTNode Binary(ASTNode Left, IToken Mid, ASTNode Right)
    {
        return new ASTNode(
            [Left, Mid, Right]
        );
    }
    public static ASTNode PrimedBinary(ASTNode Left, ASTNode Prime) => new ASTNode([Left, Prime]);
    public static ASTNode BinaryPrime(IToken Operator, ASTNode Right, ASTNode Repeat) => new([Operator, Right, Repeat]);
}