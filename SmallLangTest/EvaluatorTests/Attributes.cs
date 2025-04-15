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
}
