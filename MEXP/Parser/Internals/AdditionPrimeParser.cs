using Common.Tokens;

namespace MEXP.Parser.Internals;
class AdditionPrimeParser : BinaryPrimeParserBase
{
    public AdditionPrimeParser(Parser p) : base(p)
    {
    }

    private protected override ParsingFunction NextInPriority => _Parser.Multiplication;

    private protected override ICollection<TokenType> Operators => [TokenType.Addition, TokenType.Subtraction];

    private protected override string Name => "AdditionPrime";
}