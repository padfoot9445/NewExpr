using SmallLang.Metadata;

namespace SmallLang.Constants;

class TypeData
{
    private TypeData()
    {

    }
    public static readonly TypeData Data = new();
    public readonly SmallLangType VoidTypeCode = new(0);
    public readonly SmallLangType StringTypeCode = new(1);
    public readonly SmallLangType FloatTypeCode = new(2);
    public readonly SmallLangType IntTypeCode = new(3);
    public readonly SmallLangType DoubleTypeCode = new(4);
    public readonly SmallLangType NumberTypeCode = new(5);
    public readonly SmallLangType LongTypeCode = new(6);
    public readonly SmallLangType LongintTypeCode = new(7);
    public readonly SmallLangType ByteTypeCode = new(8);
    public readonly SmallLangType CharTypeCode = new(9);
    public readonly SmallLangType BooleanTypeCode = new(10);
    public readonly SmallLangType RationalTypeCode = new(11);
    public readonly int TypeCodeOffsetInHeader = 16;

    public Dictionary<string, SmallLangType> GetTypeFromTypeName => __gtftni ?? (__gtftni = new()
    {
        ["void"] = Data.VoidTypeCode,
        ["string"] = Data.StringTypeCode,
        ["float"] = Data.FloatTypeCode,
        ["int"] = Data.IntTypeCode,
        ["double"] = Data.DoubleTypeCode,
        ["number"] = Data.NumberTypeCode,
        ["long"] = Data.LongTypeCode,
        ["longint"] = Data.LongintTypeCode,
        ["byte"] = Data.ByteTypeCode,
        ["char"] = Data.CharTypeCode,
        ["bool"] = Data.BooleanTypeCode,
        ["rational"] = Data.RationalTypeCode,
    });
    public Dictionary<string, SmallLangType>? __gtftni = null;
}