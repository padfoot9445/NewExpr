namespace Common.Transformers.ASTTransformers;
public class TypeCodeProvider
{
    private protected Dictionary<string, uint> Dict = new Dictionary<string, uint>();
    private protected uint Current = 0;
    public uint GetTypeCode(string TypeName)
    {
        if (Dict.TryGetValue(TypeName, out uint val))
        {
            return val;
        }
        Dict[TypeName] = Current++;
        return Current;
    }

}