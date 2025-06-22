using System.Security.Cryptography.X509Certificates;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Constants;
namespace SmallLang.Metadata;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public class AttributeVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
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
            ImportantASTNodeType.NewExpr => (x, y) => false,
            ImportantASTNodeType.GenericType => (x, y) => false,
            ImportantASTNodeType.TypeCSV => (x, y) => false,
            ImportantASTNodeType.BaseType => (x, y) => false,
            _ => throw new Exception(node.NodeType.ToString())
        };
    }
    bool Changed(Attributes oldattr, Attributes newattr) => (oldattr == newattr) is false;
    private bool FunctionCall(Node? parent, Node self)
    {
        FunctionID ID = Functions.Mapper.FunctionNameToFunctionID[self.Children[0].Data!.Lexeme];
        SmallLangType RetType = Functions.Mapper.FunctionToRetType[ID];
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { FunctionID = ID, DeclArgumentTypes = Functions.Mapper.FunctionToFunctionArgs[ID], TypeOfExpression = RetType };
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
                TokenType.String => self.Data.Literal.Length > 3 ? TypeData.Data.StringTypeCode : TypeData.Data.CharTypeCode,
                TokenType.TrueLiteral => TypeData.Data.BooleanTypeCode,
                TokenType.FalseLiteral => TypeData.Data.BooleanTypeCode,
                TokenType.Number => self.Data.Literal.Contains('.') ? TypeData.Data.FloatTypeCode : TypeData.Data.IntTypeCode,
                _ => throw new Exception($"Unknown primary type {self.Data.TT}"),
            }

            };
        }
        return Changed(oldattr, self.Attributes);
    }
}
