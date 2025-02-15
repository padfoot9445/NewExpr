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

    #region ParserList
    protected StatementParser Statement => new StatementParser(Data);
    protected SectionParser Section => new SectionParser(Data);
    protected BlockParser Block => new BlockParser(Data);
    protected CondParser Cond => new CondParser(Data);
    protected ReturnStatementParser ReturnStatement => new ReturnStatementParser(Data);
    protected ExpressionParser Expression => new ExpressionParser(Data);
    protected FunctionParser Function => new FunctionParser(Data);
    protected LoopControlStatementParser LCtrlStatement => new LoopControlStatementParser(Data);
    protected LoopParser Loop => new LoopParser(Data);
    #endregion
}