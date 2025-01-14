using Common.Tokens;

namespace MEXP.Parser.Internals;
class PowerPrimeParser : BinaryPrimeParserBase
{
    public PowerPrimeParser(ParserData p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => Negation;

    private protected override ICollection<TokenType> Operators => [TokenType.Exponentiation];

    private protected override string Name => "PowerPrime";
}