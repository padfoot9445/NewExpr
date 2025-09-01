namespace SmallLang.IR.Metadata;

using Common.Metadata;
using FunctionID = Common.Metadata.FunctionID<BackingNumberType>;
using FunctionSignature = Common.Metadata.FunctionSignature<BackingNumberType, SmallLangType>;
public record Scope()
{
    public required Scope? Parent { get; init; }
    public HashSet<VariableName> NamesDefinedInThisScope { get; } = new();
    public VariableName GetName(string name) => throw new NotImplementedException();
    public bool IsDefined(string name) => IsDefined(GetName(name));
    public bool IsDefined(VariableName name) => NamesDefinedInThisScope.Contains(name) || (Parent is not null && Parent.IsDefined(name));
    public virtual bool Equals(Scope? other)
    {
        return other is not null && Parent == other.Parent && NamesDefinedInThisScope.Intersect(other.NamesDefinedInThisScope).Count() == NamesDefinedInThisScope.Count;
    }
    public override int GetHashCode()
    {
        return Parent is null ? 0 : (Parent.GetHashCode() + 1) * 17;
    }
    private Functions FunctionsDefinedInThisScope { get; } = new();
    private bool FunctionIsDefinedInThisScope(string name) => FunctionsDefinedInThisScope.RegisteredFunctions.Any(x => x.Name == name);
    public bool FunctionIsDefined(string name)
    {
        return FunctionIsDefinedInThisScope(name) || (Parent is not null && Parent.FunctionIsDefined(name));
    }
    public void DefineFunction(FunctionSignature functionSignature)
    {
        FunctionsDefinedInThisScope.RegisterFunction(functionSignature);
    }
    public FunctionSignature GetSignature(string name)
    {
        if (FunctionIsDefinedInThisScope(name))
        {
            return FunctionsDefinedInThisScope.GetSignature(name);
        }
        else
        {
            if (Parent is null) throw new ArgumentOutOfRangeException($"Could not find function defined in the current or enclosing scope of name {name}");
            else return Parent.GetSignature(name);

        }
    }
    public FunctionID GetID(string name) => GetSignature(name).ID;
}