namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    public static void BuildAttributes(this SmallLangNode node)
    {

    }

    public static void TryEvaluate<T>(SmallLangNode node)
    {
        if (node is T TNode)
        {
            Evaluate()
        }
    }
}