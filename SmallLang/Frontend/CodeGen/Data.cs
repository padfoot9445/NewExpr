using System.Collections;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.LinearIR;

namespace SmallLang.Frontend.CodeGen;

public record class Data
{
    public Chunks Sections { get; init; } = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> VariableSlots = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> StaticDataArea = new();
}