using System.Collections;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang.Frontend.CodeGen;

public record class Data
{
    public Chunks Sections { get; init; } = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> VariableSlots = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> StaticDataArea = new();
    public Dictionary<LoopGUID, (int ContinueDSTChunk, int BreakDSTChunk)> LoopData = new();
}