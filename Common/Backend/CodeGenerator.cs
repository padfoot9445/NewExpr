using Common.AST;
using Common.Evaluator;

namespace Common.Backend;
public class CodeGenerator
{
    public void Evaluate<T, A>(DynamicASTNode<T, A> node, IDynamicASTVisitor<T, A> visitor) where A : IMetadata, new()
    {
        throw new NotImplementedException();
        node.Walk(visitor, out bool _);
    }
}