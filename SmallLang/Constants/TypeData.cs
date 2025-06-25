using System.Text.Json;
using SmallLang.Metadata;

namespace SmallLang.Constants;

class TypeData
{
    readonly string[] types;
    private TypeData()
    {
        var filecontent = File.ReadAllText(@"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang\Constants\TypeData.json");
        using (var document = JsonDocument.Parse(filecontent))
        {

            types = document.RootElement.GetProperty("typenames").EnumerateArray().Select(x => x.GetString()).ToArray()!;
            VoidTypeCode = TypeCodeAndInitDictFromTypeIndex(0, document, GetTypeFromTypeName);
            StringTypeCode = TypeCodeAndInitDictFromTypeIndex(1, document, GetTypeFromTypeName);
            FloatTypeCode = TypeCodeAndInitDictFromTypeIndex(2, document, GetTypeFromTypeName);
            IntTypeCode = TypeCodeAndInitDictFromTypeIndex(3, document, GetTypeFromTypeName);
            DoubleTypeCode = TypeCodeAndInitDictFromTypeIndex(4, document, GetTypeFromTypeName);
            NumberTypeCode = TypeCodeAndInitDictFromTypeIndex(5, document, GetTypeFromTypeName);
            LongTypeCode = TypeCodeAndInitDictFromTypeIndex(6, document, GetTypeFromTypeName);
            LongintTypeCode = TypeCodeAndInitDictFromTypeIndex(7, document, GetTypeFromTypeName);
            ByteTypeCode = TypeCodeAndInitDictFromTypeIndex(8, document, GetTypeFromTypeName);
            CharTypeCode = TypeCodeAndInitDictFromTypeIndex(9, document, GetTypeFromTypeName);
            BooleanTypeCode = TypeCodeAndInitDictFromTypeIndex(10, document, GetTypeFromTypeName);
            RationalTypeCode = TypeCodeAndInitDictFromTypeIndex(11, document, GetTypeFromTypeName);
        }
    }
    SmallLangType TypeCodeAndInitDictFromTypeIndex(int ind, JsonDocument document, Dictionary<string, SmallLangType> d)
    {

        // string[] types = ["void", "string", "float", "int", "double", "number", "long", "longint", "byte", "char", "bool", "rational"];
        SmallLangType _out = new(document.RootElement.GetProperty(types[ind]).GetUInt32());
        d[types[ind]] = _out;
        return _out;
    }
    public static readonly TypeData Data = new();
    public readonly SmallLangType VoidTypeCode;
    public readonly SmallLangType StringTypeCode;
    public readonly SmallLangType FloatTypeCode;
    public readonly SmallLangType IntTypeCode;
    public readonly SmallLangType DoubleTypeCode;
    public readonly SmallLangType NumberTypeCode;
    public readonly SmallLangType LongTypeCode;
    public readonly SmallLangType LongintTypeCode;
    public readonly SmallLangType ByteTypeCode;
    public readonly SmallLangType CharTypeCode;
    public readonly SmallLangType BooleanTypeCode;
    public readonly SmallLangType RationalTypeCode;
    public readonly int TypeCodeOffsetInHeader = 16;

    public Dictionary<string, SmallLangType> GetTypeFromTypeName = new();
}