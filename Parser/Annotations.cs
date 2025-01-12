using Common.AST;

namespace Parser;
public class Annotations : IMetadata
{
    public Annotations()
    {

    }
    public Annotations(uint? TypeCode = null, bool IsEmpty = false, uint? TypeDenotedByIdentifier = null, bool CanBeResolvedToAssignable = false)
    {
        this.TypeCode = TypeCode;
        this.IsEmpty = IsEmpty;
        this.TypeDenotedByIdentifier = TypeDenotedByIdentifier; // as in, in "int x = 0", the "int" node literal would have have this set; also used for custom identifiers but we don't have that
        this.CanBeResolvedToAssignable = CanBeResolvedToAssignable;
    }
    public uint? TypeCode { get; } = null;
    public bool IsEmpty { get; } = false;
    public uint? TypeDenotedByIdentifier { get; } = null;
    public bool CanBeResolvedToAssignable { get; } = false;
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
    public Annotations Copy()
    {
        throw new NotImplementedException();
    }
}