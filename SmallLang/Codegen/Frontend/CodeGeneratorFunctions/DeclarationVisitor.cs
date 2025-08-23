using System.Diagnostics;
using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryVisitorSubFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;


internal static class DeclarationVisitor
{

    internal static void Visit(DeclarationNode Self, CodeGenerator Driver)
    {

        Driver.EnteringChunk(() =>
        {
            var slot = Driver.Data.AllocateRegisters(Self.Attributes.VariableName!, (int)Self.Type.Attributes.TypeLiteralType!.Size);

            if (Self.AssignmentPrime is not null)
            {
                Driver.Cast(Self.AssignmentPrime, Self.Type.Attributes.TypeLiteralType);
                Driver.Emit(HighLevelOperation.LoadFromStack(slot, Self.Type.Attributes.TypeLiteralType!.Size));
            }

            Driver.Next();
        });

    }
}