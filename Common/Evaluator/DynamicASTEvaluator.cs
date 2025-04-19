using Common.AST;

namespace Common.Evaluator;
public class DynamicASTEvaluator
{
    public void Evaluate<T, A>(DynamicASTNode<T, A> node, IDynamicASTVisitor<T, A> visitor) where A : IMetadata, new()
    {
        bool Changed;
        do
        {
            node.Walk(visitor, out Changed);
        }
        while (Changed);
    }
}