using Common.AST;

namespace SmallLang.Evaluator;
public class DynamicASTEvaluator
{
    public void Evaluate<T, A>(DynamicASTNode<T, A> node, IDynamicASTVisitor visitor) where A : IMetadata
    {
        int Hash1 = node.GetHashCode();
        int Hash2;
        do
        {
            node.Walk(visitor);
            Hash2 = Hash1;
            Hash1 = node.GetHashCode();
        }
        while (node.GetHashCode() != Hash2);
    }
}