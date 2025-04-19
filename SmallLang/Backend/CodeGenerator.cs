using Common.AST;
using SmallLang.Evaluator;

namespace SmallLang.Backend;
public class CodeGenerator
{
    public void Evaluate<T, A>(DynamicASTNode<T, A> node, IDynamicASTVisitor<T, A> visitor) where A : IMetadata, new()
    {
        node.Walk(visitor, out bool _);
    }
}