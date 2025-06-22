using Common.AST;

namespace SmallLang.Metadata;

public record class Attributes(List<SmallLangType>? DeclArgumentTypes = null, FunctionID? FunctionID = null, SmallLangType? TypeOfExpression = null) : IMetadata
{
    public Attributes() : this(DeclArgumentTypes: null) { }
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        throw new NotImplementedException();
    }

    public bool IsEquivalentTo(IMetadata other)
    {
        throw new NotImplementedException();
    }

    public void Merge(IMetadata other)
    {
        throw new NotImplementedException();
    }
}