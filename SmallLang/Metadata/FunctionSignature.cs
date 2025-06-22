namespace SmallLang.Metadata;

public record class FunctionSignature(string Name, FunctionID ID, SmallLangType RetVal, List<SmallLangType> ArgTypes);