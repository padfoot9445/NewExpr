using System.Diagnostics;
using Common.Dispatchers;
using Common.Metadata;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryVisitorSubFunctions;

internal static class PtrNumVisitor
{
    public static void Visit(PrimaryNode Self, CodeGenerator Driver)
    {
        Driver.Emit
        (
            HighLevelOperation.Push(
                Self.Switch<PrimaryNode, SmallLangType, Func<PrimaryNode, CodeGenerator, Pointer<byte>>>
                    (
                        x => x.TypeOfExpression!,
                        (x, y) => x == y,
                        (TypeData.Longint, VisitLongInt),
                        (TypeData.Rational, VisitRational),
                        (TypeData.Number, VisitNumber)
                    )
                    (Self, Driver)
            ));
    }

    private static List<BackingNumberType> GetArrayOfBNTs(string number)
    {
        var width = (int)Math.Ceiling(Math.Log10(BackingNumberType.MaxValue));

        number = number.PadLeft((int)(Math.Ceiling((double)number.Length / width) * width));
        List<BackingNumberType> Chars = [];
        for (var i = 0; i < number.Length; i += width) Chars.Add(BackingNumberType.Parse(number[i..(i + width)]));

        //Add typecode and length prefix
        var LengthArray = new GenericNumberWrapper<int>(Chars.Count).Value;

        foreach (var i in
                 LengthArray
                     .Reverse()) //Chars.Count = 15: [0x00, 0x00, 0x00, 0x0F] -> [0x0F, 0x00, 0x00, 0x00] which is then prepended from start to finish to get back to big-endian order
            Chars = Chars.Prepend(i).ToList();

        Chars = Chars.Prepend(TypeData.Longint.Value.Single()).ToList();
        return Chars;
    }

    private static Pointer<BackingNumberType> VisitLongInt(PrimaryNode Self, CodeGenerator Driver)
    {
        var Chars = GetArrayOfBNTs(Self.Data!.Lexeme);
        var Ptr = Driver.Data.StaticDataArea.AllocateAndFill(Chars.Count, Chars);
        return Ptr;
    }

    private static Pointer<BackingNumberType> VisitRational(PrimaryNode Self, CodeGenerator Driver)
    {
        var parts = Self.Data!.Lexeme.Split('.');
        Debug.Assert(parts.Length == 2);
        var Denominator = "1".PadRight(parts[1].Length);
        Debug.Assert(Denominator[0] == '1');

        //TODO: GCD the numerator and Ptr

        var NumeratorList = GetArrayOfBNTs(parts[0] + parts[1]);
        var DenominatorList = GetArrayOfBNTs(Denominator);
        var NumeratorPtr = Driver.Data.StaticDataArea.AllocateAndFill(NumeratorList.Count, NumeratorList);
        var DenominatorPtr = Driver.Data.StaticDataArea.AllocateAndFill(DenominatorList.Count, DenominatorList);

        List<BackingNumberType> PtrList = [.. NumeratorPtr.Value, .. DenominatorPtr.Value];
        Debug.Assert(PtrList.Count <= BackingNumberType.MaxValue);
        List<BackingNumberType> RationalList =
            [TypeData.Rational.Value.Single(), (BackingNumberType)PtrList.Count, .. PtrList];

        var RationalPtr = Driver.Data.StaticDataArea.AllocateAndFill(RationalList.Count, RationalList);

        return RationalPtr;
    }

    private static Pointer<BackingNumberType> VisitNumber(PrimaryNode Self, CodeGenerator Driver)
    {
        throw new NotImplementedException();
    }
}