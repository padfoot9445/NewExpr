using Common.Tokens;

namespace Common.AST;
public record class DynamicASTNode<TNodeType, TAnnotationContainer>(IToken? Data, List<DynamicASTNode<TNodeType, TAnnotationContainer>> Children, TNodeType NodeType) where TAnnotationContainer : IMetadata
{

}