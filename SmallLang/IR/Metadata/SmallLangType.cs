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

    public SmallLangType GreatestCommonType(SmallLangType other)
    {
        if (this == other) return this;
        if ((this == TypeData.Char && other == TypeData.String) || (this == TypeData.String && other == TypeData.Char))
            return TypeData.String;

        if (IsNum && other.IsNum)
        {
            if (IsRefType != other.IsRefType) //IsRefType XOR other.IsRefType
                return IsRefType ? this : other; //return the one that is a ref type

            if ((IsRefType || other.IsRefType) is false)
            {
                if (NumberType != other.NumberType) return TypeData.Number;
                return TypeData.AllTypes
                    .Where(x =>
                        x is { IsNum: true, IsRefType: false }
                        && x.NumberType == NumberType &&
                        !(x == TypeData.Char || x == TypeData.Bool)
                    )
                    .OrderBy(x => x.ValMaxSize).First();
            }

            Debug.Assert(IsRefType && other.IsRefType);
            List<SmallLangType> PointerTypesHierarchy = [TypeData.Longint, TypeData.Rational, TypeData.Number];

            return PointerTypesHierarchy.IndexOf(this) > PointerTypesHierarchy.IndexOf(other) ? this : other;
        }

        throw new ArgumentException(
            "Could not find a Greatest Common Type between two types which were not both numeric and were not both the same.");
    }
}