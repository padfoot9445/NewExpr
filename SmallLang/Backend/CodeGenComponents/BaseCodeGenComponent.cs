using Common.AST;
using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.Backend.CodeGenComponents;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
abstract class BaseCodeGenComponent(CodeGenVisitor driver)
{
    protected void Emit(Opcode Op) => Emit(Op, new uint[0]);
    protected void Emit(Opcode Op, params uint[] args) => Emit(Op, args.Select(x => (UIntOpArg)x).ToArray());
    protected void Emit(Opcode Op, params IOperationArgument<uint>[] args) => Emit(new Operation<uint>(new OpcodeWrapper(Op), args));
    protected void Emit(Operation<uint> Instruction)
    {
        Driver.Instructions.Add(Instruction);
    }
    protected CodeGenVisitor Driver = driver;
    public abstract void GenerateCode(Node? parent, Node self);
}