using Common.Tokens;
using SmallLang.TreeWalkInterpreter.RuntimeObjects;

namespace SmallLang.TreeWalkInterpreter;

public interface IRunTimeBaseType<out TSelf, out TBacking> : IRunTimeObject<TBacking>
{
  public static abstract TSelf FromToken(IToken token);
}