using Common.Transformers.ASTTransformers;
namespace Transformers.ASTTransformers;
public class MEXPTypeCodeProvider : TypeCodeProvider
{
    public MEXPTypeCodeProvider()
    {
        IntTypeCode = GetTypeCode(Int);
        FloatTypeCode = GetTypeCode(Float);
        DoubleTypeCode = GetTypeCode(Double);
        NumberTypeCode = GetTypeCode(Number);
        LongTypeCode = GetTypeCode(Long);
        LongintTypeCode = GetTypeCode(Longint);
        ByteTypeCode = GetTypeCode(Byte);
    }
    const string Int = "int";
    const string Float = "float";
    const string Double = "double";
    const string Number = "number";
    const string Long = "long";
    const string Longint = "longint";
    const string Byte = "byte";
    public uint IntTypeCode { get; init; }
    public uint FloatTypeCode { get; init; }
    public uint DoubleTypeCode { get; init; }
    public uint NumberTypeCode { get; init; }
    public uint LongTypeCode { get; init; }
    public uint LongintTypeCode { get; init; }
    public uint ByteTypeCode { get; init; }
}

