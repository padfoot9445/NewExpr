namespace SmallLang.IR.Metadata;

public record Scope
{
    //Should be set by the parent in the case of implicit sections, or set by the child (self) in case of explicit sections
    public required Scope? Parent { get; init; }
    private HashSet<VariableName> NamesDefinedInThisScope { get; } = new();
    private Dictionary<VariableName, SmallLangType> NamesDefinedInThisScopeDictionary { get; } = new();
    public VariableName GetName(string name) => throw new NotImplementedException();
    public bool IsDefined(string name) => IsDefined(GetName(name));
    public bool IsDefined(VariableName name) => NamesDefinedInThisScope.Contains(name) || (Parent is not null && Parent.IsDefined(name));
    public void Define(VariableName name, SmallLangType Type)
    {
        NamesDefinedInThisScope.Add(name);
        NamesDefinedInThisScopeDictionary[name] = Type;
    }
}