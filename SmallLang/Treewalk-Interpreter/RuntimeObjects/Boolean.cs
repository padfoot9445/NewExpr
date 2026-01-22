using Common.Tokens;

namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record Boolean(bool Value) : IRunTimeBaseType<Boolean, bool>
{
  public static Boolean FromToken(IToken token)
  {
    if (token.TT is TokenType.TrueLiteral) return new(true);
    return new(false);
  }
}