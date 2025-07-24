using System.Diagnostics;
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
        else if ((src == StringTypeCode || dst == StringTypeCode) && dst != src) return false;
        else if (src.IsRefType || dst.IsRefType)
        {
            if (src.IsRefType && dst.IsRefType) return src == dst;
            return IsNumber(src) && IsNumber(dst);
        }
        else return true; //since all other types are number types
    }
    public bool ImplicitCastTo(SmallLangType src, SmallLangType dst)
    {
        if (src == dst) return true;
        else if (dst == FloatTypeCode || dst == DoubleTypeCode)
        {
            return src == FloatTypeCode || dst == DoubleTypeCode;
        }
        else if (src == FloatTypeCode || src == DoubleTypeCode)
        {
            return dst == NumberTypeCode || dst == DoubleTypeCode || !(src == DoubleTypeCode && dst == FloatTypeCode);
        }
        else if (IsNumber(src) && IsNumber(dst))
        {
            Debug.Assert(src != FloatTypeCode && src != DoubleTypeCode);
            if (!src.IsRefType) return dst.Size >= src.Size || dst.IsRefType;
            else if (src == LongintTypeCode) return dst.IsRefType; //all ref numbers should be able to take a longint
            else if (src == RationalTypeCode) return dst == RationalTypeCode || dst == NumberTypeCode;
            else if (src == NumberTypeCode) return dst == NumberTypeCode;
        }
        else if (src == CharTypeCode && dst == StringTypeCode) return true;
        return false;
    }
    bool IsNumber(SmallLangType type)
    {
        return type.IsNum;
    }
    public static readonly TypeData Data = new();
    private static uint NextTypeCode = 1;
    private static uint GetTypeCode => NextTypeCode++;
    private readonly HashSet<uint> PointerTypes;
    SmallLangType[] Types;
    public readonly SmallLangType VoidTypeCode = new(GetTypeCode, "void");
    public readonly SmallLangType StringTypeCode = new(GetTypeCode, "string", true);
    public readonly SmallLangType FloatTypeCode = new(GetTypeCode, "float", IsNum: true);
    public readonly SmallLangType IntTypeCode = new(GetTypeCode, "int", IsNum: true);
    public readonly SmallLangType DoubleTypeCode = new(GetTypeCode, "double", Size: 2, IsNum: true);
    public readonly SmallLangType NumberTypeCode = new(GetTypeCode, "number", true, IsNum: true);
    public readonly SmallLangType LongTypeCode = new(GetTypeCode, "long", Size: 2, IsNum: true);
    public readonly SmallLangType LongintTypeCode = new(GetTypeCode, "longint", true, IsNum: true);
    public readonly SmallLangType ByteTypeCode = new(GetTypeCode, "byte", IsNum: true);
    public readonly SmallLangType CharTypeCode = new(GetTypeCode, "char", IsNum: true);
    public readonly SmallLangType BooleanTypeCode = new(GetTypeCode, "boolean", IsNum: true);
    public readonly SmallLangType RationalTypeCode = new(GetTypeCode, "rational", true, IsNum: true);
    public readonly SmallLangType ArrayTypeCode = new(GetTypeCode, "array", true);
    public readonly SmallLangType ListTypeCode = new(GetTypeCode, "list", true);
    public readonly SmallLangType SetTypeCode = new(GetTypeCode, "set", true);
    public readonly SmallLangType DictTypeCode = new(GetTypeCode, "dict", true);
    public readonly int TypeCodeOffsetInHeader = 16;

    public Dictionary<string, SmallLangType> GetTypeFromTypeName;
}