using Common.AST;
using Common.Parser;
using Common.Tokens;
namespace SmallLang.Parser.InternalParsers;
class TypeAndIdentifierCSVOrEmptyParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        throw new NotImplementedException();
    }
}