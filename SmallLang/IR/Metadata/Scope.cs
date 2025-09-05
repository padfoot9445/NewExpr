using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SmallLang.IR.Metadata;

using Common.Dispatchers;
using Common.Metadata;
using FunctionID = Common.Metadata.FunctionID<BackingNumberType>;
using FunctionSignature = Common.Metadata.FunctionSignature<BackingNumberType, GenericSmallLangType>;

public record Scope
{
    public Scope(Scope? Parent)
    {
        this.Parent = Parent;


        NamesDefinedInThisScope = new HashSet<string>();
        FunctionsDefinedInThisScope = new Functions();
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
    public Scope? Parent { get; }
    public HashSet<string> NamesDefinedInThisScope { get; } = [];

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
        return other is not null && Parent == other.Parent &&
               NamesDefinedInThisScope.Intersect(other.NamesDefinedInThisScope).Count() ==
               NamesDefinedInThisScope.Count;
    }

    public override int GetHashCode()
    {
        return Parent is null
            ? 0
            : HashCode.Combine(Parent.GetHashCode(),
                NamesDefinedInThisScope.Aggregate(17, (x, y) => x ^ y.GetHashCode()),
                FunctionsDefinedInThisScope.GetHashCode());
    }

    private Functions FunctionsDefinedInThisScope { get; } = new();

    private bool FunctionIsDefinedInThisScope(string name) =>
        FunctionsDefinedInThisScope.RegisteredFunctions.Any(x => x.Name == name);

    public bool FunctionIsDefined(string name)
    {
        return FunctionIsDefinedInThisScope(name) || (Parent is not null && Parent.FunctionIsDefined(name));
    }

    public void DefineFunction(FunctionSignature functionSignature)
    {
        DefineName(functionSignature.Name);
        FunctionsDefinedInThisScope.RegisterFunction(functionSignature);

        DefineTypeOfName(GetName(functionSignature.Name), new(TypeData.Void));

        Console.WriteLine($"Scope {ScopeID} defining function {functionSignature.Name} with VName {GetName(functionSignature.Name)}");
    }

    public FunctionSignature GetSignature(string name)
    {
        if (FunctionIsDefinedInThisScope(name))
        {
            return FunctionsDefinedInThisScope.GetSignature(name);
        }
        else
        {
            if (Parent is null)
                throw new ArgumentException(
                    $"Could not find function defined in the current or enclosing scope of name {name}");
            else return Parent.GetSignature(name);
        }
    }

    public FunctionID GetID(string name) => GetSignature(name).ID;

    public FunctionID GetIDOfConstructorFunction(GenericSmallLangType type)
    {
        return new(0);
    }

    private Dictionary<VariableName, GenericSmallLangType> TypeNameCombinationsDefinedInThisScope { get; } = [];

    public void DefineTypeOfName(VariableName variableName, GenericSmallLangType Type)
    {
        TypeNameCombinationsDefinedInThisScope[variableName] = Type;
    }

    public bool TryGetTypeOfVariable(VariableName variableName, [NotNullWhen(true)] out GenericSmallLangType? type)
    {
        if (TypeNameCombinationsDefinedInThisScope.TryGetValue(variableName, out type)) return true;
        else if (Parent is null) return false;
        else return Parent.TryGetTypeOfVariable(variableName, out type);
    }
}