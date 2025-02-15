using Common.AST;
using Common.Parser;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseInternalParser(ParserData data)
{
    protected readonly ParserData Data = data;
    public abstract bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node);

}