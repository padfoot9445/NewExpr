namespace SmallLang.TreeWalkInterpreter.RuntimeObjects;

public record Boolean(bool Value) : IRunTimeObject<bool>;