using Common.Dispatchers;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;

using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static ImportantASTNodeType;
using static Opcode;

internal static partial class PrimaryVisitor
{
    static void ParseIdentifier(Node self, CodeGenerator Driver)
    {
        Driver.Emit<int, int>(DeloadVar, Driver.Data.VariableSlots.KeyToPointerStartMap[self.Attributes.VariableName!], Driver.Data.VariableSlots.KeyToNumberOfCellsUsed[self.Attributes.VariableName!]);
    }
    static void ParseValNum(Node self, CodeGenerator Driver) => ValNumParser.Parse(self, Driver);
    internal static void Visit(Node Self, CodeGenerator Driver)
    {
        Driver.SETCHUNK();
        if (Self.NodeType == Identifier) { ParseIdentifier(Self, Driver); return; }
        else
        {
            Self.Switch(
            x => x.Attributes.TypeOfExpression!,
            (x, y) => x == y,
            (TypeData.Data.StringTypeCode, ParseString),
            (TypeData.Data.CharTypeCode, ParseValNum),
            (TypeData.Data.FloatTypeCode, ParseValNum),
            (TypeData.Data.IntTypeCode, ParseValNum),
            (TypeData.Data.DoubleTypeCode, ParseValNum),
            (TypeData.Data.ByteTypeCode, ParseValNum),
            (TypeData.Data.LongTypeCode, ParseValNum),
            (TypeData.Data.NumberTypeCode, ParsePtrNum),
            (TypeData.Data.LongintTypeCode, ParsePtrNum),
            (TypeData.Data.RationalTypeCode, ParsePtrNum),
            (TypeData.Data.BooleanTypeCode, ParseBool),
            (TypeData.Data.ArrayTypeCode, ParseCollection),
            (TypeData.Data.ListTypeCode, ParseCollection),
            (TypeData.Data.SetTypeCode, ParseCollection),
            (TypeData.Data.DictTypeCode, ParseCollection)
        )
        (Self, Driver);
        }



        void ParsePtrNum(Node self, CodeGenerator Driver) => throw new NotImplementedException();
        void ParseBool(Node self, CodeGenerator Driver) => throw new NotImplementedException();
        void ParseString(Node self, CodeGenerator Driver) => throw new NotImplementedException();
        void ParseCollection(Node self, CodeGenerator Driver) => throw new NotImplementedException();

    }
}
