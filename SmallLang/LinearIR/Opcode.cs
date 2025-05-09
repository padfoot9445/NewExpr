namespace SmallLang.LinearIR;
public enum Opcode
{
    ICallS, //ICallS FunctionID #arguments are in the stack, Function ID immediate. Returns value to the stack. Stack: [Arg1, Arg2...] -> Stack:[Retval]
    SCallS, //SCall #Stack: [FunctionID, Arg1, Arg2...] -> Stack:[Retval]
    ICallR, //ICallR FunctionID ReturnRegister #Arguments are in the stack, Function ID immediate, return destination register immediate. Stores the retval into the ReturnRegister.
    LoadI, //LoadI Word DestinationRegister #loads the inline constant Word into the DestinationRegister register
    PushI, //PushI Word #Pushes the inline constant Word onto the stack.
}