using Common.Metadata;

namespace SmallLang.Metadata;

public record class FunctionSignature(string Name, FunctionID<BackingNumberType> ID, SmallLangType RetVal, List<SmallLangType> ArgTypes);