using System.Numerics;
using Common.Dispatchers;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;



internal static class ValNumParser
{
    static Action EmitCodeDelegateGenerator<T>(Func<string, T> parser, SmallLangNode self, CodeGenerator Driver) where T : IBinaryInteger<T>, IMinMaxValue<T>
    {

        void EmitCode()
        {
            Driver.Emit(HighLevelOperation.Push<T>(parser(self.Data!.Lexeme)));
        }
        return EmitCode;
    }
    static Func<string, TOut> GetIntParseFunctionFromFloatParseFunction<TIn, TOut>(Func<string, TIn> FloatParseFunction, Func<TIn, TOut> Converter)
    where TIn : IBinaryFloatingPointIeee754<TIn>
    where TOut : IBinaryInteger<TOut>
    {
        return x => Converter(FloatParseFunction(x));
    }
    internal static void Parse(SmallLangNode self, CodeGenerator Driver)
    {
        Action Emitter = self.Switch
            (
                Accessor: x => x.Attributes.TypeOfExpression!,
                Comparer: (x, y) => x == y,
                (TypeData.Char, EmitCodeDelegateGenerator(char.Parse, self, Driver)),
                (TypeData.Float, EmitCodeDelegateGenerator(GetIntParseFunctionFromFloatParseFunction(float.Parse, BitConverter.SingleToUInt32Bits), self, Driver)),
                (TypeData.Int, EmitCodeDelegateGenerator(int.Parse, self, Driver)),
                (TypeData.Double, EmitCodeDelegateGenerator(GetIntParseFunctionFromFloatParseFunction(double.Parse, BitConverter.DoubleToUInt64Bits), self, Driver)),
                (TypeData.Byte, EmitCodeDelegateGenerator(byte.Parse, self, Driver)),
                (TypeData.Long, EmitCodeDelegateGenerator(long.Parse, self, Driver))
            );
        Emitter();
    }

}

