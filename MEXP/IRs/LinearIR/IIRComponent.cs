namespace MEXP.IRs.LinearIR;
public interface IIRComponent
{
    public IEnumerable<IIRComponent> Flatten();
}