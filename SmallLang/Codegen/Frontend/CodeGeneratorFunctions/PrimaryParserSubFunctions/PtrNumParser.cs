using Common.Dispatchers;
using SmallLang.IR.Metadata;

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
    static void VisitLongInt(Node Self, CodeGenerator Driver)
    {
        List<byte> Chars = [];
        for (int i = 0; i < Self.Data!.Lexeme.Length; i += 2)
        {
            Chars.Add(byte.Parse(Self.Data.Lexeme[i..(i + 2)]));
        }
        var Ptr = Driver.AddStaticData(Chars);
    }
    static void VisitRational(Node Self, CodeGenerator Driver)
    {
        throw new NotImplementedException();
    }
    static void VisitNumber(Node Self, CodeGenerator Driver)
    {
        throw new NotImplementedException();
    }

}