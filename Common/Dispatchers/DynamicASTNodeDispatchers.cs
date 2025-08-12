using Common.AST;
using Common.Evaluator;
using sly.i18n;
namespace Common.Dispatchers;

public static class DynamicASTNodeDispatchers
{
    public static TReturn DispatchNodeType<TN, TAC, TReturn>
    (
        this DynamicASTNode<TN, TAC> node,
        params
        (
            TN,
            Func<DynamicASTNode<TN, TAC>, TReturn>
        )[] Cases
    )
    where TAC : IMetadata, new()
    {

        return node.DispatchGeneric(Cases.Select
        <
            (TN, Func<DynamicASTNode<TN, TAC>, TReturn>),
            (Func<DynamicASTNode<TN, TAC>, bool>, Func<DynamicASTNode<TN, TAC>, TReturn>)
        >
        (x => (y => y.NodeType!.Equals(x.Item1), x.Item2)).ToArray());
    }
    public static void DispatchNodeType<TN, TA>
    (
        this DynamicASTNode<TN, TA> node,
        params
        (
            TN,
            Action<DynamicASTNode<TN, TA>>
        )[] Cases
    )
    where TA : IMetadata, new()
    {
        node.DispatchNodeType(Cases.Select<(TN, Action<DynamicASTNode<TN, TA>>), (TN, Func<DynamicASTNode<TN, TA>, bool>)>(x => (x.Item1, y => { x.Item2(y); return true; })).ToArray());
        //Don't question it too hard
        //this takes the Action<Node> of the cases, turns them into Func<Node, bool>, and then calls Dispatch with return-type bool and then discards the return
    }

    public static void DispatchGeneric<TN, TA>(this DynamicASTNode<TN, TA> node,
    params
    (
        Func<DynamicASTNode<TN, TA>, bool>,
        Action<DynamicASTNode<TN, TA>>
    )[] Cases)

    where TA : IMetadata, new()
    {
        node.DispatchGeneric(Cases.Select<(Func<DynamicASTNode<TN, TA>, bool>, Action<DynamicASTNode<TN, TA>>), (Func<DynamicASTNode<TN, TA>, bool>, Func<DynamicASTNode<TN, TA>, bool>)>(x => (x.Item1, y => { x.Item2(y); return true; })).ToArray());
        //Don't question it too hard
        //this takes the Action<Node> of the cases, turns them into Func<Node, bool>, and then calls Dispatch with return-type bool and then discards the return
    }
    public static TReturnType DispatchGeneric<TN, TA, TReturnType>(this DynamicASTNode<TN, TA> node,
    params
    (
        Func<DynamicASTNode<TN, TA>, bool>,
        Func<DynamicASTNode<TN, TA>, TReturnType>
    )[] Cases)

    where TA : IMetadata, new()
    {
        foreach ((var cond, var func) in Cases)
        {
            if (cond(node)) return func(node);
        }
        throw new Exception("DispatchGeneric ran out of conditions");
    }
}