namespace SmallLang.Constants;

class TypeData
{
    public static readonly TypeData Data = new();
    public uint StringTypeCode = 1;
    public uint FloatTypeCode = 2;
    public uint IntTypeCode = 3;
    public uint DoubleTypeCode = 4;
    public uint NumberTypeCode = 5;
    public uint LongTypeCode = 6;
    public uint LongintTypeCode = 7;
    public uint ByteTypeCode = 8;
    public uint CharTypeCode = 9;
    public uint BooleanTypeCode = 10;
    public uint RationalTypeCode = 11;
    public int TypeCodeOffsetInHeader = 16;
}