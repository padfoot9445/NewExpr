using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseBinaryParser(ParserData data) : BaseInternalParser(data)
{
    protected abstract BaseBinaryPrimeParser BinaryPrimeParser { get; init; }
    protected abstract BaseInternalParser NextInPriority { get; init; }
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? node)
    {
        //Binary := NextInPriority BPrime
        if (!NextInPriority.Parse(out var LeftNode))
        {
            node = null;
            return false;
        }
        if (BinaryPrimeParser.Parse(out var RightNode))
        {
            RightNode!.Children.Insert(0, LeftNode!);
            node = RightNode;
            return true;
        }
        node = LeftNode;
        return false;
    }
}