namespace MEXP.Parser.Internals;
class MultiplicationParser : PrimedBinaryParserBase
{
    public MultiplicationParser(Parser p) : base(p)
    {
    }

    private protected override ParsingFunction NextInPriority => _Parser.Power;

    private protected override ParsingFunction BinaryPrime => _Parser.MultiplicationPrime;

    private protected override string Name => "Multiplication";
}