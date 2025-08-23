using Common.Dispatchers;
using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryVisitorSubFunctions;

using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;


internal static class PrimaryVisitor
{
    internal static void VisitIdentifier(IdentifierNode self, CodeGenerator Driver)
    {
        Driver.Emit(HighLevelOperation.PushFromRegister(Driver.Data.GetVariableStartRegister(self.Attributes.VariableName!), Driver.Data.GetVariableWidth(self.Attributes.VariableName!)));
    }
    internal static void Visit(PrimaryNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {

            Self.Switch(
            x => x.Attributes.TypeOfExpression!,
            (x, y) => x == y,
            (TypeData.String, VisitString),
            (TypeData.Char, ValNumVisitor.Visit),
            (TypeData.Float, ValNumVisitor.Visit),
            (TypeData.Int, ValNumVisitor.Visit),
            (TypeData.Double, ValNumVisitor.Visit),
            (TypeData.Byte, ValNumVisitor.Visit),
            (TypeData.Long, ValNumVisitor.Visit),
            (TypeData.Number, PtrNumVisitor.Visit),
            (TypeData.Longint, PtrNumVisitor.Visit),
            (TypeData.Rational, PtrNumVisitor.Visit),
            (TypeData.Bool, VisitBool),
            (TypeData.Array, VisitCollection),
            (TypeData.List, VisitCollection),
            (TypeData.Set, VisitCollection),
            (TypeData.Dict, VisitCollection)
        )
        (Self, Driver);

            Driver.Next();
        });
    }

    private static void VisitBool(SmallLangNode self, CodeGenerator Driver)
    {
        Driver.Emit(HighLevelOperation.Push<BackingNumberType>(self.Switch(x => x.Data!.TT, (x, y) => x == y, (TokenType.TrueLiteral, CodeGenerator.TrueValue), (TokenType.FalseLiteral, CodeGenerator.FalseValue))));
    }
    private static void VisitString(SmallLangNode self, CodeGenerator Driver)
    {
        List<byte> Chars =
        [
            TypeData.String.Value.Single(),
                .. ((GenericNumberWrapper<int>)self.Data!.Lexeme.Length).Value,
                .. self.Data!.Lexeme.Select(x => (byte)x)
        ];

        var Ptr = Driver.Data.StaticDataArea.AllocateAndFill(Chars.Count, Chars);
        Driver.Emit(HighLevelOperation.Push(Ptr));
    }
    private static void VisitCollection(SmallLangNode self, CodeGenerator Driver) => throw new NotSupportedException("Shouldn't have Collection Primaries");

}
