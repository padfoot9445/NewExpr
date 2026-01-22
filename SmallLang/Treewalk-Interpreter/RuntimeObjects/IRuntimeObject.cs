namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public interface IRunTimeObject<out T> : IRunTimeObject
{
  public T Value { get; }
}

public interface IRunTimeObject
{
  T As<T>() => this is IRunTimeObject<T> rtoT ? rtoT.Value : default;
}
