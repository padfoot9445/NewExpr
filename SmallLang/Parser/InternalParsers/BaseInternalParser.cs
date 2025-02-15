using Common.AST;
using Common.Parser;

namespace SmallLang.InternalParsers;
abstract class BaseInternalParser(ParserData data)
{
    protected readonly ParserData Data = data;
    protected abstract DynamicASTNode<ASTNodeType, Attributes> InternalParse();
    public virtual DynamicASTNode<ASTNodeType, Attributes> Parse() => InternalParse();
}