using Common.AST;

namespace SmallLang.Evaluator;
static class DynamicASTWalker
{
    public static DynamicASTNode<T1, T2> Walk<T1, T2>(this DynamicASTNode<T1, T2> node, Action<DynamicASTNode<T1, T2>?, DynamicASTNode<T1, T2>> action) where T2 : IMetadata
    {
        return Walk(node, action);
    }
    static DynamicASTNode<T1, T2> Walk<T1, T2>(DynamicASTNode<T1, T2> node, Action<DynamicASTNode<T1, T2>?, DynamicASTNode<T1, T2>> action, DynamicASTNode<T1, T2>? parent = null) where T2 : IMetadata
    {
        action(parent, node);
        foreach (var child in node.Children)
        {
            Walk(child, action, node);
        }
        return node;
    }
    public static DynamicASTNode<T1, T2> Walk<T1, T2>(this DynamicASTNode<T1, T2> node, IDynamicASTVisitor visitor) where T2 : IMetadata
    {
        return Walk(node, visitor.Dispatch(node));
    }
}