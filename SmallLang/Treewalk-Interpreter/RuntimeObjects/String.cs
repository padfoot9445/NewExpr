namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record String(string Value) : IRunTimeObject<string>;