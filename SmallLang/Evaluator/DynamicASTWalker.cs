using Common.AST;

namespace SmallLang.Evaluator;
static class DynamicASTWalker
{
    public static void Walk<T1, T2>(this DynamicASTNode<T1, T2> node, IDynamicASTVisitor<T1, T2> visitor) where T2 : IMetadata, new()
    {
        Walk(node, visitor, null);
    }
    static void Walk<T1, T2>(DynamicASTNode<T1, T2> node, IDynamicASTVisitor<T1, T2> visitor, DynamicASTNode<T1, T2>? parent = null) where T2 : IMetadata, new()
    {
        visitor.Dispatch(node)(parent, node);
        foreach (var child in node.Children)
        {
            Walk(child, visitor, node);
        }
    }
}