
namespace MEXP.IRs.LinearIR;
public record class Chunk : IIRComponent
{
    public List<IIRComponent> Values { get; }
    public Chunk(List<IIRComponent> values)
    {
        Values = values;
    }
    public Chunk(IEnumerable<IIRComponent> values)
    {
        Values = values.ToList();
    }
    public IEnumerable<IIRComponent> Flatten()
    {
        return Values;
    }
}
