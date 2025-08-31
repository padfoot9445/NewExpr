namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(SmallLangNode node)
    {
        foreach (SmallLangNode ChildNode in node.ChildNodes.Cast<SmallLangNode>())
        {
            ChildNode.Scope ??= node.Scope;
        }
    }
}