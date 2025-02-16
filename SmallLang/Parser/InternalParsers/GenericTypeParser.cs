using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class GenericTypeParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (Data.TryConsumeToken(out var GenericTypeToken, TokenType.TypeArray, TokenType.TypeList, TokenType.TypeSet, TokenType.TypeDict))
        {
            Node = new(GenericTypeToken!, [], ASTNodeType.GenericType);
            return true;
        }
        Node = null;
        return false;
    }
}