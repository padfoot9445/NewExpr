
namespace MEXP.IRs.LinearIR;
public readonly record struct LiteralContainer(uint Value) : ISingleValue
{

    public IEnumerable<IIRComponent> Flatten()
    {
        return [this];
    }
}
