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

    private static Dictionary<VariableName, SmallFunctionSignature> FunctionSignatures { get; } = new();
}