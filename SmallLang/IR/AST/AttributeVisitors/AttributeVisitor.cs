namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    public static void BuildAttributes(this ISmallLangNode node)
    {

    }

    public static bool TryEvaluate<T>(ISmallLangNode node) where T : ISmallLangNode
    {
        if (node is T TNode)
        {
            int HashBefore = TNode.GetHashCode();
            Evaluate(TNode);

            return HashBefore == TNode.GetHashCode();
        }
        return true;
    }
}