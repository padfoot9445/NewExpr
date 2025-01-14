using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
class NegationParser : InternalParserBase
{
    public NegationParser(Parser p) : base(p)
    {
    }

    private protected override string Name => "Negation";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (CurrentToken()!.TT == TokenType.Subtraction)
        {
            IToken Operator = CurrentToken(Inc: true)!;
            if (_Parser.SP.SafeParse(_Parser.Expression, out AnnotatedNode<Annotations>? Expr, Suppress: false, Current: ref _Parser.Current))
            {
                Node = new(
                    Attributes: Expr!.Attributes.Copy(), //Since Unary Negation does not change type at all, we can just copy - will introduce unary table at some point
                    node: ASTNode.Unary(Operator: Operator, Operand: Expr!, Name));
                return true;
            }
        }
        else if (_Parser.SP.SafeParse(_Parser.Primary, out AnnotatedNode<Annotations>? PrimaryNode, Suppress: false, Current: ref _Parser.Current))
        {
            Node = new(
                Attributes: PrimaryNode!.Attributes.Copy(), //single nest so we can just copy
                node: ASTNode.NonTerminal(PrimaryNode!, Name)
            );
            return true;
        }
        //we can use current here because being here means primary also failed, and thus current is rolled back
        Log.Log($"Expected \"-\" or Primary at Token Position {Position}, but got \"{CurrentToken()!.Lexeme}\"; Error may be here, or at Primary:");

        Node = null;
        return false;
    }

}