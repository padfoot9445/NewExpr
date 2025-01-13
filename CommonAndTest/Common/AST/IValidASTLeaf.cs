namespace Common.AST;
public interface IValidASTLeaf : IIsEquivalentTo<IValidASTLeaf>
{
    public ASTLeafType Type { get; }
    public ICollection<IValidASTLeaf> Flatten();
}