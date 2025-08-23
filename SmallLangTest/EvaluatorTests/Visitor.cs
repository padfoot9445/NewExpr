using System.Diagnostics;
using System.Linq.Expressions;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
using SmallLang;
using Node = Common.AST.DynamicASTNode<SmallLang.ImportantASTNodeType, SmallLangTest.EvaluatorTests.Attributes>;
namespace SmallLangTest.EvaluatorTests;
//Primary: True -> IsLiteral, PARSE -> Value, Parent.Value -> RootValue
//BinaryExperssion: False -> IsLiteral, EVAL(Operator, Op1, Op2) -> Value, parent.Value -> RootValue
class Visitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{

    bool Primary(Node? parent, Node node)
    {
        Debug.Assert(node.Data!.TT == TokenType.Number);
        double Value = double.Parse(node.Data!.Lexeme);
        double? RootVal = parent is not null ? parent.RootValue : Value;
        var oldAttr = node.Attributes;
        node.Attributes = node.Attributes with { RootValue = RootVal, Value = Value, IsLiteral = true };
        return oldAttr.Equals(node.Attributes) is false;
    }
    bool BinaryExpression(Node? parent, Node node)
    {
        Debug.Assert(node.Data!.TT == TokenType.Addition || node.Data!.TT == TokenType.Multiplication);
        double? Value = null;
        switch (node.Data!.TT)
        {
            case TokenType.Addition:
                if (node.Children[0].Value is not null && node.Children[1].Value is not null)
                {
                    Value = node.Children[0].Value + node.Children[1].Value;
                }
                break;
            case TokenType.Multiplication:
                if (node.Children[0].Value is not null && node.Children[1].Value is not null)
                {
                    Value = node.Children[0].Value * node.Children[1].Value;
                }
                break;
            default: throw new Exception();
        }
        double? RootVal = parent is not null ? parent.RootValue : Value;
        var oldAttr = node.Attributes;
        node.Attributes = node.Attributes with { RootValue = RootVal, Value = Value, IsLiteral = false };
        return oldAttr.Equals(node.Attributes) is false;
    }
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        switch (node.NodeType)
        {
            case ImportantASTNodeType.Primary: return Primary;
            case ImportantASTNodeType.BinaryExpression: return BinaryExpression;
            default: throw new Exception($"nodetype {node.NodeType} out of scope for test");
        }
    }
}
