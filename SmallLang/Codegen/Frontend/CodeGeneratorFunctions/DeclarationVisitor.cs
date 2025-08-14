using System.Diagnostics;
using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static ImportantASTNodeType;
using static Opcode;

internal static class DeclarationVisitor
{
    internal static void Visit(Node Self, CodeGenerator Driver)
    {
        Driver.SETCHUNK();

        Node Type = Self.Children[0].NodeType == DeclarationModifiersCombined ? Self.Children[1] : Self.Children[0];
        Debug.Assert(Type.NodeType == BaseType || Type.NodeType == GenericType);
        Node? AssignmentPrime = Self.Children.Last().NodeType == ImportantASTNodeType.AssignmentPrime ? Self.Children.Last() : null;

        //ENTERING CHUNK
        if (AssignmentPrime is not null)
        {
            //this should be the first time this identifier is used
            Driver.Cast(AssignmentPrime.Children[0], Type.Attributes.TypeLiteralType!);
            var ptr = Driver.Data.VariableSlots.Allocate((int)Type.Attributes.TypeLiteralType!.Size);
            Driver.Emit<int, uint>(LoadVar, ptr, Type.Attributes.TypeLiteralType.Size);
        }
    }
}