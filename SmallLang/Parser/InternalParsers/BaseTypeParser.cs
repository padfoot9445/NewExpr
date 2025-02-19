using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class BaseTypeParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (Data.TryConsumeToken(out var TypeToken, TokenType.TypeByte, TokenType.TypeShort, TokenType.TypeInt, TokenType.TypeLong, TokenType.TypeLongInt, TokenType.TypeFloat, TokenType.TypeDouble, TokenType.TypeRational, TokenType.TypeNumber, TokenType.TypeString, TokenType.TypeChar, TokenType.TypeVoid))
        {
            Node = new(TypeToken!, [], ASTNodeType.BaseType);
            return true;
        }
        Node = null;
        return false;
    }
}