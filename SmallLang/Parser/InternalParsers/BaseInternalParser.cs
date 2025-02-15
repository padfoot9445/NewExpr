using Common.AST;
using Common.Parser;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseInternalParser(ParserData data)
{
    protected ParserData Data { get; private set; } = data;
    public abstract bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node);
    public bool SafeParse(BaseInternalParser Parser, out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        var dataCopy = Data.Copy();
        if (Parser.Parse(out Node))
        {
            return true;
        }
        Data = dataCopy;
        return false;
    }
}