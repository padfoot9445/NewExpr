
namespace MEXP.IRs.LinearIR;
public class OpCode : ISingleValue
{
    public uint Value { get; }
    public string Name { get; }

    public OpCode(uint value, string name)
    {
        Value = value;
        Name = name;
    }
    public OpCode(string name)
    {
        Value = NextVal;
        Name = name;
    }
    static uint Current = 1;
    static uint NextVal { get => Current++; }

    public IEnumerable<IIRComponent> Flatten()
    {
        return [this];
    }
}