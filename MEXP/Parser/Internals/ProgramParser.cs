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
        if (!_Parser.SP.SafeParse(_Parser.Expression, out AnnotatedNode<Annotations>? Expr, Current: ref _Parser.Current))
        {
            Node = null;
            return false;
        }
        //assert semicolon
        IToken C = CurrentToken(Inc: true)!;
        if (C.TT != TokenType.Semicolon)
        {
            Log.Log($"Expected \";\" at {Position}");
            Node = null;
            return false;
        }
        //check for repeat(is not EOF)
        if (!CurrentToken().TCmp(TokenType.EOF))
        {
            if (_Parser.SP.SafeParse(this, out AnnotatedNode<Annotations>? Repeat, Current: ref _Parser.Current))
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
        else
        {
            Node = new(new(IsEmpty: false), [ASTLeafType.NonTerminal, ASTLeafType.Terminal], [Expr!, C], Name);
            return true;
        }
    }

    private protected override string Name => "Program";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return Program(out Node);
    }
}