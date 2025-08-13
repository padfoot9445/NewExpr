using System.Numerics;
using Common.Dispatchers;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;

using static SmallLang.IR.LinearIR.Opcode;

internal static class ValNumParser
{
    static Action EmitCodeDelegateGenerator<T>(Func<string, T> parser, Node self, CodeGenerator Driver) where T : IBinaryInteger<T>, IMinMaxValue<T>
    {

        void EmitCode()
        {
            Driver.Emit<T>(Push, parser(self.Data!.Lexeme));
        }
        return EmitCode;
    }
    static Func<string, TOut> GetIntParseFunctionFromFloatParseFunction<TIn, TOut>(Func<string, TIn> FloatParseFunction, Func<TIn, TOut> Converter)
    where TIn : IBinaryFloatingPointIeee754<TIn>
    where TOut : IBinaryInteger<TOut>
    {
        return x => Converter(FloatParseFunction(x));
    }
    internal static void Parse(Node self, CodeGenerator Driver)
    {
        self.Switch
            (
                Accessor: x => x.Attributes.TypeOfExpression!,
                Comparer: (x, y) => x == y,
                (TypeData.Data.CharTypeCode, EmitCodeDelegateGenerator(char.Parse, self, Driver)),
                (TypeData.Data.FloatTypeCode, EmitCodeDelegateGenerator(GetIntParseFunctionFromFloatParseFunction(float.Parse, BitConverter.SingleToUInt32Bits), self, Driver)),
                (TypeData.Data.IntTypeCode, EmitCodeDelegateGenerator(int.Parse, self, Driver)),
                (TypeData.Data.DoubleTypeCode, EmitCodeDelegateGenerator(GetIntParseFunctionFromFloatParseFunction(double.Parse, BitConverter.DoubleToUInt64Bits), self, Driver)),
                (TypeData.Data.ByteTypeCode, EmitCodeDelegateGenerator(byte.Parse, self, Driver)),
                (TypeData.Data.LongTypeCode, EmitCodeDelegateGenerator(long.Parse, self, Driver))
            );
    }

}

