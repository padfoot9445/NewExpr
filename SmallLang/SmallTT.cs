using sly.lexer;

namespace SmallLang;
public enum SmallTT
{
    EOF,
    [Sugar("=")]
    Equals,
    [Sugar("-")]
    Plus,
    [Sugar(":")]
    Colon
}