using Common.AST;
using Common.Tokens;
using MEXP.IRs.ParseTree;

namespace MEXP.Parser.Internals;
class ProgramParser : InternalParserBase
{
    public ProgramParser(ParserData p) : base(p)
    {
    }
    private protected override string Name => "Program";
    private bool ConsumeSemicolon(out IToken? Semicolon)
    {
        Semicolon = CurrentToken(Inc: true)!;
        if (!Semicolon.TCmp(TokenType.Semicolon))
        {
            Log.Log($"Expected \";\" at {Position}");
            return false;
        }
        return true;
    }
    private bool ParseRepeat(AnnotatedNode<Annotations> Expr, IToken C, out AnnotatedNode<Annotations>? Node)
    {
        if (SafeParse(this, out AnnotatedNode<Annotations>? Repeat))
        {
            Node = new(ASTNode.Repeating(Expr!, C!, Repeat!, Name));
            return true;
        }
        else
        {
            Log.Log($"Expected EOF at Token Position {Position} but got \"{CurrentToken()!.Lexeme}\"");
            Node = null;
            return false;
        }
    }
    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (!SafeParse(Expression, out AnnotatedNode<Annotations>? Expr))
        {
            Node = null;
            return false;
        }
        //assert semicolon
        if (!ConsumeSemicolon(out IToken? C))
        {
            Node = null;
            return false;
        }
        //check for repeat(is not EOF)
        if (CurrentToken().TCmp(TokenType.EOF))
        {
            Node = new(new(IsEmpty: false), [ASTLeafType.NonTerminal, ASTLeafType.Terminal], [Expr!, C!], Name);
            return true;
        }
        else
        {
            return ParseRepeat(Expr!, C!, out Node);
        }

    }
}