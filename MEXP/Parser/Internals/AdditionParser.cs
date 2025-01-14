namespace MEXP.Parser.Internals;
class AdditionParser : PrimedBinaryParserBase
{
    public AdditionParser(ParserData p) : base(p)
    {
    }
    private protected override InternalParserBase NextInPriority => Multiplication;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)AdditionPrime;

    private protected override string Name => "Addition";
}