using Common.AST;
using Common.Evaluator;
namespace Common.Backend;

public class CodeGenerator
{
    public static TReturn Dispatch<TN, TAC, TReturn>
    (
        DynamicASTNode<TN, TAC> node,
        params
        (
            TN,
            Func<DynamicASTNode<TN, TAC>, TReturn>
        )[] Cases
    )
    where TAC : IMetadata, new()
    {
        foreach ((var type, var Function) in Cases)
        {
            if (node.NodeType!.Equals(type)) return Function(node);
        }
        throw new Exception($"Ran out of cases when switching on {node.NodeType}");
    }
    public static void Dispatch<TN, TA>
    (
        DynamicASTNode<TN, TA> node,
        params
        (
            TN,
            Action<DynamicASTNode<TN, TA>>
        )[] Cases
    )
    where TA : IMetadata, new()
    {
        Dispatch(node, Cases.Select<(TN, Action<DynamicASTNode<TN, TA>>), (TN, Func<DynamicASTNode<TN, TA>, bool>)>(x => (x.Item1, y => { x.Item2(y); return true; })).ToArray());
        //Don't question it too hard
        //this takes the Action<Node> of the cases, turns them into Func<Node, bool>, and then calls Dispatch with return-type bool and then discards the return
    }
}