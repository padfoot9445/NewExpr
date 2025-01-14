using Common.AST;

namespace MEXP.Parser.Internals;
class ExpressionParser : InternalParserBase
{
    public ExpressionParser(ParserData p) : base(p)
    {
    }
    private protected override string Name => "Expression";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (SafeParse(Declaration, out AnnotatedNode<Annotations>? Add, Suppress: false)) //no additional context to add here so we get the context from safeparse
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, Name)); //TypeCode <- Addition.TypeCode
            return true;
        }
        Node = null;
        return false;
    }
}