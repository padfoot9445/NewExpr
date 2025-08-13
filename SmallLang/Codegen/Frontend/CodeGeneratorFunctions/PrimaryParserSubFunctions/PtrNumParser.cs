using System.Diagnostics;
using Common.Dispatchers;
using Common.Metadata;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
using static SmallLang.IR.LinearIR.Opcode;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;

internal static class PtrNumParser
{
    public static void Parse(Node Self, CodeGenerator Driver)
    {
        Driver.Emit
        (
            Push,
            Self.Switch
            (
                x => x.Attributes.TypeOfExpression!,
                Comparer: (x, y) => x == y,
                (TypeData.Data.LongintTypeCode, VisitLongInt),
                (TypeData.Data.RationalTypeCode, VisitRational),
                (TypeData.Data.NumberTypeCode, VisitNumber)

            )
                (Self, Driver)
        );
    }
    static List<BackingNumberType> GetArrayOfBNTs(string number)
    {
        int width = (int)Math.Ceiling(Math.Log10(BackingNumberType.MaxValue));

        number = number.PadLeft((int)(Math.Ceiling((double)number.Length / width) * width));
        List<BackingNumberType> Chars = [];
        for (int i = 0; i < number.Length; i += width)
        {
            Chars.Add(BackingNumberType.Parse(number[i..(i + width)]));
        }

        //Add typecode and length prefix
        var LengthArray = new GenericNumberWrapper<int>(Chars.Count).Value;

        foreach (var i in LengthArray.Reverse())//Chars.Count = 15: [0x00, 0x00, 0x00, 0x0F] -> [0x0F, 0x00, 0x00, 0x00] which is then prepended from start to finish to get back to big-endian order
        {
            Chars = Chars.Prepend(i).ToList();
        }
        Chars = Chars.Prepend(TypeData.Data.LongintTypeCode.Value.Single()).ToList();
        return Chars;
    }
    static Pointer<BackingNumberType> VisitLongInt(Node Self, CodeGenerator Driver)
    {
        var Chars = GetArrayOfBNTs(Self.Data!.Lexeme);
        var Ptr = Driver.Data.StaticDataArea.AllocateAndFill(Chars.Count, Chars);
        return Ptr;
    }
    static Pointer<BackingNumberType> VisitRational(Node Self, CodeGenerator Driver)
    {
        var parts = Self.Data!.Lexeme.Split('.');
        Debug.Assert(parts.Length == 2);
        string Denominator = "1".PadRight(parts[1].Length);
        Debug.Assert(Denominator[0] == '1');

        //TODO: GCD the numerator and Ptr

        var NumeratorList = GetArrayOfBNTs(parts[0] + parts[1]);
        var DenominatorList = GetArrayOfBNTs(Denominator);
        var NumeratorPtr = Driver.Data.StaticDataArea.AllocateAndFill(NumeratorList.Count, NumeratorList);
        var DenominatorPtr = Driver.Data.StaticDataArea.AllocateAndFill(DenominatorList.Count, DenominatorList);

        List<BackingNumberType> PtrList = [.. NumeratorPtr.Value, .. DenominatorPtr.Value];
        Debug.Assert(PtrList.Count <= BackingNumberType.MaxValue);
        List<BackingNumberType> RationalList = [TypeData.Data.RationalTypeCode.Value.Single(), (BackingNumberType)PtrList.Count, .. PtrList];

        var RationalPtr = Driver.Data.StaticDataArea.AllocateAndFill(RationalList.Count, RationalList);

        return RationalPtr;
    }
    static Pointer<BackingNumberType> VisitNumber(Node Self, CodeGenerator Driver)
    {
        throw new NotImplementedException();
    }

}