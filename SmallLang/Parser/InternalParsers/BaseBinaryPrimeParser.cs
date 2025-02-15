using System.Diagnostics;
using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseBinaryPrimeParser(ParserData data) : BaseInternalParser(data)
{
    protected abstract ASTNodeType OutNodeType { get; init; }
    protected abstract ICollection<IToken> Operators { get; init; }
    protected abstract BaseBinaryParser Binary { get; init; }
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? ResultNode)
    {
        //BPrime := OPERATOR Binary | EMPTY;
        if (Data.AtEnd || !Operators.Contains(Data.CurrentToken))
        {
            ResultNode = null;
            return false;
        }
        Debug.Assert(Operators.Contains(Data.CurrentToken));
        IToken Operator = Data.Advance();
        if (!Binary.Parse(out var RightNode))
        { ResultNode = null; return false; }
        ResultNode = new(Operator, [RightNode!], OutNodeType);
        return true;
    }
}