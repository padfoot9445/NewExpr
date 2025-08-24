namespace Common.AST;

public static class TreeNodeExtensions
{
    public static IEnumerable<T> Flatten<T>(this ITreeNode<T> self)
    where T : ITreeNode<T>
    {
        return [(T)self, .. self.ChildNodes.SelectMany(x => x.Flatten())];
    }
}