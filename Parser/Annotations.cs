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
    public uint? TypeCode = null;
    public bool IsEmpty = false;
    public uint? TypeDenotedByIdentifier = null;
    public bool CanBeResolvedToAssignable = false;
    void ForceMergeField<T>(ref T Property, T Value, bool PrioritizeOther)
    {
        if (PrioritizeOther)
        {
            if (Value is null)
            {
                return;
                //do nothing
            }
            Property = Value;
        }
        else
        {
            if (Property is null)
            {
                Property = Value;
            }
            else
            {
                return;
                //do nothing
            }
        }
    }
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        if (other is Annotations otherAnnotations)
        {
            ForceMergeField(ref TypeCode, otherAnnotations.TypeCode, PrioritizeOther);
            ForceMergeField(ref IsEmpty, otherAnnotations.IsEmpty, PrioritizeOther);
            ForceMergeField(ref TypeDenotedByIdentifier, otherAnnotations.TypeDenotedByIdentifier, PrioritizeOther);
            ForceMergeField(ref CanBeResolvedToAssignable, otherAnnotations.CanBeResolvedToAssignable, PrioritizeOther);
        }
        else
        {
            throw new InvalidOperationException("Other must be of type Annotations to be merged with Annotations");
        }
    }

    public bool IsEquivalentTo(IMetadata other)
    {
        return (
            other is Annotations annotations &&
            TypeCode == annotations.TypeCode &&
            IsEmpty == annotations.IsEmpty &&
            TypeDenotedByIdentifier == annotations.TypeDenotedByIdentifier &&
            CanBeResolvedToAssignable == annotations.CanBeResolvedToAssignable
        );
    }

    public void Merge(IMetadata other)
    {
        throw new NotImplementedException();
    }
    public Annotations Copy()
    {
        return new(
            TypeCode: TypeCode,
            IsEmpty: IsEmpty,
            TypeDenotedByIdentifier: TypeDenotedByIdentifier,
            CanBeResolvedToAssignable: CanBeResolvedToAssignable
        );
    }
}