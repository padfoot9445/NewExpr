using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.AttributeVisitors;

using SmallFunctionSignature = Common.Metadata.FunctionSignature<BackingNumberType, SmallLangType>;

public static partial class AttributeVisitor
{
    private static readonly IEnumerable<Func<ISmallLangNode, bool>> EvaluationPasses =
    [
        TryEvaluate<ISmallLangNode>
    ];
    public static void BuildAttributes(this ISmallLangNode node)
    {
        while (true)
        {
            if (EvaluationPasses.Select(x => x(node)).All(x => x)) break;
        }
    }

    private static bool TryEvaluate<T>(ISmallLangNode node) where T : ISmallLangNode
    {
        if (node is T TNode)
        {
            int HashBefore = TNode.GetHashCode();
            Evaluate((dynamic)TNode);

            return HashBefore == TNode.GetHashCode();
        }
        return true;
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
    private static Dictionary<VariableName, SmallFunctionSignature> FunctionSignatures { get; } = new();
}