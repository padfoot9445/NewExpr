using Common.AST;
using Common.Logger;
using Common.Tokens;
using MEXP.Parser.Internals;
namespace MEXP.Parser;
public interface IParser
{
    public static IParser NewParser(IEnumerable<IToken> Tokens, ILogger? Log = null, ITypeProvider? TP = null) => new InternalParser(new ParserData(Tokens, Log, TP));
    public static bool Parse(IEnumerable<IToken> tokens, out AnnotatedNode<Annotations>? Node, ILogger? Log = null, ITypeProvider? TP = null) => NewParser(tokens.ToArray(), Log, TP).Parse(out Node);
    public bool Parse(out AnnotatedNode<Annotations>? Node);
}