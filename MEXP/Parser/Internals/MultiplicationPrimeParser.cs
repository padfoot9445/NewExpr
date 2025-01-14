using Common.Tokens;

namespace MEXP.Parser.Internals;
class MultiplicationPrimeParser : BinaryPrimeParserBase
{
    public MultiplicationPrimeParser(ParserData p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => Power;

    private protected override ICollection<TokenType> Operators => [TokenType.Multiplication, TokenType.Division];

    private protected override string Name => "MultiplicationPrime";
}