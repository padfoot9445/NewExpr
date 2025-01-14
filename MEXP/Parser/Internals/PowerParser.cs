namespace MEXP.Parser.Internals;
class PowerParser : PrimedBinaryParserBase
{
    public PowerParser(Parser p) : base(p)
    {
    }

    private protected override ParsingFunction NextInPriority => _Parser.Negation;

    private protected override ParsingFunction BinaryPrime => _Parser.PowerPrime;

    private protected override string Name => "Power";
}