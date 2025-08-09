using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.Frontend.CodeGen;

using static Opcode;
class OpcodeCodeEmitter
{
    Operation<Opcode, BackingNumberType> Emit(Opcode opcode, params IOperationArgument<byte>[] args)
    {
        return new((OpcodeWrapper)opcode, args);
    }
    Operation<Opcode, BackingNumberType> JMP(GenericNumberWrapper<int> DestinationChunk, int width)
    {
        return Emit(JMPu8, DestinationChunk);
    }
    Operation<Opcode, BackingNumberType> JMP(GenericNumberWrapper<int> DestinationChunk)
    {
        if (DestinationChunk.BackingValue <= byte.MaxValue)
        {
            return JMP(DestinationChunk, 8);
        }
        throw new NotImplementedException($"JMP not implemented for chunk-id of values larger than 255. Got {DestinationChunk}.");
    }
}