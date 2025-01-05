using Common.Tokens;

namespace Common.AST;
public class AnnotatedNode<AnnotationContainer> : ASTNode where AnnotationContainer : IMetadata, new()
{
    private AnnotatedNode(AnnotationContainer Attributes, IEnumerable<ASTLeafType> pattern, IEnumerable<IValidASTLeaf> children, string name) : base(pattern, children, name)
    {
        this.Attributes = Attributes;
    }
    private AnnotatedNode(AnnotationContainer Attributes, ASTNode node) : base(node.Pattern, node.Children, node.Name)
    {
        this.Attributes = Attributes;
    }
    public static AnnotatedNode<AnnotationContainer> FromNodeRecursive(AnnotationContainer? attributes, IValidASTLeaf leaf)
    {
        attributes ??= new();
        if (leaf is AnnotatedNode<AnnotationContainer> AnnoNode)
        {
            attributes.Merge(AnnoNode.Attributes); //we know that this call is safe since both types must be AnnotationContainer
            return new AnnotatedNode<AnnotationContainer>(attributes, AnnoNode);
        }
        else if (leaf is ASTNode node)
        {
            return new AnnotatedNode<AnnotationContainer>(
                Attributes: attributes,
                pattern: node.Pattern.Select(x => ASTLeafType.AnnotatedNode).ToArray(), //convert pattern to all annotated
                children: node.Children.Select(x => FromNodeRecursive(new AnnotationContainer(), x)), //convert all children to annotated nodes with blank annotations
                name: node.Name
                );
        }
        else if (leaf is IToken token)
        {
            return new AnnotatedNode<AnnotationContainer>(attributes, Terminal(token, "token-to-annotated-automatic-conversion")); //future: do not recurse or this will be infinite
        }
        throw new Exception($"Unexpected type of leaf {leaf.GetType().Name}");
    }
    public AnnotationContainer Attributes { get; init; }
    public override ASTLeafType Type => ASTLeafType.AnnotatedNode;
    public override bool IsEquivalentTo(IValidASTLeaf other, bool __cinv = true)
    {
        return
            other is AnnotatedNode<AnnotationContainer> Annonode &&
            Attributes.IsEquivalentTo(Annonode.Attributes) &&
            base.IsEquivalentTo(other, __cinv)
        ;
    }
}