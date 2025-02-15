using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseBinaryParser(ParserData data) : BaseInternalParser(data)
{
    protected abstract BaseBinaryPrimeParser BinaryPrimeParser { get; init; }
    protected abstract BaseInternalParser NextInPriority { get; init; }
    public override DynamicASTNode<ASTNodeType, Attributes> Parse()
    {
        //Binary := NextInPriority BPrime
        var LeftNode = NextInPriority.Parse();
        if (BinaryPrimeParser.TryParse(out var RightNode))
        {
            RightNode!.Children.Insert(0, LeftNode);
            return RightNode;
        }
        return LeftNode;
    }
}