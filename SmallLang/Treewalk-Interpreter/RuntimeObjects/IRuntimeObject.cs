namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public interface IRunTimeObject<out T> : IRunTimeObject
{
  public T Value { get; }
}

public interface IRunTimeObject
{
}
