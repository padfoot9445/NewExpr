using Common.AST;

namespace SmallLangTest.EvaluatorTests;
record class Attributes(double? Value, bool? IsLiteral, double? RootValue) : IMetadata
{
    public Attributes() : this(null, null, null) { }
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        throw new NotImplementedException();
    }

    public void Merge(IMetadata other)
    {
        throw new NotImplementedException();
    }
    int GH<T>(T? v)
    {
        return v is null ? (int.MinValue + 1394) : v.GetHashCode();
    }
    public override int GetHashCode()
    {
        return GH(Value) + GH(IsLiteral) + GH(RootValue);
    }
    public virtual bool Equals(Attributes? att)
    {
        if (att is null) return false;
        return att!.Value == Value && att.IsLiteral == IsLiteral && att.RootValue == RootValue;
    }
}
