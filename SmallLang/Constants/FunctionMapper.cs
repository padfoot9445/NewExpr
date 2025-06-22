using SmallLang.Backend.CodeGenComponents;

namespace SmallLang.Constants;

class FunctionMapper
{
    public static readonly FunctionMapper Mapper;
    static FunctionMapper()
    {
        Mapper = new FunctionMapper();
    }
    public Dictionary<uint, List<uint>> FunctionToFunctionArgs = new()
    {
        [1] = [],
        [2] = [BaseCodeGenComponent.StringTypeCode]
    };
    public Dictionary<uint, uint> FunctionToRetType = new()
    {
        [1] = 1,
        [2] = 0,
    };
    public Dictionary<string, uint> FunctionNameToFunctionID = new()
    {
        ["input"] = 1,
        ["SOut"] = 2,
    };
}