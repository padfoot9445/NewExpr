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
}