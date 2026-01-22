using SmallLang.TreeWalkInterpreter.RuntimeObjects;

namespace SmallLang.TreeWalkInterpreter;

public record InterpreterState(Stack<IRunTimeObject> Stack);