using System.Diagnostics;
using Common.AST;
using Common.LinearIR;
using SmallLang.Constants;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang.Backend.CodeGenComponents;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public abstract class BaseCodeGenComponent(CodeGenVisitor driver)
{
    protected T SwitchOnType<T>(
        SmallLangType Type,
        Func<Exception> NotFound,
        params (SmallLangType TypeCode, Func<T>)[] Types
    )
    {
        foreach ((SmallLangType TypeCode, Func<T> RetVal) in Types)
        {
            if (Type == TypeCode)
            {
                return RetVal();
            }
        }
        throw NotFound();
    }
    protected SmallLangType StringTypeCode => TypeData.Data.StringTypeCode;
    protected SmallLangType FloatTypeCode => TypeData.Data.FloatTypeCode;
    protected SmallLangType IntTypeCode => TypeData.Data.IntTypeCode;
    protected SmallLangType DoubleTypeCode => TypeData.Data.DoubleTypeCode;
    protected SmallLangType NumberTypeCode => TypeData.Data.NumberTypeCode;
    protected SmallLangType LongTypeCode => TypeData.Data.LongTypeCode;
    protected SmallLangType LongintTypeCode => TypeData.Data.LongintTypeCode;
    protected SmallLangType ByteTypeCode => TypeData.Data.ByteTypeCode;
    protected SmallLangType CharTypeCode => TypeData.Data.CharTypeCode;
    protected SmallLangType BooleanTypeCode => TypeData.Data.BooleanTypeCode;
    protected SmallLangType RationalTypeCode => TypeData.Data.RationalTypeCode;
    protected int TypeCodeOffsetInHeader => TypeData.Data.TypeCodeOffsetInHeader;
    protected void Emit(Opcode Op) => Emit(Op, new uint[0]);
    protected void Emit(Opcode Op, params uint[] args) => Emit(Op, args.Select(x => (UIntOpArg)x).ToArray());
    protected void Emit(Opcode Op, params IOperationArgument<uint>[] args) => Emit(new Operation<uint>(new OpcodeWrapper(Op), args));
    protected void Emit(Operation<uint> Instruction)
    {
        Driver.Instructions.Add(Instruction);
    }
    protected uint WriteStaticData(SmallLangType Typecode, uint[] Data) => WriteStaticData((ushort)Typecode.Value, (ushort)Data.Length, Data);
    protected uint WriteStaticData(ushort Typecode, ushort WordLength, uint[] Data)
    {
        uint Header = (uint)((Typecode << TypeCodeOffsetInHeader) | WordLength);
        uint[] ToWrite = [Header, .. Data];
        uint Ptr = (uint)Driver.StaticData.Count;
        Driver.StaticData.AddRange(ToWrite);
        return Ptr;
    }
    protected uint[] BytesToUInt(params byte[] Vals)
    {
        byte[] InVals = new byte[Vals.Length + (4 - (Vals.Length % 4))];
        Vals.CopyTo(InVals, 0);
        Debug.Assert(InVals[Vals.Length..^0].All(x => x == 0));
        Debug.Assert(InVals.Length % 4 == 0);
        uint[] OutVals = new uint[InVals.Length / 4];
        for (int i = 0; i < InVals.Length; i += 4)
        {
            OutVals[i / 4] = BitConverter.ToUInt32(InVals[i..(i + 4)]);
        }
        return OutVals;

    }
    protected uint? LoadValue(params byte[] Vals)
    {
        var Uints = BytesToUInt(Vals);
        Debug.Assert(Vals.Length > 0);
        Debug.Assert(Uints.Length > 0);
        var ret = LoadValue(Uints[0]);
        for (int i = 1; i < Uints.Length; i++)
        {
            Driver.DestinationRegister = Driver.DestinationRegister is null ? null : Driver.DestinationRegister + 1;
            LoadValue(Uints[i]);
        }
        return ret;
    }
    protected uint? LoadValue(uint Val)
    {
        if (Driver.OutputToRegister)
        {
            Emit(Opcode.LoadI, Val, GetDestRegister());
            return Driver.DestinationRegister ?? Driver.LastUsedRegister;
        }
        else
        {
            Emit(Opcode.PushI, Val);
            return null;
        }
    }
    protected void PushArgsToStack(Node? Arguments, Node self)
    {
        if (Arguments is not Node arg)
            return;
        else
        {
            foreach (var ia in arg.Children)
            {
                Driver.Exec(self, ia);
            }
        }
    }
    protected uint GetDestRegister() => Driver.DestinationRegister ?? ++Driver.LastUsedRegister;
    protected CodeGenVisitor Driver = driver;
    public abstract void GenerateCode(Node? parent, Node self);
}