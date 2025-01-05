namespace IRs.ParseTree;
using Common.AST;
public class AttributeRecord : IMetadata
{
    public AttributeRecord(uint? TypeCode = null)
    {
        this._TypeCode = TypeCode;
    }
    private uint? _TypeCode;
    public uint? TypeCode => _TypeCode;
    bool Common.IIsEquivalentTo<IMetadata>.IsEquivalentTo(IMetadata other)
    {
        return IsEquivalentTo(other);
    }
    public bool IsEquivalentTo(IMetadata other)
    {
        if (other is AttributeRecord otherRecord)
        {
            return TypeCode == otherRecord.TypeCode;
        }
        return false;
    }
    private static void MergeField<T>(ref T fieldToBeSet, T otherValue, bool ThrowOnError = true, bool PrioritizeOther = false)
    {
        if (ThrowOnError && fieldToBeSet is not null)
        {
            throw new InvalidOperationException("Attempting to merge field that is not null");
        }
        else if (fieldToBeSet is null) //we can just set, since if this is null even if we prioritize this we still use other, and if both are null then setting changes nothing
        {
            fieldToBeSet = otherValue;
        }
        else //if (fieldToBeSet is not null)
        {
            if (PrioritizeOther && otherValue is not null) //if not prioritize other and we already have a value we can just leave it, and if we do prioritize but othervalue is null then we still leave it as value merged with null is value no matter priority
            {
                fieldToBeSet = otherValue;
            }
        }
    }
    private void ForceMergeField<T>(ref T fieldToBeSet, T othervalue, bool PrioritizeOther) => MergeField<T>(ref fieldToBeSet, othervalue, false, PrioritizeOther);
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        if (other is not AttributeRecord otherRecord)
        {
            throw new InvalidOperationException("Other must be of type AttributeRecord to be merged with AttributeRecord");
        }
        ForceMergeField(ref _TypeCode, otherRecord.TypeCode, PrioritizeOther);
    }
    public void Merge(IMetadata other)
    {
        if (other is not AttributeRecord otherRecord)
        {
            throw new InvalidOperationException("Other must be of type AttributeRecord to be merged with AttributeRecord");
        }
        MergeField(ref _TypeCode, otherRecord.TypeCode);
    }
}