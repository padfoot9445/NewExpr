using Common.AST;

namespace MEXP.Parser.Internals;
class ExpressionParser : InternalParserBase
{
    public ExpressionParser(Parser p) : base(p)
    {
    }
    public bool Expression(out AnnotatedNode<Annotations>? Node)
    {
        if (_Parser.SP.SafeParse(_Parser.Declaration, out AnnotatedNode<Annotations>? Add, Suppress: false, Current: ref _Parser.Current)) //no additional context to add here so we get the context from safeparse
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, nameof(Expression))); //TypeCode <- Addition.TypeCode
            return true;
        }
        Node = null;
        return false;
    }
    private protected override string Name => "Expression";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return Expression(out Node);
    }
}