using Common.AST;

namespace SmallLang.Evaluator;
public interface IDynamicASTVisitor<T1, T2> where T2 : IMetadata, new()
{
    public Action<DynamicASTNode<T1, T2>?, DynamicASTNode<T1, T2>> Dispatch(DynamicASTNode<T1, T2> node);
}