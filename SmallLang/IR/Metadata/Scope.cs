using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Common.Metadata;

namespace SmallLang.IR.Metadata;

using FunctionID = FunctionID<BackingNumberType>;
using FunctionSignature = FunctionSignature<BackingNumberType, GenericSmallLangType>;

public sealed class Scope : IEquatable<Scope>
{
    private readonly int ScopeID;

    public Scope(Scope? Parent)
    {
        this.Parent = Parent;


        NamesDefinedInThisScope = new HashSet<string>();
        FunctionsDefinedInThisScope = new Functions();
        TypeNameCombinationsDefinedInThisScope = new Dictionary<VariableName, GenericSmallLangType>();
        ScopeID = ++UsedScopeIDs;
        foreach (var Function in Functions.StdLibFunctions) DefineFunction(Function);
    }

    private static int UsedScopeIDs { get; set; }
    private string ScopeName => Parent is null ? "Global" : ScopeID.ToString();
    public string FullScopeName => Parent is not null ? $"{Parent.FullScopeName}.{ScopeName}" : ScopeName;
    public Scope? Parent { get; }
    public HashSet<string> NamesDefinedInThisScope { get; }

    private Functions FunctionsDefinedInThisScope { get; }

    private Dictionary<VariableName, GenericSmallLangType> TypeNameCombinationsDefinedInThisScope { get; }


    public VariableName GetName(string name)
    {
        return new VariableName($"{FullScopeName}::{name}");
    }

    public VariableName SearchName(string name)
    {
        if (IsDefinedLocally(name)) return GetName(name);
        if (Parent is not null) return Parent.SearchName(name);
        throw new ArgumentOutOfRangeException($"{name} was not defined");
    }

    public bool IsDefined(string name)
    {
        return IsDefinedLocally(name) || (Parent is not null && Parent.IsDefined(name));
    }

    public bool IsDefinedLocally(string name)
    {
        return NamesDefinedInThisScope.Contains(name);
    }

    public VariableName DefineName(string name)
    {
        NamesDefinedInThisScope.Add(name);
        Debug.Assert(SearchName(name) == GetName(name));
        return GetName(name);
    }


    public static bool operator ==(Scope? @this, Scope? other)
    {
        return (@this is null && other is null) || (@this is not null && @this.Equals(other));
    }

    public static bool operator !=(Scope @this, Scope other)
    {
        return !(@this == other);
    }

    public bool Equals(Scope? other)
    {
        return other is not null &&
                Parent == other.Parent &&
                NamesDefinedInThisScope.Intersect(other.NamesDefinedInThisScope).Count()
                    == NamesDefinedInThisScope.Count;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Scope);
    }

    public override int GetHashCode()
    {
        return Parent is null
            ? 0
            : HashCode.Combine(Parent.GetHashCode(),
                NamesDefinedInThisScope.Aggregate(17, (x, y) => x ^ y.GetHashCode()),
                FunctionsDefinedInThisScope.GetHashCode());
    }

    private bool FunctionIsDefinedInThisScope(string name)
    {
        return FunctionsDefinedInThisScope.RegisteredFunctions.Any(x => x.Name == name);
    }

    public bool FunctionIsDefined(string name)
    {
        return FunctionIsDefinedInThisScope(name) || (Parent is not null && Parent.FunctionIsDefined(name));
    }

    public void DefineFunction(FunctionSignature functionSignature)
    {
        DefineName(functionSignature.Name);
        FunctionsDefinedInThisScope.RegisterFunction(functionSignature);

        DefineTypeOfName(GetName(functionSignature.Name), new GenericSmallLangType(TypeData.Void));
    }

    public FunctionSignature GetSignature(string name)
    {
        if (FunctionIsDefinedInThisScope(name)) return FunctionsDefinedInThisScope.GetSignature(name);

        if (Parent is null)
            throw new ArgumentException(
                $"Could not find function defined in the current or enclosing scope of name {name}");
        return Parent.GetSignature(name);
    }

    public FunctionID GetID(string name)
    {
        return GetSignature(name).ID;
    }

    public static FunctionID GetIDOfConstructorFunction(GenericSmallLangType type)
    {
        return new FunctionID(0);
    }

    public void DefineTypeOfName(VariableName variableName, GenericSmallLangType Type)
    {
        Debug.Assert(variableName is not null);
        TypeNameCombinationsDefinedInThisScope[variableName] = Type;
    }

    public bool TryGetTypeOfVariable(VariableName variableName, [NotNullWhen(true)] out GenericSmallLangType? type)
    {
        if (TypeNameCombinationsDefinedInThisScope.TryGetValue(variableName, out type)) return true;
        if (Parent is null) return false;
        return Parent.TryGetTypeOfVariable(variableName, out type);
    }
}