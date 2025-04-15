using Common.Tokens;

namespace Common.AST;
public record class DynamicASTNode<TNodeType, TAnnotationContainer>(IToken? Data, List<DynamicASTNode<TNodeType, TAnnotationContainer>> Children, TNodeType NodeType) where TAnnotationContainer : IMetadata, new()
{
    public TAnnotationContainer Attributes { get; set; } = new();
    public override int GetHashCode()
    {
        int CSUM = 0;
        foreach (var child in Children)
        {
            CSUM += child.GetHashCode();
        }
        return (Data?.GetHashCode() ?? int.MinValue + 1974) + CSUM + (NodeType?.GetHashCode() ?? int.MinValue + 3523) + Attributes.GetHashCode();
    }
    public virtual bool Equals(DynamicASTNode<TNodeType, TAnnotationContainer>? obj)
    {
        if (!(obj is DynamicASTNode<TNodeType, TAnnotationContainer> node)) return false;
        return Data == node.Data && Children.Select((x, i) => Children[i].Equals(x)).All(x => x is true) && NodeType!.Equals(node.NodeType) && Attributes.Equals(node.Attributes);
    }
}