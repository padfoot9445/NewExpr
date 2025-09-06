using System.Collections.ObjectModel;
using System.Numerics;

namespace Common.Metadata;

public record class FunctionSignature<T, TType>(
    string Name,
    FunctionID<T> ID,
    TType RetVal,
    ReadOnlyCollection<TType> ArgTypes)
    where T : IBinaryInteger<T>, IMinMaxValue<T>
    where TType : IMetadataTypes<TType>
{
    public FunctionSignature(string Name, FunctionID<T> ID, TType RetVal, List<TType> ArgTypes) : this(Name, ID, RetVal,
        ArgTypes.AsReadOnly())
    {
    }

    public virtual bool Equals(FunctionSignature<T, TType>? Other)
    {
        return Other is not null && Name == Other.Name && ID == Other.ID && RetVal.Equals(Other.RetVal) &&
               ArgTypes.Count == Other.ArgTypes.Count &&
               ArgTypes.Zip(Other.ArgTypes).Select(x => x.First.Equals(x.Second)).All(x => x);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name.GetHashCode(), ID.GetHashCode(), RetVal.GetHashCode(),
            ArgTypes.Aggregate(487, (x, y) => x * 13 + 17 + y.GetHashCode()));
    }
}