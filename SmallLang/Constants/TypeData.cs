using System.Text.Json;
using SmallLang.Metadata;

namespace SmallLang.Constants;

public class TypeData
{
    readonly string[] types;
    private TypeData()
    {
        Types = [VoidTypeCode, StringTypeCode, FloatTypeCode, IntTypeCode, DoubleTypeCode, NumberTypeCode, LongTypeCode, LongintTypeCode, ByteTypeCode, CharTypeCode, BooleanTypeCode, RationalTypeCode, ArrayTypeCode, ListTypeCode, SetTypeCode, DictTypeCode];
        PointerTypes = Types.Select(x => x).Where(x => x.IsRefType).Select(x => x.Value).ToHashSet();
        types = Types.Select(x => x.Name).ToArray();
        GetTypeFromTypeName = Types.ToDictionary(x => x.Name);
    }
    public bool IsPointerType(SmallLangType type)
    {
        return PointerTypes.Contains(type.Value);
    }
    public bool CanDeclareTo(SmallLangType dst, SmallLangType src)
    {
        if (src == VoidTypeCode || dst == VoidTypeCode) return false;
        if ((src == StringTypeCode || dst == StringTypeCode) && dst != src) return false;
        if (src.IsRefType || dst.IsRefType) return false;
        return true; //since all other types are number types
    }

    public static readonly TypeData Data = new();
    private static uint NextTypeCode = 1;
    private static uint GetTypeCode => NextTypeCode++;
    private readonly HashSet<uint> PointerTypes;
    SmallLangType[] Types;
    public readonly SmallLangType VoidTypeCode = new(GetTypeCode, "void");
    public readonly SmallLangType StringTypeCode = new(GetTypeCode, "string", true);
    public readonly SmallLangType FloatTypeCode = new(GetTypeCode, "float");
    public readonly SmallLangType IntTypeCode = new(GetTypeCode, "int");
    public readonly SmallLangType DoubleTypeCode = new(GetTypeCode, "double", Size: 2);
    public readonly SmallLangType NumberTypeCode = new(GetTypeCode, "number", true);
    public readonly SmallLangType LongTypeCode = new(GetTypeCode, "long", Size: 2);
    public readonly SmallLangType LongintTypeCode = new(GetTypeCode, "longint", true);
    public readonly SmallLangType ByteTypeCode = new(GetTypeCode, "byte");
    public readonly SmallLangType CharTypeCode = new(GetTypeCode, "char");
    public readonly SmallLangType BooleanTypeCode = new(GetTypeCode, "boolean");
    public readonly SmallLangType RationalTypeCode = new(GetTypeCode, "rational", true);
    public readonly SmallLangType ArrayTypeCode = new(GetTypeCode, "array", true);
    public readonly SmallLangType ListTypeCode = new(GetTypeCode, "list", true);
    public readonly SmallLangType SetTypeCode = new(GetTypeCode, "set", true);
    public readonly SmallLangType DictTypeCode = new(GetTypeCode, "dict", true);
    public readonly int TypeCodeOffsetInHeader = 16;

    public Dictionary<string, SmallLangType> GetTypeFromTypeName;
}