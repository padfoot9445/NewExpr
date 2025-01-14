using Common.AST;
using Common.Logger;
using Common.Tokens;
using MEXP.Parser;

namespace MEXP.Parser.Internals;
abstract class InternalParserBase
{
    public InternalParserBase(Parser p)
    {
        this._Parser = p;
        Log = _Parser.Log;
    }
    private protected Parser _Parser { get; private set; }
    public abstract bool Parse(out AnnotatedNode<Annotations>? Node);
    private protected IToken? CurrentToken(int offset = 0, bool Inc = false) => _Parser.CurrentToken(offset, Inc);
    private protected ILogger Log;
    private protected int Position => _Parser.Position;
    private protected abstract string Name { get; }
    private protected bool SafeParse(InternalParserBase Parser, out AnnotatedNode<Annotations>? Node, bool Suppress = true) => _Parser.SP.SafeParse(Parser, out Node, ref _Parser.Current, Suppress);
}