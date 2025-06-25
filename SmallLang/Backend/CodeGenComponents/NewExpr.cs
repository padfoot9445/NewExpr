using System.Diagnostics;
using System.Reflection.Emit;
using Common.AST;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Constants;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Backend.CodeGenComponents;

class NewExpr : BaseCodeGenComponent
{
    public NewExpr(CodeGenVisitor driver) : base(driver)
    {
    }

    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        var Arguments = self.Children.Count == 2 ? self.Children[1] : null;
        int Count = 0;
        if (Arguments is not null)
        {

            PushArgsToStack(Arguments, self);
            Count = Arguments.Children.Count;
        }
        //TODO: Implement arg labels
        var type = self.Children[0];
        Debug.Assert(type.Children[0].NodeType == ImportantASTNodeType.TypeCSV);
        var innertype = type.Children[0];
        Emit(innertype.Children.Select(x => TypeData.Data.IsPointerType(x.Attributes.TypeLiteralType!)).Any() ? Opcode.NewP : Opcode.NewD, type.Attributes.TypeLiteralType!, (uint)Count);
    }
}