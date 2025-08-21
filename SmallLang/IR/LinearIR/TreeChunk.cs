using System.Collections.Immutable;
using System.Diagnostics;

namespace SmallLang.IR.LinearIR;

public record class TreeChunk(Chunk Self, List<TreeChunk> Children)
{
    public TreeChunk() : this(new Chunk(), []) { }
    public int NumberOfChildren { get; init; }

    public TreeChunk(Chunk Self, IEnumerable<TreeChunk> Children, int? NumberOfChildren = null) : this(Self, Children.ToList())
    {
        this.NumberOfChildren = Children.Count();
        if (NumberOfChildren is not null) Debug.Assert(this.NumberOfChildren == NumberOfChildren);
    }
}