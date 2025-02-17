using Common.Lexer;
using Common.Tokens;
using sly.lexer;

namespace SmallLang;
public sealed class Lexer(string input) : Scanner(input)
{
    protected override ICollection<string> StringQuotes => ["\"", "'"];
    protected override ICollection<string> EOLCommentBegin => ["#"];
    protected override ICollection<char> EscapeChars => ['\\'];
    protected override IList<string> StartEndCommentBegin => ["/*"];
    protected override IList<(string, char)> EscapeSequenceToStringValue =>
    [
        (@"n", '\n'),
        (@"t", '\t'),
        (@"r", '\r'),
        (@"b", '\b'),
        (@"f", '\f'),
        (@"v", '\v'),
        (@"\", '\\'),
        (@"'", '\''),
        (@"" + "\"", '\"')
    ];
    protected override IList<string> StartEndCommentEnd => ["*/"];
    protected override IEnumerable<(string, TokenType)> MCTTM => new (string, TokenType)[]
    {
        (";",TokenType.Semicolon),
        ("return",TokenType.Return),
        ("break",TokenType.Break),
        ("continue",TokenType.Continue),
        ("as",TokenType.As),
        ("for",TokenType.For),
        (")",TokenType.CloseParen),
        ("while",TokenType.While),
        ("if",TokenType.If),
        ("else",TokenType.Else),
        ("switch",TokenType.Switch),
        ("{",TokenType.OpenCurly),
        ("}",TokenType.CloseCurly),
        (":",TokenType.Colon),
        ("cascading",TokenType.Cascading),
        (",",TokenType.Comma),
        ("ref",TokenType.Ref),
        ("readonly",TokenType.Readonly),
        ("frozen",TokenType.Frozen),
        ("immut",TokenType.Immut),
        ("copy",TokenType.Copy),
        ("=",TokenType.Equals),
        ("implies",TokenType.LogicalImplies),
        ("or",TokenType.LogicalOr),
        ("xor",TokenType.LogicalXor),
        ("and",TokenType.LogicalAnd),
        ("not",TokenType.LogicalNot),
        ("==",TokenType.EqualTo),
        ("!=",TokenType.NotEqualTo),
        (">",TokenType.GreaterThan),
        (">=",TokenType.GreaterThanOrEqualTo),
        ("<",TokenType.LessThan),
        ("<=",TokenType.LessThanOrEqualTo),
        ("+",TokenType.Addition),
        ("-",TokenType.Subtraction),
        ("*",TokenType.Multiplication),
        ("/",TokenType.Division),
        ("**",TokenType.Exponentiation),
        ("!",TokenType.Factorial),
        ("|",TokenType.BitwiseOr),
        ("^",TokenType.BitwiseXor),
        ("&",TokenType.BitwiseAnd),
        ("~",TokenType.BitwiseNegation),
        ("(",TokenType.OpenParen),
        ("new",TokenType.New),
        ("[",TokenType.OpenSquare),
        ("]",TokenType.CloseSquare),
        (".",TokenType.Dot),
        (",",TokenType.Comma),
        ("<[",TokenType.OpenAngleSquare),
        ("]>",TokenType.CloseAngleSquare),
        ("array",TokenType.TypeArray),
        ("list",TokenType.TypeList),
        ("set",TokenType.TypeSet),
        ("dict",TokenType.TypeDict),
        ("byte",TokenType.TypeByte),
        ("short",TokenType.TypeShort),
        ("int",TokenType.TypeInt),
        ("long",TokenType.TypeLong),
        ("bigint",TokenType.TypeLongInt),
        ("float",TokenType.TypeFloat),
        ("double",TokenType.TypeDouble),
        ("rational",TokenType.TypeRational),
        ("bigfloat",TokenType.TypeNumber),
        ("string",TokenType.TypeString),
        ("char",TokenType.TypeChar),
        ("void",TokenType.TypeVoid),
        ("true",TokenType.TrueLiteral),
        ("false",TokenType.FalseLiteral)
    };
    public override IEnumerable<IToken> Scan()
    {
        ILexer<SmallTT> lexer = LexerBuilder.BuildLexer<SmallTT>().Result;
        var tokenss = lexer.Tokenize(input);
        var tokensd = tokenss.Tokens;
        var x = tokensd.GetEnumerator();
        var tokens = tokensd.ToList();
        return [];
        // var rt = tokens.Select(x => IToken.NewToken(x.TokenID, x.Value, x.Position.Index, x.StringWithoutQuotes)).ToArray();
        // for (int i = 0; i < rt.Length; i++)
        // {
        //     var token = rt[i];
        //     if (token.TT == TokenType.String)
        //     {
        //         input = $"\"{token.Literal}\"";
        //         rt[i] = GetLiteral();
        //     }
        // }
        // return rt;
    }
}