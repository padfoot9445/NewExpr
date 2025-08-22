using Common.Dispatchers;
using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;

using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static ImportantASTNodeType;
internal static class PrimaryVisitor
{
    static void ParseIdentifier(SmallLangNode self, CodeGenerator Driver)
    {
        Driver.Emit(HighLevelOperation.DeloadVar<int, int>(Driver.Data.GetVariableStartRegister(self.Attributes.VariableName!), Driver.Data.GetVariableWidth(self.Attributes.VariableName!)));
    }
    static void ParseValNum(SmallLangNode self, CodeGenerator Driver) => ValNumParser.Parse(self, Driver);
    internal static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {

        if (Self.NodeType == Identifier) { ParseIdentifier(Self, Driver); return; }
        else
        {
            Self.Switch(
            x => x.Attributes.TypeOfExpression!,
            (x, y) => x == y,
            (TypeData.String, ParseString),
            (TypeData.Char, ParseValNum),
            (TypeData.Float, ParseValNum),
            (TypeData.Int, ParseValNum),
            (TypeData.Double, ParseValNum),
            (TypeData.Byte, ParseValNum),
            (TypeData.Long, ParseValNum),
            (TypeData.Number, ParsePtrNum),
            (TypeData.Longint, ParsePtrNum),
            (TypeData.Rational, ParsePtrNum),
            (TypeData.Bool, ParseBool),
            (TypeData.Array, ParseCollection),
            (TypeData.List, ParseCollection),
            (TypeData.Set, ParseCollection),
            (TypeData.Dict, ParseCollection)
        )
        (Self, Driver);
        }



        void ParsePtrNum(SmallLangNode self, CodeGenerator Driver) => PtrNumParser.Parse(self, Driver);
        void ParseBool(SmallLangNode self, CodeGenerator Driver)
        {
            Driver.Emit(HighLevelOperation.Push<BackingNumberType>(self.Switch(x => x.Data!.TT, (x, y) => x == y, (TokenType.TrueLiteral, CodeGenerator.TrueValue), (TokenType.FalseLiteral, CodeGenerator.FalseValue))));
        }
        void ParseString(SmallLangNode self, CodeGenerator Driver)
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
        void ParseCollection(SmallLangNode self, CodeGenerator Driver) => throw new NotSupportedException("Shouldn't have Collection Primaries");

    }
}
