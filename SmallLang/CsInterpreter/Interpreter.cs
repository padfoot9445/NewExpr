using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.CsIntepreter;
public class Interpreter(Operation<uint>[] Operations, uint[] Data, TextReader reader, TextWriter writer)
{
    readonly Stack<uint> Stack = new Stack<uint>();
    Dictionary<uint, uint> Registers = new();
    public void Interpret()
    {
        foreach (var Op in Operations)
        {
            switch ((Opcode)Op.Op.Value)
            {
                case Opcode.ICallS:
                    CallStdLibFunctions(Op.Operands[0].Value);
                    break;
                case Opcode.SCallS:
                    CallStdLibFunctions(Stack.Pop());
                    break;
                case Opcode.LoadI:
                    Registers[Op.Operands[0].Value] = Op.Operands[1].Value;
                    break;
                case Opcode.PushI:
                    Stack.Push(Op.Operands[0].Value);
                    break;
                case Opcode.ICallR:
                    throw new NotImplementedException();
            }
        }
    }
    string DecodeString(int Ptr)
    {
        ushort TypeCode = (ushort)(Data[Ptr] >> 16);
        Debug.Assert(TypeCode == 1);
        ushort WordCount = (ushort)(Data[Ptr] & 0xFFFF);
        Ptr++;
        uint[] StringStruct = Data[Ptr..(Ptr + WordCount)];
        uint CharCount = StringStruct[0];
        StringBuilder Chars = new();
        foreach (uint FourChars in StringStruct.Skip(1).SkipLast((CharCount % 4) == 0 ? 0 : 1))
        {
            Chars.Append((char)(FourChars & 0xFF));
            Chars.Append((char)((FourChars >> 8) & 0xFF));
            Chars.Append((char)((FourChars >> 16) & 0xFF));
            Chars.Append((char)((FourChars >> 24) & 0xFF));
        }

        for (int i = 0; i < CharCount % 4; i++)
        {
            Chars.Append((char)((StringStruct[^1] >> (8 * i)) & 0xFF));
        }

        return Chars.ToString();
    }
    void CallStdLibFunctions(uint FunctionID)
    {
        switch (FunctionID)
        {
            case 1:
                StdLibFunctions__Input();
                break;
            case 2:
                StdLibFunctions__Output(); break;
            default:
                throw new Exception();
        }
    }
    void StdLibFunctions__Input()
    {
        throw new NotImplementedException();
    }
    void StdLibFunctions__Output()
    {
        writer.WriteLine(DecodeString((int)Stack.Pop()));
    }
}