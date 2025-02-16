using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class TypeParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (SafeParse(Data.BaseType, out Node))
        {
            return true;
        }
        else if (SafeParse(Data.GenericType, out var GenTypeNode) && Data.TryConsumeToken(TokenType.OpenAngleSquare) && SafeParse(this, out var NestedType) && Data.TryConsumeToken(TokenType.CloseAngleSquare))
        {
            Node = GenTypeNode! with { NodeType = ASTNodeType.Type, Children = [NestedType] };
            //Dict<[List<[Key, Val]>]>
            return true;
        }
        Node = null;
        return false;
    }
}