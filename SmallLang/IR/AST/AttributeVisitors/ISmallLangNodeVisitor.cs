namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(ISmallLangNode node)
    {
        throw new Exception($"Unhandled node of type {node.GetType()}");
    }
}