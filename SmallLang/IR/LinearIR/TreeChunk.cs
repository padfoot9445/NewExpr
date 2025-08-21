using System.Collections.Immutable;
using System.Diagnostics;

namespace SmallLang.IR.LinearIR;

public record class TreeChunk(Chunk Self, ImmutableArray<TreeChunk> Children)
{
    public int NumberOfChildren { get; init; }

    public TreeChunk(Chunk Self, IEnumerable<TreeChunk> Children, int? NumberOfChildren = null) : this(Self, Children.ToImmutableArray())
    {
        this.NumberOfChildren = Children.Count();
        if (NumberOfChildren is not null) Debug.Assert(this.NumberOfChildren == NumberOfChildren);
    }
}