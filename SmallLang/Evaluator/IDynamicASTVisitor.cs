using Common.AST;

namespace SmallLang.Evaluator;
public interface IDynamicASTVisitor
{
    public Action<DynamicASTNode<T1, T2>?, DynamicASTNode<T1, T2>> Dispatch<T1, T2>(DynamicASTNode<T1, T2> node) where T2 : IMetadata;
}