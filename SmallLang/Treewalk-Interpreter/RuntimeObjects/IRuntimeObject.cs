namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public interface IRunTimeObject<out T>
{
  public T Value { get; }
}