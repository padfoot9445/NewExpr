using System.Diagnostics;

namespace SmallLang.IR.Metadata;

using Common.Metadata;
using FunctionID = Common.Metadata.FunctionID<BackingNumberType>;
using FunctionSignature = Common.Metadata.FunctionSignature<BackingNumberType, SmallLangType>;
public record Scope
{
    public Scope()
    {
        foreach (var Function in Functions.StdLibFunctions)
        {
            DefineFunction(Function);
        }
        ScopeID = ++UsedScopeIDs;
    }
    private static int UsedScopeIDs { get; set; } = 0;
    private readonly int ScopeID;
    private string ScopeName => Parent is null ? "Global" : ScopeID.ToString();
    public string FullScopeName => Parent is not null ? $"{Parent.FullScopeName}.{ScopeName}" : ScopeName;
    public required Scope? Parent { get; init; }
    public HashSet<string> NamesDefinedInThisScope { get; } = new();
    public VariableName GetName(string name)
    {
        return new VariableName($"{FullScopeName}::{name}");
    }
    public VariableName SearchName(string name)
    {
        if (IsDefinedLocally(name)) return GetName(name);
        else if (Parent is not null) return Parent.SearchName(name);
        else throw new ArgumentOutOfRangeException($"{name} was not defined");
    }
    public bool IsDefined(string name) => IsDefinedLocally(name) || (Parent is not null && Parent.IsDefined(name));
    public bool IsDefinedLocally(string name) => NamesDefinedInThisScope.Contains(name);
    public VariableName DefineName(string name)
    {
        NamesDefinedInThisScope.Add(name);
        Debug.Assert(SearchName(name) == GetName(name));
        return GetName(name);
    }
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