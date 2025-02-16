using Common.AST;
using Common.Tokens;
using MEXP.IRs.ParseTree;

namespace MEXP.Parser.Internals;
class InternalParser : InternalParserBase, IParser
{
    public InternalParser(ParserData p) : base(p)
    {
    }

    private protected override string Name => throw new NotImplementedException();
    public override bool Parse(out AnnotatedNode<Annotations>? node)
    {
        //returns true if parse success; node will be of type AST. If parse failure, returns false; node is undefined
        if (ParserData.Input.Count == 0)
        {
            node = null;
            return false;
        }
        else if (!SafeParse(Data.Program, out node))
        {
            if (Recover())
            {
                Parse(out node);
                //if we skipped forwards, it means that we are parsing from a new location, so we can continue
                //but we still failed so
                return false;
            }
            else
            {
                //no skip forwards, we've discovered as many syntax errors as we can.
                node = null;
                return false;
            }
        }
        return true;
    }
    bool Recover()
    {
        //skip forwards to the next token in RecoveryTokens. If we reach EOF before finding the next token, return false; else true
        while (!CurrentToken().TCmp(TokenType.EOF))
        {
            if (ParserData.RecoveryTokens.Contains(ParserData.Advance()!.TT)) //current++ because no matter what we want to focus on the token after the one we are focusing on now
            {
                return true;
            }
        }
        return false;
    }
}
