using SmallLang.Metadata;

namespace SmallLang.Constants;

class TypeData
{
    public static readonly TypeData Data = new();
    public SmallLangType VoidTypeCode = new(0);
    public SmallLangType StringTypeCode = new(1);
    public SmallLangType FloatTypeCode = new(2);
    public SmallLangType IntTypeCode = new(3);
    public SmallLangType DoubleTypeCode = new(4);
    public SmallLangType NumberTypeCode = new(5);
    public SmallLangType LongTypeCode = new(6);
    public SmallLangType LongintTypeCode = new(7);
    public SmallLangType ByteTypeCode = new(8);
    public SmallLangType CharTypeCode = new(9);
    public SmallLangType BooleanTypeCode = new(10);
    public SmallLangType RationalTypeCode = new(11);
    public int TypeCodeOffsetInHeader = 16;
}