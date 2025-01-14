using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
class ProgramParser : InternalParserBase
{
    public ProgramParser(Parser p) : base(p)
    {
    }
    bool Program(out AnnotatedNode<Annotations>? Node)
    {
        if (!SafeParse(_Parser.Expression, out AnnotatedNode<Annotations>? Expr))
        {
            Node = null;
            return false;
        }
        //assert semicolon
        IToken C = CurrentToken(Inc: true)!;
        if (!C.TCmp(TokenType.Semicolon))
        {
            Log.Log($"Expected \";\" at {Position}");
            Node = null;
            return false;
        }
        //check for repeat(is not EOF)
        if (CurrentToken().TCmp(TokenType.EOF))
        {
            Node = new(new(IsEmpty: false), [ASTLeafType.NonTerminal, ASTLeafType.Terminal], [Expr!, C], Name);
            return true;
        }
        else
        {
            if (SafeParse(this, out AnnotatedNode<Annotations>? Repeat))
            {
                Node = new(ASTNode.Repeating(Expr!, C, Repeat!, Name));
                return true;
            }
            else
            {
                Log.Log($"Expected EOF at Token Position {Position} but got \"{CurrentToken()!.Lexeme}\"");
                Node = null;
                return false;
            }
        }

    }

    private protected override string Name => "Program";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return Program(out Node);
    }
}