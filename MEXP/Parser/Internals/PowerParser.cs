namespace MEXP.Parser.Internals;
class PowerParser : PrimedBinaryParserBase
{
    public PowerParser(Parser p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => _Parser.Negation;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)_Parser.PowerPrime;

    private protected override string Name => "Power";
}