using System.Diagnostics;
using Common.AST;
using Common.LinearIR;
using SmallLang.Constants;
using SmallLang.LinearIR;

namespace SmallLang.Backend.CodeGenComponents;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public abstract class BaseCodeGenComponent(CodeGenVisitor driver)
{
    protected T SwitchOnType<T>(
        uint Type,
        Func<Exception> NotFound,
        params (uint TypeCode, Func<T>)[] Types
    )
    {
        foreach ((uint TypeCode, Func<T> RetVal) in Types)
        {
            if (Type == TypeCode)
            {
                return RetVal();
            }
        }
        throw NotFound();
    }
    protected uint StringTypeCode => TypeData.Data.StringTypeCode;
    protected uint FloatTypeCode => TypeData.Data.FloatTypeCode;
    protected uint IntTypeCode => TypeData.Data.IntTypeCode;
    protected uint DoubleTypeCode => TypeData.Data.DoubleTypeCode;
    protected uint NumberTypeCode => TypeData.Data.NumberTypeCode;
    protected uint LongTypeCode => TypeData.Data.LongTypeCode;
    protected uint LongintTypeCode => TypeData.Data.LongintTypeCode;
    protected uint ByteTypeCode => TypeData.Data.ByteTypeCode;
    protected uint CharTypeCode => TypeData.Data.CharTypeCode;
    protected uint BooleanTypeCode => TypeData.Data.BooleanTypeCode;
    protected uint RationalTypeCode => TypeData.Data.RationalTypeCode;
    protected int TypeCodeOffsetInHeader => TypeData.Data.TypeCodeOffsetInHeader;
    protected void Emit(Opcode Op) => Emit(Op, new uint[0]);
    protected void Emit(Opcode Op, params uint[] args) => Emit(Op, args.Select(x => (UIntOpArg)x).ToArray());
    protected void Emit(Opcode Op, params IOperationArgument<uint>[] args) => Emit(new Operation<uint>(new OpcodeWrapper(Op), args));
    protected void Emit(Operation<uint> Instruction)
    {
        Driver.Instructions.Add(Instruction);
    }
    protected uint WriteStaticData(uint Typecode, uint[] Data) => WriteStaticData((ushort)Typecode, (ushort)Data.Length, Data);
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
    protected uint GetDestRegister() => Driver.DestinationRegister ?? ++Driver.LastUsedRegister;
    protected CodeGenVisitor Driver = driver;
    public abstract void GenerateCode(Node? parent, Node self);
}