using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.AttributeVisitors;

using SmallFunctionSignature = Common.Metadata.FunctionSignature<BackingNumberType, SmallLangType>;

public static partial class AttributeVisitor
{
    private static readonly IEnumerable<Action<ISmallLangNode>> EvaluationPasses =
    [
        TryEvaluate<UnaryExpressionNode>,
        TryEvaluate<FunctionCallNode>,
        TryEvaluate<IdentifierNode>,
        TryEvaluate<FunctionNode>,
        TryEvaluate<ReTypeOriginalNode>
    ];
    public static void BuildAttributes(this ISmallLangNode node)
    {
        int HashBefore = unchecked(node.GetHashCode() + 1);
        while (HashBefore != node.GetHashCode())
        {
            foreach (var pass in EvaluationPasses)
            {
                pass(node);
            }

            foreach (var childnode in node.ChildNodes)
            {
                childnode.BuildAttributes();
            }
        }
    }

    private static void TryEvaluate<T>(ISmallLangNode node) where T : ISmallLangNode
    {
        if (node is T TNode)
        {
            Evaluate((dynamic)TNode);
        }
    }
    private static TReturn? GetIfNotNullRefReturn<TO1, TO2, TReturn>(TO1 Src, TO2? Key, Func<TO1, TO2, TReturn> Accessor)
    where TReturn : class
    {
        if (Key is not null) return Accessor(Src, Key);
        return null;
    }
    private static TReturn? GetIfNotNullValueReturn<TO1, TO2, TReturn>(TO1 Src, TO2? Key, Func<TO1, TO2, TReturn> Accessor)
    where TReturn : struct
    {
        if (Key is not null) return Accessor(Src, Key);
        return null;
    }
    private static void DoIfNotNull(Action action, params object?[] args)
    {
        if (args.All(x => x is not null)) { action(); }
    }
    private static Dictionary<VariableName, SmallFunctionSignature> FunctionSignatures { get; } = new();
}