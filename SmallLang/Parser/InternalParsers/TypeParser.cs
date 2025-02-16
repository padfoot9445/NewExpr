using Common.AST;
using Common.Parser;

namespace SmallLang.Parser.InternalParsers;
class TypeParser(ParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        throw new NotImplementedException();
    }
}