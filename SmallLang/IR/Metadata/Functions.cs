using System.Text.Json;
using Common.Metadata;
using SmallLang.Metadata;

namespace SmallLang.IR.Metadata;

class Functions
{
    private Functions()
    {
        List<FunctionSignature<BackingNumberType, SmallLangType>> signatures = new();
        ReadJson(signatures);
    }
    static void ReadJson(List<FunctionSignature<BackingNumberType, SmallLangType>> Signatures)
    {
        var filecontent = File.ReadAllText(@"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang\Constants\Functions.json");
        using (var document = JsonDocument.Parse(filecontent))
        {
            JsonElement functionsarray = document.RootElement.GetProperty("Functions");
            foreach (JsonElement function in functionsarray.EnumerateArray())
            {
                Signatures.Add(GetFSFromJson(function));
            }
        }
    }
    static FunctionSignature<BackingNumberType, SmallLangType> GetFSFromJson(JsonElement FE)
    {
        var Args = FE.GetProperty("arguments").EnumerateArray().Select(x => TypeData.Data.GetTypeFromTypeName[x.GetString()!]).ToList();
        return new(
            Name: FE.GetProperty("name").GetString()!,
            ID: new(FE.GetProperty("ID").GetUInt32()),
            RetVal: TypeData.Data.GetTypeFromTypeName[FE.GetProperty("returns").GetString()!],
            ArgTypes: Args.Select(x => (IMetadataTypes<SmallLangType>)x).ToList()
        );
    }
    public static readonly Functions Values;
    static Functions()
    {
        Values = new Functions();
    }
    public readonly List<FunctionSignature<BackingNumberType, SmallLangType>> Signatures = new();
    public Dictionary<FunctionID<BackingNumberType>, List<SmallLangType>> FunctionToFunctionArgs = new()
    {
        [new(1)] = [],
        [new(2)] = [TypeData.Data.StringTypeCode]
    };
    public Dictionary<FunctionID<BackingNumberType>, SmallLangType> FunctionToRetType = new()
    {
        [new(1)] = TypeData.Data.StringTypeCode,
        [new(2)] = TypeData.Data.VoidTypeCode,
    };
    public Dictionary<string, FunctionID<BackingNumberType>> FunctionNameToFunctionID = new()
    {
        ["input"] = new(1),
        ["SOut"] = new(2),
    };
}