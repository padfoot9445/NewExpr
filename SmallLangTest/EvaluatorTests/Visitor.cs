using System.Diagnostics;
using System.Linq.Expressions;
using Common.AST;
using Common.Tokens;
using SmallLang;
using SmallLang.Evaluator;
using Node = Common.AST.DynamicASTNode<SmallLang.ImportantASTNodeType, SmallLangTest.EvaluatorTests.Attributes>;
namespace SmallLangTest.EvaluatorTests;
//Primary: True -> IsLiteral, PARSE -> Value, Parent.Value -> RootValue
//BinaryExperssion: False -> IsLiteral, EVAL(Operator, Op1, Op2) -> Value, parent.Value -> RootValue
class Visitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    void Primary(Node? parent, Node node)
    {
        Debug.Assert(node.Data!.TT == TokenType.Number);
        double Value = double.Parse(node.Data!.Lexeme);
        double? RootVal = parent?.Attributes.Value ?? Value;
        node.Attributes = node.Attributes with { RootValue = RootVal, Value = Value, IsLiteral = true };
    }
    void BinaryExpression(Node? parent, Node node)
    {
        Debug.Assert(node.Data!.TT == TokenType.Addition || node.Data!.TT == TokenType.Multiplication);
        double? Value = null;
        switch (node.Data!.TT)
        {
            case TokenType.Addition:
                if (node.Children[0].Attributes.Value is not null && node.Children[1].Attributes.Value is not null)
                {
                    Value = node.Children[0].Attributes.Value + node.Children[1].Attributes.Value;
                }
                break;
            case TokenType.Multiplication:
                if (node.Children[0].Attributes.Value is not null && node.Children[1].Attributes.Value is not null)
                {
                    Value = node.Children[0].Attributes.Value * node.Children[1].Attributes.Value;
                }
                break;
            default: throw new Exception();
        }
        double? RootVal = parent?.Attributes.Value ?? Value;
        node.Attributes = node.Attributes with { RootValue = RootVal, Value = Value, IsLiteral = false };
    }
    public Action<Node?, Node> Dispatch(Node node)
    {
        switch (node.NodeType)
        {
            case ImportantASTNodeType.Primary: return Primary;
            case ImportantASTNodeType.BinaryExpression: return BinaryExpression;
            default: throw new Exception($"nodetype {node.NodeType} out of scope for test");
        }
    }
}
