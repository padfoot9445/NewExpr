using Common.Tokens;

namespace MEXP.Parser.Internals;
class AdditionPrimeParser : BinaryPrimeParserBase
{
    public AdditionPrimeParser(ParserData p) : base(p)
    {
    }

    private protected override InternalParserBase NextInPriority => Multiplication;

    private protected override ICollection<TokenType> Operators => [TokenType.Addition, TokenType.Subtraction];

    private protected override string Name => "AdditionPrime";
}