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
    public bool TryParse(out DynamicASTNode<ASTNodeType, Attributes>? ResultNode)
    {
        //BPrime := OPERATOR Binary | EMPTY;
        if (Data.AtEnd || !Operators.Contains(Data.CurrentToken))
        {
            ResultNode = null;
            return false;
        }
        Debug.Assert(Operators.Contains(Data.CurrentToken));
        IToken Operator = Data.Advance();
        var RightNode = Binary.Parse();
        ResultNode = new(Operator, [RightNode], OutNodeType);
        return true;
    }
    public override DynamicASTNode<ASTNodeType, Attributes> Parse()
    {
        if (TryParse(out var OutNode))
        {
            return OutNode!;
        }
        throw new Exception($"Tried to parse Empty BinaryPrimeParser at {(Data.SafeCurrentToken ?? IToken.NewToken(TokenType.Number, "ERROR", -1)).Position}, TokenPosition");
    }
}