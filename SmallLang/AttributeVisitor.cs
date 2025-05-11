using System.Security.Cryptography.X509Certificates;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
using SmallLang.Backend.CodeGenComponents;
namespace SmallLang;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class AttributeVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    Dictionary<uint, List<uint>> FunctionToFunctionArgs = new()
    {
        [1] = []
    };
    Dictionary<string, uint> FunctionNameToFunctionID = new()
    {
        ["input"] = 1
    };
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        return node.NodeType switch
        {
            ImportantASTNodeType.FunctionCall => FunctionCall,
            ImportantASTNodeType.Section => (x, y) => false,
            ImportantASTNodeType.Identifier => (x, y) => false,
            ImportantASTNodeType.Primary => Primary,
            ImportantASTNodeType.ArgList => (x, y) => false,
            ImportantASTNodeType.ArgListElement => (x, y) => false,
            _ => throw new Exception()
        };
    }
    bool Changed(Attributes oldattr, Attributes newattr) => (oldattr == newattr) is false;
    private bool FunctionCall(Node? parent, Node self)
    {
        uint ID = FunctionNameToFunctionID[self.Children[0].Data!.Lexeme];
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { FunctionID = ID, DeclArgumentTypes = FunctionToFunctionArgs[ID] };
        return Changed(oldattr, self.Attributes);
    }
    private bool Primary(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        if (self.Attributes.TypeOfExpression is null)
        {
            self.Attributes = self.Attributes with
            {
                TypeOfExpression =

            self.Data!.TT switch
            {
                TokenType.String => self.Data.Literal.Length > 3 ? BaseCodeGenComponent.StringTypeCode : BaseCodeGenComponent.CharTypeCode,
                TokenType.TrueLiteral => BaseCodeGenComponent.BooleanTypeCode,
                TokenType.FalseLiteral => BaseCodeGenComponent.BooleanTypeCode,
                TokenType.Number => self.Data.Literal.Contains('.') ? BaseCodeGenComponent.FloatTypeCode : BaseCodeGenComponent.IntTypeCode,
                _ => throw new Exception($"Unknown primary type {self.Data.TT}"),
            }

            };
        }
        return Changed(oldattr, self.Attributes);
    }
}
