using System.Text.Json;
using Common.Metadata;
namespace SmallLang.IR.Metadata;

partial class Functions
{
    public static readonly FunctionSignature<BackingNumberType, SmallLangType> Input = new(Name: "name", ID: new(1), RetVal: TypeData.Dict, ArgTypes: []);
    public static readonly Functions Values;
    public void RegisterFunction(FunctionSignature<BackingNumberType, SmallLangType> Signature)
    {
        __functions.Add(Signature);
    }
    private readonly List<FunctionSignature<BackingNumberType, SmallLangType>> __functions = new();
    public readonly List<FunctionSignature<BackingNumberType, SmallLangType>> Signatures = new();
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(string Name)
    {
        return __functions.Where(x => x.Name == Name).Single();
    }
    public FunctionSignature<BackingNumberType, SmallLangType> GetSignature(FunctionID<BackingNumberType> ID)
    {
        return __functions.Where(x => x.ID == ID).Single();
    }
}