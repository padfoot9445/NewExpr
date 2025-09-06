namespace Common.AST;

public static class TreeNodeExtensions
{
    public static IEnumerable<T> Flatten<T>(this ITreeNode<T> self)
        where T : ITreeNode<T>
    {
        return [(T)self, .. self.ChildNodes.SelectMany(x => x.Flatten())];
    }

    public static T? GetParent<T>(this T self, T Root)
        where T : ITreeNode<T>
    {
        if (ReferenceEquals(self, Root)) return default;

        var ParentNodes = Root.Flatten().Where(x => x.ChildNodes.Any(x => ReferenceEquals(x, self)));

        if (ParentNodes.Count() > 1)
            throw new NotSupportedException("GetParent does not currently support nodes with multiple parents");

        if (!ParentNodes.Any()) //ParentNodes.Count() == 0
            throw new InvalidOperationException(
                "Invalid Operation: self was not the root of the provided tree, but no parent was found. This implies self is not part of the provided tree.");

        return ParentNodes.Single();
    }

    public static T RecursiveGetParent<T>(this T self, T Root, Func<T, bool> Predicate)
        where T : ITreeNode<T>
    {
        var NodeUnderConsideration = self.GetParent(Root) ??
                                     throw new ArgumentException(
                                         "Self was equal to Root. This cannot be the case if you want to get information from a parent.");

        while (!Predicate(NodeUnderConsideration))
            NodeUnderConsideration = NodeUnderConsideration.GetParent(Root) ??
                                     throw new ArgumentException("Reached Root before predicate was satisfied.");
        return NodeUnderConsideration;
    }
}