using Common.AST;
using Common.Logger;
using Common.Tokens;
using MEXP.IRs.ParseTree;
using MEXP.Parser;

namespace MEXP.Parser.Internals;
abstract class InternalParserBase
{
    public InternalParserBase(ParserData p)
    {
        this.ParserData = p;
        Log = ParserData.Log;
        TP = ParserData.TP;
    }
    private protected IToken? Advance() => ParserData.Advance();
    private protected ParserData ParserData { get; private set; }
    public abstract bool Parse(out AnnotatedNode<Annotations>? Node);
    private protected IToken? CurrentToken(int offset = 0, bool Inc = false) => ParserData.CurrentToken(offset, Inc);
    private protected readonly ITypeProvider TP;
    private protected readonly ILogger Log;
    private protected int Position => ParserData.Position;
    private protected abstract string Name { get; }
    public Annotations GetFromChildIndex(ASTNode node, int index)
    {
        if (node.Children.Length <= index)
        {
            throw new ArgumentOutOfRangeException($"Index out of range, {index}, {node.Children.Length}");
        }
        return ((AnnotatedNode<Annotations>)node.Children[index]).Attributes;
    }
    private protected bool SafeParse(InternalParserBase Parser, out AnnotatedNode<Annotations>? Node, bool Suppress = true) => ParserData.SP.SafeParse(Parser, out Node, ref ParserData.Current, Suppress);
    private protected InternalParserBase Program => _Program is null ? new ProgramParser(ParserData) : _Program;
    private InternalParserBase? _Program = null;
    private protected InternalParserBase Expression => _Expression is null ? new ExpressionParser(ParserData) : _Expression;
    private InternalParserBase? _Expression = null;
    private protected InternalParserBase Declaration => _Declaration is null ? new DeclarationParser(ParserData) : _Declaration;
    private InternalParserBase? _Declaration = null;
    //<AssignmentPrime> ::= "=" <Addition> <AssignmentPrime> | <Empty>;
    private protected InternalParserBase AssignmentPrime => _AssignmentPrime is null ? new AssignmentPrimeParser(ParserData) : _AssignmentPrime;
    private InternalParserBase? _AssignmentPrime = null;

    private protected InternalParserBase Addition => _Addition is null ? new AdditionParser(ParserData) : _Addition;
    private InternalParserBase? _Addition = null;
    private protected InternalParserBase AdditionPrime => _AdditionPrime is null ? new AdditionPrimeParser(ParserData) : _AdditionPrime;
    private InternalParserBase? _AdditionPrime = null;
    // AdditionPrime ::= ("-" | "+") Multiplication AdditionPrime | Empty
    private protected InternalParserBase Multiplication => _Multiplication is null ? new MultiplicationParser(ParserData) : _Multiplication;
    private InternalParserBase? _Multiplication = null;

    private protected InternalParserBase MultiplicationPrime => _MultiplicationPrime is null ? new MultiplicationPrimeParser(ParserData) : _MultiplicationPrime;
    private InternalParserBase? _MultiplicationPrime = null;
    private protected InternalParserBase Power => _Power is null ? new PowerParser(ParserData) : _Power;
    private InternalParserBase? _Power = null;
    private protected InternalParserBase PowerPrime => _PowerPrime is null ? new PowerPrimeParser(ParserData) : _PowerPrime;
    private InternalParserBase? _PowerPrime = null;
    private protected InternalParserBase Negation => _Negation is null ? new NegationParser(ParserData) : _Negation;
    private InternalParserBase? _Negation = null;
    private protected InternalParserBase Primary => _Primary is null ? new PrimaryParser(ParserData) : _Primary;
    private InternalParserBase? _Primary = null;
    private protected InternalParserBase Type => _Type is null ? new TypeParser(ParserData) : _Type;
    private InternalParserBase? _Type = null;

}