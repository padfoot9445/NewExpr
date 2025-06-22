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
    public Dictionary<uint, List<SmallLangType>> FunctionToFunctionArgs = new()
    {
        [1] = [],
        [2] = [TypeData.Data.StringTypeCode]
    };
    public Dictionary<uint, SmallLangType> FunctionToRetType = new()
    {
        [1] = TypeData.Data.StringTypeCode,
        [2] = TypeData.Data.VoidTypeCode,
    };
    public Dictionary<string, uint> FunctionNameToFunctionID = new()
    {
        ["input"] = 1,
        ["SOut"] = 2,
    };
}