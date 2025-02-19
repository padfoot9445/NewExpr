using Common.Logger;
using Common.Parser;
using Common.Tokens;
using SmallLang.Parser.InternalParsers;

namespace SmallLang.Parser;
class SmallLangParserData(int start, int current, List<IToken> tokens, ILogger logger) : ParserData(start, current, tokens, logger)
{
    public SmallLangParserData(ParserData baseVal) : this(baseVal.Start, baseVal.Current, baseVal.Tokens, baseVal.Logger)
    {

    }
    public override SmallLangParserData Copy()
    {
        return new(base.Copy());
    }
    #region ParserList
    public StatementParser Statement => new StatementParser(this);
    public SectionParser Section => new SectionParser(this);
    public BlockParser Block => new BlockParser(this);
    public CondParser Cond => new CondParser(this);
    public ReturnStatementParser ReturnStatement => new ReturnStatementParser(this);
    public ExpressionParser Expression => new ExpressionParser(this);
    public FunctionParser Function => new FunctionParser(this);
    public LoopControlStatementParser LCtrlStatement => new LoopControlStatementParser(this);
    public LoopParser Loop => new LoopParser(this);
    public OpValInLCTRLParser OpValInLCTRL => new(this);
    public TypeParser Type => new TypeParser(this);
    public TypeAndIdentifierCSVOrEmptyParser TypeAndIdentifierCSVOrEmpty => new(this);
    public BaseTypeParser BaseType => new(this);
    public GenericTypeParser GenericType => new(this);
    #endregion

}