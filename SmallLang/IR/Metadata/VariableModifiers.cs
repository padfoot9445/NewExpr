namespace SmallLang.IR.Metadata;

[Flags]
public enum VariableModifiers : byte
{
    None = 0,
    Ref = 1,
    Readonly = 1 << 1,
    Frozen = 1 << 2,
    Immut = 1 << 3,
    Copy = 1 << 4
}