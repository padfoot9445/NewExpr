using System.Diagnostics;
using System.Numerics;
using Common.Dispatchers;
using Common.LinearIR;
using SmallLang.Constants;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Frontend.CodeGen;

using static ImportantASTNodeType;
using static Opcode;
public partial class CodeGenerator
{
    private void ParsePrimary(Node Self)
    {
        SETCHUNK();
        if (Self.NodeType == Identifier) { ParseIdentifier(Self); return; }
        else Self.Switch(
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
        );
        void ParseIdentifier(Node self)
        {
            Emit<int, int>(DeloadVar, Data.VariableSlots.KeyToPointerStartMap[self.Attributes.VariableName!], Data.VariableSlots.KeyToNumberOfCellsUsed[self.Attributes.VariableName!]);
        }

        Action EmitCodeDelegateGenerator<T>(Func<string, T> parser, Node self) where T : INumber<T>
        {

            void EmitCode<T>() where T : INumber<T>
            {
                Emit<T>(Push, parser(self.Data!.Lexeme));
            }
            return EmitCode<T>;
        }
        void ParseValNum(Node self)
        {
            self.Switch
                (
                    Accessor: x => x.Attributes.TypeOfExpression!,
                    Comparer: (x, y) => x == y,
                    (TypeData.Data.CharTypeCode, EmitCodeDelegateGenerator(char.Parse, self)),
                    (TypeData.Data.FloatTypeCode, EmitCodeDelegateGenerator(float.Parse, self)),
                    (TypeData.Data.IntTypeCode, EmitCodeDelegateGenerator(int.Parse, self)),
                    (TypeData.Data.DoubleTypeCode, EmitCodeDelegateGenerator(double.Parse, self)),
                    (TypeData.Data.ByteTypeCode, EmitCodeDelegateGenerator(byte.Parse, self)),
                    (TypeData.Data.LongTypeCode, EmitCodeDelegateGenerator(long.Parse, self))
                );
        }

        void ParsePtrNum(Node self) => throw new NotImplementedException();
        void ParseBool(Node self) => throw new NotImplementedException();
        void ParseString(Node self) => throw new NotImplementedException();
        void ParseCollection(Node self) => throw new NotImplementedException();
    }
}
