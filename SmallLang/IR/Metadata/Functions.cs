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