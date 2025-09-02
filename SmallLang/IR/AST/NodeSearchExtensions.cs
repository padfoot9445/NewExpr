using Common.AST;

namespace SmallLang.IR.AST;

public static class NodeSearchExtensions
{
    public static ISmallLangNode? GetParent(this ISmallLangNode self, ISmallLangNode Root)
    {
        if (ReferenceEquals(self, Root)) return null;

        var ParentNodes = Root.Flatten().Where(x => x.ChildNodes.Any(x => ReferenceEquals(x, self)));

        if (ParentNodes.Count() > 1)
        {
            throw new NotSupportedException("GetParent does not currently support nodes with multiple parents");
        }
        else if (!ParentNodes.Any()) //ParentNodes.Count() == 0
        {
            throw new InvalidOperationException("Invalid Operation: self was not the root of the provided tree, but no parent was found. This implies self is not part of the provided tree.");
        }
        else
        {
            return ParentNodes.Single();
        }
    }

    public static ISmallLangNode RecursiveGetParent(this ISmallLangNode self, ISmallLangNode Root, Func<ISmallLangNode, bool> Predicate)
    {

        ISmallLangNode NodeUnderConsideration = self.GetParent(Root) ?? throw new ArgumentException("Self was equal to Root. This cannot be the case if you want to get information from a parent.");

        while (!Predicate(NodeUnderConsideration))
        {
            NodeUnderConsideration = NodeUnderConsideration.GetParent(Root) ?? throw new ArgumentException("Reached Root before predicate was satisfied.");
        }
        return NodeUnderConsideration;
    }
}