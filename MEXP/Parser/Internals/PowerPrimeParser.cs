using Common.Tokens;

namespace MEXP.Parser.Internals;
class PowerPrimeParser : BinaryPrimeParserBase
{
    public PowerPrimeParser(Parser p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => _Parser.Negation;

    private protected override ICollection<TokenType> Operators => [TokenType.Exponentiation];

    private protected override string Name => "PowerPrime";
}