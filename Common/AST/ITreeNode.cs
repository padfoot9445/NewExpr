namespace Common.AST;

public interface ITreeNode<T> where T : ITreeNode<T>
{
    IEnumerable<T> ChildNodes { get; }
}