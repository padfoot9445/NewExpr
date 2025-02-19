using System.Diagnostics;
using Common.AST;
using Common.Tokens;
using MEXP.IRs.ParseTree;

namespace MEXP.Parser.Internals;
class NegationParser : InternalParserBase
{
    public NegationParser(ParserData p) : base(p)
    {
    }

    private protected override string Name => "Negation";
    private bool ParseNegation(out AnnotatedNode<Annotations>? Node)
    {
        IToken Operator = CurrentToken(Inc: true)!;
        Debug.Assert(Operator.TCmp(TokenType.Subtraction));
        if (SafeParse(Data.Expression, out AnnotatedNode<Annotations>? Expr, Suppress: false))
        {
            Node = new(
                Attributes: Expr!.Attributes.Copy(), //Since Unary Negation does not change type at all, we can just copy - will introduce unary table at some point
                node: ASTNode.Unary(Operator: Operator, Operand: Expr!, Name));
            return true;
        }
        Log.Log($"Expected Expression after \"-\" at {Position}");
        Node = null;
        return false;
    }

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (CurrentToken()!.TT == TokenType.Subtraction)
        {
            return ParseNegation(out Node);
        }
        else if (SafeParse(Data.Primary, out AnnotatedNode<Annotations>? PrimaryNode, Suppress: false))
        {
            Node = new(
                Attributes: PrimaryNode!.Attributes.Copy(), //single nest so we can just copy
                node: ASTNode.NonTerminal(PrimaryNode!, Name)
            );
            return true;
        }
        //we can use current here because being here means primary also failed, and thus current is rolled back
        Log.Log($"Expected \"-\" or Primary at Token Position {Position}, but got \"{CurrentToken()!.Lexeme}\"; Error may be here, or (likely) at Primary:");

        Node = null;
        return false;
    }

}