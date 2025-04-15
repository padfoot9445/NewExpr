using System.Linq.Expressions;
using Common.AST;
using SmallLang;
using SmallLang.Evaluator;

namespace SmallLangTest.EvaluatorTests;
//Primary: True -> IsLiteral, PARSE -> Value, Parent.Value -> RootValue
//BinaryExperssion: False -> IsLiteral, EVAL(Operator, Op1, Op2) -> Value, parent.Value -> RootValue
class Visitor : IDynamicASTVisitor
{
    void Primary<T1, T2>(DynamicASTNode<T1, T2>? parent, DynamicASTNode<T1, T2> node) where T2 : IMetadata, new()
    {

    }
    void BinaryExpression<T1, T2>(DynamicASTNode<T1, T2>? parent, DynamicASTNode<T1, T2> node) where T2 : IMetadata, new() { }
    public Action<DynamicASTNode<T1, T2>?, DynamicASTNode<T1, T2>> Dispatch<T1, T2>(DynamicASTNode<T1, T2> node) where T2 : IMetadata, new()
    {
        switch (node.NodeType)
        {
            case ImportantASTNodeType.Primary: return Primary;
            case ImportantASTNodeType.BinaryExpression: return BinaryExpression;
            default: throw new Exception($"nodetype {node.NodeType} out of scope for test");
        }
    }
}
