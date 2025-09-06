using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class DeclarationVisitor
{
    internal static void Visit(DeclarationNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            var slot = Driver.Data.AllocateRegisters(Self.VariableName!, (int)Self.Type.TypeLiteralType!.Size);

            if (Self.AssignmentPrime is not null)
            {
                Driver.Cast(Self.AssignmentPrime, Self.Type.TypeLiteralType);
                Driver.Emit(HighLevelOperation.LoadFromStack(slot, Self.Type.TypeLiteralType!.Size));
            }

            Driver.Next();
        });
    }
}