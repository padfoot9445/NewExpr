using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
abstract class BaseBinaryParser(SmallLangParserData data) : BaseInternalParser(data)
{
    protected abstract BaseBinaryPrimeParser BinaryPrimeParser { get; init; }
    protected abstract BaseInternalParser NextInPriority { get; init; }
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? node)
    {
        //Binary := NextInPriority BPrime
        if (!SafeParse(NextInPriority, out var LeftNode))
        {
            node = null;
            return false;
        }
        if (SafeParse(BinaryPrimeParser, out var RightNode))
        {
            RightNode!.Children.Insert(0, LeftNode!);
            node = RightNode;
            return true;
        }
        node = LeftNode;
        return false;
    }
}