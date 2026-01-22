using Common.Tokens;

namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record String(string Value) : IRunTimeBaseType<String, string>
{
  public static String FromToken(IToken token)
  {
    return new(token.Literal);
  }
}