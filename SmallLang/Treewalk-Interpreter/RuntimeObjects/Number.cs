using Common.Tokens;

namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record Number(double Value) : IRunTimeBaseType<Number, double>
{
  public static Number FromToken(IToken token)
  {
    return new(double.Parse(token.Lexeme));
  }
}