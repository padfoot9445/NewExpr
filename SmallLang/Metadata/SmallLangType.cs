namespace SmallLang.Metadata;

public record class SmallLangType(uint Value, string Name, bool IsRefType = false, uint Size = 1) : BaseUIntWrapper(Value);