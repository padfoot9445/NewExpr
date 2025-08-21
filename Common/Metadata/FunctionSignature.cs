using System.Numerics;
using Common.Metadata;

namespace Common.Metadata;

public record class FunctionSignature<T, TType>(string Name, FunctionID<T> ID, TType RetVal, List<TType> ArgTypes)
where T : IBinaryInteger<T>, IMinMaxValue<T>
where TType : IMetadataTypes<TType>;