using SmallLang.Backend.CodeGenComponents;
using SmallLang.Metadata;

namespace SmallLang.Constants;

class FunctionMapper
{
    public static readonly FunctionMapper Mapper;
    static FunctionMapper()
    {
        Mapper = new FunctionMapper();
    }
    public Dictionary<FunctionID, List<SmallLangType>> FunctionToFunctionArgs = new()
    {
        [new(1)] = [],
        [new(2)] = [TypeData.Data.StringTypeCode]
    };
    public Dictionary<FunctionID, SmallLangType> FunctionToRetType = new()
    {
        [new(1)] = TypeData.Data.StringTypeCode,
        [new(2)] = TypeData.Data.VoidTypeCode,
    };
    public Dictionary<string, FunctionID> FunctionNameToFunctionID = new()
    {
        ["input"] = new(1),
        ["SOut"] = new(2),
    };
}