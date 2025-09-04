using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Common.AST;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public interface ISmallLangType
{
    uint BaseValue { get; init; }
    string Name { get; init; }
    bool IsRefType { get; init; }
    uint Size { get; init; }
    bool IsNum { get; init; }
    NumberType NumberType { get; init; }
    bool IsCollection { get; init; }
    int? ValMaxSize { get; init; }

    bool CanDeclareTo(SmallLangType other);

    void Deconstruct(out uint BaseValue, out string Name, out bool IsRefType, out uint Size, out bool IsNum,
        out NumberType NumberType, out bool IsCollection, out int? ValMaxSize);

    bool Equals(object? obj);
    bool Equals(GenericNumberWrapper<byte>? other);
    bool Equals(SmallLangType? other);
    int GetHashCode();
    bool ImplicitCast(SmallLangType other);
    string ToString();
}

public record class GenericSmallLangType : GenericNumberWrapper<byte>, ITreeNode<GenericSmallLangType>,
    IMetadataTypes<GenericSmallLangType>, ISmallLangType
{

    
    public GenericSmallLangType(SmallLangType OutmostType, params IEnumerable<GenericSmallLangType> TypeArguments) :
        base(OutmostType.BackingValue)
    {
        this.OutmostType = OutmostType;
        ChildNodes = TypeArguments.ToList();
    }

    public SmallLangType OutmostType { get; init; }
    public IEnumerable<GenericSmallLangType> ChildNodes { get; init; }
    public bool IsLeafNode => ChildNodes.Any() is false;
    public uint BaseValue
    {
        get => OutmostType.BaseValue;
        init => _ = value;
    }

    public string Name
    {
        get => OutmostType.Name;
        init => _ = value;
    }

    public bool IsRefType
    {
        get => OutmostType.IsRefType;
        init => _ = value;
    }

    public uint Size
    {
        get => OutmostType.Size;
        init => _ = value;
    }

    public bool IsNum
    {
        get => OutmostType.IsNum;
        init => _ = value;
    }

    public NumberType NumberType
    {
        get => OutmostType.NumberType;
        init => _ = value;
    }

    public bool IsCollection
    {
        get => OutmostType.IsCollection;
        init => _ = value;
    }

    public int? ValMaxSize
    {
        get => OutmostType.ValMaxSize;
        init => _ = value;
    }

    bool IMetadataTypes<GenericSmallLangType>.CanDeclareTo(GenericSmallLangType other)
    {
        return this.OutmostType.CanDeclareTo(other.OutmostType);
    }

    bool IMetadataTypes<GenericSmallLangType>.ImplicitCast(GenericSmallLangType other)
    {
        throw new NotImplementedException();
    }
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(OutmostType);
        foreach (var type in ChildNodes)
        {
            hash.Add(type);
        }
        return hash.ToHashCode();
    }
    public static GenericSmallLangType ParseType(ITypeNode type)
    {
        if (type is BaseTypeNode BT) return new(TypeData.GetType(BT.Data.Lexeme));
        else
        {
            return type is GenericTypeNode GT
                ? new GenericSmallLangType(TypeData.GetType(GT.Data.Lexeme),
                [
                    ..GT.Types.Select(ParseType)
                ])
                : throw new ArgumentException("type contained unknown nodetype");
        }
    }


    public static GenericSmallLangType ParseType(string type)
    {
        var (frontType, csv) = ParseTypePrivate(type);
        return new GenericSmallLangType(frontType, csv);
    }

    private static (SmallLangType, IEnumerable<GenericSmallLangType>) ParseTypePrivate(string type)
    {
        var (outType, remnant) = ParseTypeFront(type);
        return (outType, ParseTypeCSV(remnant));
    }

    private static (SmallLangType, string) ParseTypeFront(string type)
    {
        StringBuilder front = new();
        StringBuilder back = new();
        bool HitBracket = false;
        for (int i = 0; i < type.Length; i++)
        {
            if ($"{type[i]}{type.ElementAtOrDefault(i + 1)}" == "<[") HitBracket = true;

            if (HitBracket) back.Append(type[i]);
            else front.Append(type[i]);
        }

        return (TypeData.GetType(front.ToString()), back.ToString());
    }

    private static IEnumerable<GenericSmallLangType> ParseTypeCSV(string csv)
    {
        if (csv.Length == 0) yield break;
        Debug.Assert(csv[..2] == "<[" && csv[^2..] == "]>");
        int bracketsNumber = 0;
        StringBuilder builder = new();

        for (int i = 2; i < (csv.Length - 2); i++)
        {
            if (csv[i] == ',' && bracketsNumber == 0)
            {
                yield return ParseType(builder.ToString());
                builder.Clear();
            }
            else
            {
                switch (csv[i..(i + 2)])
                {
                    case "<[": bracketsNumber++; break;
                    case "]>": bracketsNumber--; break;
                }

                builder.Append(csv[i]);
            }
        }
        yield break;
    }

    public bool CanDeclareTo(SmallLangType other)
    {
        throw new NotImplementedException();
    }

    public void Deconstruct(out uint BaseValue, out string Name, out bool IsRefType, out uint Size, out bool IsNum,
        out NumberType NumberType, out bool IsCollection, out int? ValMaxSize)
    {
        throw new NotImplementedException();
    }

    public bool Equals(SmallLangType? other)
    {
        throw new NotImplementedException();
    }

    public bool ImplicitCast(SmallLangType other)
    {
        throw new NotImplementedException();
    }
}