namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record Number(double Value) : IRunTimeObject<double>;