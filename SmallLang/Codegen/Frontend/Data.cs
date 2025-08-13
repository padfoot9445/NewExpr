using System.Collections;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

public record class Data
{
    public Chunks Sections { get; init; } = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> VariableSlots = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> StaticDataArea = new();
    public Dictionary<LoopGUID, (GenericNumberWrapper<int> ContinueDSTChunk, GenericNumberWrapper<int> BreakDSTChunk)> LoopData = new();
}