namespace SmallLang.LinearIR;
public enum Opcode
{
    ICall, //ICall FunctionID #arguments are in the stack, Function ID immediate. Stack: [Arg1, Arg2...]
    SCall, //SCall #Stack: [FunctionID, Arg1, Arg2...]
    LoadI, //LoadI Word DestinationRegister #loads the inline constant Word into the DestinationRegister register
    PushI, //PushI Word #Pushes the inline constant Word onto the stack.
}