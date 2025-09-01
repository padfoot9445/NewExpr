using System.Text.Json;
using Common.Metadata;
namespace SmallLang.IR.Metadata;

partial class Functions
{
    public void RegisterFunction(FunctionSignature<BackingNumberType, SmallLangType> Signature)
    {
        RegisteredFunctions.Add(Signature);
    }
    public List<FunctionSignature<BackingNumberType, SmallLangType>> RegisteredFunctions { get; } = new();
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(string Name)
    {
        return RegisteredFunctions.Where(x => x.Name == Name).Single();
    }
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(FunctionID<BackingNumberType> ID)
    {
        return RegisteredFunctions.Where(x => x.ID == ID).Single();
    }
}