using Common.AST;

namespace Common.Evaluator;

public static class DynamicASTWalker
{
    public static void Walk<T1, T2>(this DynamicASTNode<T1, T2> node, IDynamicASTVisitor<T1, T2> visitor,
        out bool Changed) where T2 : IMetadata, new()
    {
        Walk(node, visitor, null, out Changed);
    }

    private static void Walk<T1, T2>(DynamicASTNode<T1, T2> node, IDynamicASTVisitor<T1, T2> visitor,
        DynamicASTNode<T1, T2>? parent, out bool Changed) where T2 : IMetadata, new()
    {
        Changed = visitor.Dispatch(node)(parent, node);
        bool ChildChanged;
        foreach (var child in node.Children)
        {
            Walk(child, visitor, node, out ChildChanged);
            Changed = ChildChanged || Changed;
        }
    }
}