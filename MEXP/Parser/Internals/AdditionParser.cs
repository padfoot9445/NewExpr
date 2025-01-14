namespace MEXP.Parser.Internals;
class AdditionParser : PrimedBinaryParserBase
{
    public AdditionParser(Parser p) : base(p)
    {
    }
    private protected override InternalParserBase NextInPriority => _Parser.Multiplication;

    private protected override BinaryPrimeParserBase BinaryPrime => (BinaryPrimeParserBase)_Parser.AdditionPrime;

    private protected override string Name => "Addition";
}