#if false
using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.CodeGen.Frontend;
using Op = Operation<Opcode, BackingNumberType>;
class OpcodeCodeEmitter
{
    static Op Emit(Opcode opcode, params IOperationArgument<byte>[] args)
    {
        return new((OpcodeWrapper)opcode, args);
    }
    static 
    static Op JMP(GenericNumberWrapper<int> DestinationChunk, int width)
    {
        return Emit(JMPu8, DestinationChunk);
    }
    public static Op JMP(GenericNumberWrapper<int> DestinationChunk)
    {
        if (DestinationChunk.BackingValue <= byte.MaxValue)
        {
            return JMP(DestinationChunk, 8);
        }
        throw new NotImplementedException($"JMP not implemented for chunk-id of values larger than 255. Got {DestinationChunk}.");
    }
}
#endif