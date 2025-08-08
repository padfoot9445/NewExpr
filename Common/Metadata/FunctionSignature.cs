using System.Numerics;
using Common.Metadata;

namespace SmallLang.Metadata;

public record class FunctionSignature<T, TType>(string Name, FunctionID<T> ID, IMetadataTypes<TType> RetVal, List<IMetadataTypes<TType>> ArgTypes)
where T : IBinaryInteger<T>, IMinMaxValue<T>
where TType : IMetadataTypes<TType>;