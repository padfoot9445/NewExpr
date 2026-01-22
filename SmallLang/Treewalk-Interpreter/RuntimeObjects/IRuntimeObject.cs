namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public interface IRunTimeObject<out T> : IRunTimeObject
{
  public T Value { get; }
}

public interface IRunTimeObject
{
  T? AsVal<T>() where T : struct => this is IRunTimeObject<T> rtoT ? rtoT.Value : default;

  T? AsRef<T>() where T : class => this is IRunTimeObject<T> rtoT ? rtoT.Value : null;
}
