namespace Common.AST;

public interface ITreeNode<T>
{
    IEnumerable<T> ChildNodes { get; }
}