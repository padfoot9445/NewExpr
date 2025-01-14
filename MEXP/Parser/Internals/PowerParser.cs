namespace MEXP.Parser.Internals;
class PowerParser : PrimedBinaryParserBase
{
    public PowerParser(ParserData p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => Negation;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)PowerPrime;

    private protected override string Name => "Power";
}