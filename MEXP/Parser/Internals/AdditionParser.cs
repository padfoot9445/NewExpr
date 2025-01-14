namespace MEXP.Parser.Internals;
class AdditionParser : PrimedBinaryParserBase
{
    public AdditionParser(Parser p) : base(p)
    {
    }
    private protected override ParsingFunction NextInPriority => _Parser.Multiplication;

    private protected override ParsingFunction BinaryPrime => _Parser.AdditionPrime;

    private protected override string Name => "Addition";
}