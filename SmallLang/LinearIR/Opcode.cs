namespace SmallLang.LinearIR;

public enum Opcode : OpcodeBackingType
{
    JMP,//JMP [Chunk ID] #Jumps unconditionally to the chunk given by the chunk-id
    BRZ, //BRZ [Chunk0 ID] [Chunk1 ID] #if the value at the top of the stack is 0, jump to Chunk1, otherwise jump to Chunk0
}