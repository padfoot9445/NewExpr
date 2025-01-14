namespace MEXP.Parser.Internals;
class MultiplicationParser : PrimedBinaryParserBase
{
    public MultiplicationParser(Parser p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => _Parser.Power;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)_Parser.MultiplicationPrime;

    private protected override string Name => "Multiplication";
}