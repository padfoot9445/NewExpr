namespace SmallLang.LinearIR;

public enum Opcode : OpcodeBackingType
{
    JMP,//JMP [Chunk ID] #Jumps unconditionally to the chunk given by the chunk-id
    BRZ, //BRZ [Chunk0 ID] [Chunk1 ID] #if the value at the top of the stack is 0, jump to Chunk1, otherwise jump to Chunk0
    RET, //RET #Returns control to the calling chunk. Return values should be on the stack.
    SWITCH, //SWITCH [Length], [StartingIndex] # SWITCH Length StartIndex
            // For i in [1...Length]:
            //     run CHUNKS[StartIndex + i * 2 - 1]
            //     if Top of Stack == 2nd value on stack: {bottom}[3rd value, 2nd value, 1st value (aka top of stack)]{top}
            //         run CHUNKS[StartIndex + i * 2]
            //         run CHUNKS[StartIndex + Length * 2 + 2]
    IFNE, //IFNE [StartingIndex] [Length]
          //IFNN StartIndex Length
          // for i in [1...LENGTH]:
          //     run CHUNKS[i * 2 - 1]
          //     if (POP) is TRUE:
          //         run CHUNKS[i * 2]
          //         jump to CHUNKS[LENGTH * 2 + 1]
          // jump to CHUNKS[LENGTH * 2 + 1]
}