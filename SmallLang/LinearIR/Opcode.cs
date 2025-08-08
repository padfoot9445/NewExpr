namespace SmallLang.LinearIR;

public enum Opcode : OpcodeBackingType
{
    ICallS, //ICallS FunctionID #arguments are in the stack, Function ID immediate. Returns value to the stack. Stack: [Arg1, Arg2...] -> Stack:[Retval]
    SCallS, //SCall #Stack: [FunctionID, Arg1, Arg2...] -> Stack:[Retval]
    ICallR, //ICallR FunctionID ReturnRegister #Arguments are in the stack, Function ID immediate, return destination register immediate. Stores the retval into the ReturnRegister.
    LoadI, //LoadI Word DestinationRegister #loads the inline constant Word into the DestinationRegister register
    PushI, //PushI Word #Pushes the inline constant Word onto the stack.
    NewPR, //NewPR ParentType ElementCount DstRegister #args on stack, we don't need to preserve inner type information; if it's a hashing we just visit the pointer and get the type info from there and hash that #pointer to object in register
    NewPS, //NewPS ParentType Elementcount #pointer to object on stack
    NewDR, //NewD ParentType ElementCount DstRegister #new but it's not actually pointers on stack, which is crazy, but yeah, pointer to object in register
    NewDS, //NewD ParentType, pointer to object on stack
}