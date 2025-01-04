namespace IRs.ParseTree;
using Common.AST;
public record class AttributeRecord(uint typecode) : IMetadata
{
    public uint Typecode { get; init; } = typecode;
}