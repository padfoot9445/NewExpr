using System.Collections.Immutable;
using System.Diagnostics;

namespace SmallLang.IR.LinearIR;

public record class TreeChunk
{
    public Chunk Self { get; init; }
    public List<TreeChunk> Children { get; init; }
    private static int NextUuid { get; set; }
    public int Uuid { get; init; }

    public TreeChunk() : this(new Chunk(), []) { }
    public int NumberOfChildren { get; init; }

    public TreeChunk(Chunk Self, IEnumerable<TreeChunk> Children, int? NumberOfChildren = null)
    {
        this.Children = Children.ToList();
        this.Self = Self;
        Uuid = NextUuid++;

        this.NumberOfChildren = Children.Count();
        if (NumberOfChildren is not null) Debug.Assert(this.NumberOfChildren == NumberOfChildren);
    }
}