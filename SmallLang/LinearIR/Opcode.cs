namespace SmallLang.LinearIR;

public enum Opcode : OpcodeBackingType
{
    JMP,//JMP [Chunk ID] #Jumps unconditionally to the chunk given by the chunk-id
}