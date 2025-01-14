using Common.Tokens;

namespace MEXP.Parser.Internals;
class MultiplicationPrimeParser : BinaryPrimeParserBase
{
    public MultiplicationPrimeParser(Parser p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => _Parser.Power;

    private protected override ICollection<TokenType> Operators => [TokenType.Multiplication, TokenType.Division];

    private protected override string Name => "MultiplicationPrime";
}