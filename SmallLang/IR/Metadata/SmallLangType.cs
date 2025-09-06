using System.Diagnostics;
using Common.Metadata;
using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public record class SmallLangType(
    uint BaseValue,
    string Name,
    bool IsRefType = false,
    uint Size = 1,
    bool IsNum = false,
    NumberType NumberType = NumberType.None,
    bool IsCollection = false,
    int? ValMaxSize = null) : GenericNumberWrapper<BackingNumberType>((BackingNumberType)BaseValue),
    IMetadataTypes<SmallLangType>
{
    public bool CanDeclareTo(SmallLangType other)
    {
        return TypeData.CanDeclareTo(this, other);
    }

    public bool ImplicitCast(SmallLangType other)
    {
        return TypeData.ImplicitCastTo(this, other);
    }

    private static SmallLangType GCTOfValueNumbers(SmallLangType self, SmallLangType other)
    {
        Debug.Assert(self.IsRefType is false && other.IsRefType is false); //NOSONAR
        if (self.NumberType != other.NumberType) return TypeData.Number;
        return TypeData.AllTypes
            .Where(x =>
                x is { IsNum: true, IsRefType: false }
                && x.NumberType == self.NumberType &&
                !(x == TypeData.Char || x == TypeData.Bool)
            )
            .OrderBy(x => x.ValMaxSize).First();
    }
    private static SmallLangType GCTOfRefNumbers(SmallLangType self, SmallLangType other)
    {
        Debug.Assert(self.IsRefType && other.IsRefType);
        List<SmallLangType> PointerTypesHierarchy = [TypeData.Longint, TypeData.Rational, TypeData.Number];

        return PointerTypesHierarchy.IndexOf(self) > PointerTypesHierarchy.IndexOf(other) ? self : other;
    }
    private static bool TypesAreStringOrChar(SmallLangType T1, SmallLangType T2) =>
        (T1 == TypeData.Char && T2 == TypeData.String) || (T1 == TypeData.String && T2 == TypeData.Char);
    public SmallLangType GreatestCommonType(SmallLangType other)
    {
        if (this == other) return this;


        else if (TypesAreStringOrChar(this, other)) return TypeData.String;


        else if (!IsNum || !other.IsNum) throw new ArgumentException("Could not find a Greatest Common Type between two types which were not both numeric and were not both the same.");


        //if IsRefType XOR other.IsRefType: return the one that is a ref type
        else if (IsRefType != other.IsRefType) return IsRefType ? this : other;

        else if (!(IsRefType || other.IsRefType)) return GCTOfValueNumbers(this, other);

        else return GCTOfRefNumbers(this, other);
    }
}