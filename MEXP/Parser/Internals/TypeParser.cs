using Common.AST;
using Common.Tokens;
using MEXP.IRs.ParseTree;
using MEXP.Parser;

namespace MEXP.Parser.Internals;
class TypeParser : InternalParserBase
{
    public TypeParser(ParserData p) : base(p)
    {
    }
    private protected override string Name => "Type";
    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (CurrentToken().TCmp([TokenType.TypeByte, TokenType.TypeDouble, TokenType.TypeInt, TokenType.TypeLong, TokenType.TypeLongInt, TokenType.TypeFloat, TokenType.TypeNumber]))
        {
            Node = new(
                new(
                    CanBeResolvedToAssignable: false, //even though it *can*, we can't assign
                    TypeDenotedByIdentifier: TP.GetTypeFromTypeDenotingIdentifier(CurrentToken()!.Lexeme),
                    TypeCode: null
                ),
                ASTNode.Terminal(CurrentToken()!, nameof(Type)));
            Advance();
            return true;
        }
        Log.Log($"Expected valid Type at {Position}");
        Node = null;
        return false;
    }
}