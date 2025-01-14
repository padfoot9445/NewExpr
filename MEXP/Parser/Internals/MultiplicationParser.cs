namespace MEXP.Parser.Internals;
class MultiplicationParser : PrimedBinaryParserBase
{
    public MultiplicationParser(ParserData p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => Power;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)MultiplicationPrime;

    private protected override string Name => "Multiplication";
}