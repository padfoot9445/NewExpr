using System.Diagnostics;
using Common.Dispatchers;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
using static SmallLang.IR.LinearIR.Opcode;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;

internal static class PtrNumParser
{
    public static void Parse(Node Self, CodeGenerator Driver)
    {
        Self.Switch(
            x => x.Attributes.TypeOfExpression!,
            Comparer: (x, y) => x == y,
            (TypeData.Data.LongintTypeCode, VisitLongInt),
            (TypeData.Data.RationalTypeCode, VisitRational),
            (TypeData.Data.NumberTypeCode, VisitNumber)
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
            Chars.Prepend(i);
        }
        Chars.Prepend(TypeData.Data.LongintTypeCode.Value.Single());
        return Chars;
    }
    static void VisitLongInt(Node Self, CodeGenerator Driver)
    {
        var Chars = GetArrayOfBNTs(Self.Data!.Lexeme);
        var Ptr = Driver.Data.StaticDataArea.AllocateAndFill(Chars.Count, Chars);
        Driver.Emit(Push, Ptr);
    }
    static void VisitRational(Node Self, CodeGenerator Driver)
    {
        var parts = Self.Data!.Lexeme.Split('.');
        Debug.Assert(parts.Length == 2);
        string Denominator = "1".PadRight(parts[1].Length);
        Debug.Assert(Denominator[0] == '1');
        var NumeratorList = GetArrayOfBNTs(parts[0] + parts[1]);
        var DenominatorList = GetArrayOfBNTs(Denominator);

    }
    static void VisitNumber(Node Self, CodeGenerator Driver)
    {
        throw new NotImplementedException();
    }

}