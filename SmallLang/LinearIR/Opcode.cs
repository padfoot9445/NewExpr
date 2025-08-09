namespace SmallLang.LinearIR;

public enum Opcode : OpcodeBackingType
{
    JMPu8,//JMP [Chunk ID: u8] #Jumps unconditionally to the chunk given by the chunk-id
}