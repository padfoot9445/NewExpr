namespace SmallLang.IR.Metadata;

public record Scope()
{
    public required Scope? Parent { get; init; }
    public HashSet<VariableName> NamesDefinedInThisScope { get; } = new();
    public VariableName GetName(string name) => throw new NotImplementedException();
    public bool IsDefined(string name) => IsDefined(GetName(name));
    public bool IsDefined(VariableName name) => NamesDefinedInThisScope.Contains(name) || (Parent is not null && Parent.IsDefined(name));
}