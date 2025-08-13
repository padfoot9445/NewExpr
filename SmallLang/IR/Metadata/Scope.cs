namespace SmallLang.Metadata;

public struct Scope()
{
    Scope(HashSet<VariableName> Names) : this() { VariablesInScope = Names; }
    private HashSet<VariableName> VariablesInScope = [];
    public bool Contains(VariableName variableName) => VariablesInScope.Contains(variableName);
    public Scope ScopeUnion(Scope other)
    {
        return new(VariablesInScope.Union(other.VariablesInScope).ToHashSet());
    }
    public Scope Append(VariableName other)
    {
        return ScopeUnion(new([other]));
    }
    public override bool Equals(object? other) => other is Scope scope && VariablesInScope.Intersect(scope.VariablesInScope).Count() == VariablesInScope.Count;
    public static bool operator ==(Scope left, Scope right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Scope left, Scope right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    internal object Union(Scope? v)
    {
        throw new NotImplementedException();
    }
}