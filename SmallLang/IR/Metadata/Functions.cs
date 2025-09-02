using System.Diagnostics;
using System.Text.Json;
using Common.Metadata;
namespace SmallLang.IR.Metadata;

partial class Functions
{
    public Functions()
    {

    }
    public void RegisterFunction(FunctionSignature<BackingNumberType, SmallLangType> Signature)
    {
        if (RegisteredFunctions.Any(x => x.Name == Signature.Name))
        {
            if (!(RegisteredFunctions.Where(x => x.Name == Signature.Name).Select(x => x == Signature).All(x => x)))
            {
                throw new ArgumentOutOfRangeException(string.Join('\n', RegisteredFunctions.Where(x => x.Name == Signature.Name && (x != Signature)).Select(x => x.ToString())) + $"Had the same name as {Signature} but were not the same.");
            }
            return;
        }

        RegisteredFunctions.Add(Signature);
    }
    public HashSet<FunctionSignature<BackingNumberType, SmallLangType>> RegisteredFunctions { get; } = new();
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(string Name)
    {
        return RegisteredFunctions.Single(x => x.Name == Name);
    }
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(FunctionID<BackingNumberType> ID)
    {
        return RegisteredFunctions.Single(x => x.ID == ID);
    }
}