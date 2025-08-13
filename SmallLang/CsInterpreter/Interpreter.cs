using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using Common.LinearIR;
#if false
namespace SmallLang.CsIntepreter;

public class Interpreter(Operation<Opcode, BackingNumberType>[] Operations, uint[] Data, TextReader reader, TextWriter writer)
{
    readonly Stack<BackingNumberType> Stack = new Stack<BackingNumberType>();
    Dictionary<uint, uint> Registers = new();
    List<uint> RAM = new();
    const uint RamPointerBit = ((uint)1) << 31;
    public void Interpret()
    {
        throw new NotImplementedException();
        foreach (var Op in Operations)
        {
            switch ((Opcode)Op.Op.BackingValue)
            {
                case Opcode.ICallS:
                    CallStdLibFunctions(Op.Operands[0].Value.ToArray());
                    break;
                case Opcode.SCallS:
                    CallStdLibFunctions(Stack.Pop());
                    break;
                case Opcode.LoadI:
                    Registers[Op.Operands[0].Value.Last()] = Op.Operands[1].Value.Last();
                    break;
                case Opcode.PushI:
                    Stack.Push(Op.Operands[0].Value.Last());
                    break;
                case Opcode.ICallR:
                    throw new NotImplementedException();
            }
        }
    }
    string DecodeStringStatic(uint Ptr) => DecodeString(Ptr, Data);
    string DecodeStringRAM(uint Ptr) => DecodeString((Ptr & (uint.MaxValue >> 1)), RAM.ToArray());
    string DecodeStringAuto(uint Ptr) => Ptr >> 31 == 1 ? DecodeStringRAM(Ptr) : DecodeStringStatic(Ptr);
    string DecodeString(uint APtr, uint[] Src)
    {
        int Ptr = (int)APtr;
        ushort TypeCode = (ushort)(Src[Ptr] >> 16);
        Debug.Assert(TypeCode == 1);
        ushort WordCount = (ushort)(Src[Ptr] & 0xFFFF);
        Ptr++;
        uint[] StringStruct = Src[Ptr..(Ptr + WordCount - 1 + 1)];
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
    uint[] EncodeString(string str)
    {
        ushort Typecode = 1;
        ushort WordCount = (ushort)(Math.Ceiling((double)str.Length / 4) + 1);
        uint Header = (uint)((Typecode << 16) | WordCount);
        uint CharCount = (uint)str.Length;
        List<uint> Out = [Header, CharCount];
        char[] Buffer = new char[4];
        int LP = 0;
        foreach (char i in str)
        {
            if (LP == 4)
            {
                Out.Add(
                    (uint)(Buffer[0] |
                    Buffer[1] << 8 |
                    Buffer[2] << 16 |
                    Buffer[3] << 24)
                );
                Buffer = new char[4];
                LP = 0;
            }
            Buffer[LP++] = i;
        }
        Out.Add(
                    (uint)(Buffer[0] |
                    Buffer[1] << 8 |
                    Buffer[2] << 16 |
                    Buffer[3] << 24)
                );
        return Out.ToArray();

    }
    uint Alloc() => (uint)RAM.Count;
    void WriteRam(uint[] Section, uint Ptr)
    {
        if (Ptr + Section.Length - 1 >= RAM.Count)
        {
            RAM.AddRange(new uint[(Ptr + Section.Length - 1) - RAM.Count + 1]);
        }
        for (int i = (int)Ptr; i < (Ptr + Section.Length); i++)
        {
            RAM[i] = Section[i - Ptr];
        }
    }
    void CallStdLibFunctions(params byte[] FunctionID)
    {
        switch (FunctionID.Last())
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
        string inp = reader.ReadLine()!;
        uint Ptr = Alloc();
        WriteRam(EncodeString(inp), Ptr);
        Stack.Push((byte)(Ptr | RamPointerBit));
    }
    void StdLibFunctions__Output()
    {
        writer.WriteLine(DecodeStringAuto(Stack.Pop()));
    }
}
#endif