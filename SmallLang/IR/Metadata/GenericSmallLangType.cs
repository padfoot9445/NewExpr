using System.Diagnostics;
using System.Text;
using Common.AST;
using Common.Metadata;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public record class GenericSmallLangType : GenericNumberWrapper<byte>, ITreeNode<GenericSmallLangType>,
    IMetadataTypes<GenericSmallLangType>
{
    public GenericSmallLangType(SmallLangType OutmostType, params IEnumerable<GenericSmallLangType> TypeArguments) :
        base(OutmostType.BackingValue)
    {
        this.OutmostType = OutmostType;
        ChildNodes = TypeArguments.ToList();
    }

    public SmallLangType OutmostType { get; init; }
    public bool IsLeafNode => ChildNodes.Any() is false;

    public uint Size
    {
        get => OutmostType.Size;
        init => _ = value;
    }

    bool IMetadataTypes<GenericSmallLangType>.CanDeclareTo(GenericSmallLangType other)
    {
        return OutmostType.CanDeclareTo(other.OutmostType);
    }

    bool IMetadataTypes<GenericSmallLangType>.ImplicitCast(GenericSmallLangType other)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GenericSmallLangType> ChildNodes { get; init; }

    public static GenericSmallLangType GreatestCommonType(GenericSmallLangType self, GenericSmallLangType other)
    {
        if (self == other) return self;
        if (self.ChildNodes.Any() || other.ChildNodes.Any())
            throw new ArgumentException("Cannot find gct of generic types which are not the same");

        return new GenericSmallLangType(self.OutmostType.GreatestCommonType(other.OutmostType));
    }

    public GenericSmallLangType GreatestCommonType(GenericSmallLangType other)
    {
        return GreatestCommonType(this, other);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(OutmostType);
        foreach (var type in ChildNodes) hash.Add(type);
        return hash.ToHashCode();
    }

    public static GenericSmallLangType ParseType(ITypeNode type)
    {
        if (type is BaseTypeNode BT) return new GenericSmallLangType(TypeData.GetType(BT.Data.Lexeme));
        return type is GenericTypeNode GT
            ? new GenericSmallLangType(TypeData.GetType(GT.Data.Lexeme),
            [
                ..GT.Types.Select(ParseType)
            ])
            : throw new ArgumentException("type contained unknown nodetype");
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
        var HitBracket = false;
        for (var i = 0; i < type.Length; i++)
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
        var bracketsNumber = 0;
        StringBuilder builder = new();

        for (var i = 2; i < csv.Length - 2; i++)
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

}