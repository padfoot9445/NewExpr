using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public record class RelativeChunkPointer : GenericNumberWrapper<int>
{
    protected RelativeChunkPointer(GenericNumberWrapper<int> original) : base(original)
    {
    }
    public RelativeChunkPointer(int Val) : base(Val)
    {
    }
}