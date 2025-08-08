using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class Pointer<TBacking>(int BackingValue) : NumberWrapper<int, TBacking>(BackingValue)
where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>
{
}
