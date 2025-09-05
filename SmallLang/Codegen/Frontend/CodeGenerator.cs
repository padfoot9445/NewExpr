using System.Diagnostics;
using System.Numerics;
using Common.Dispatchers;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator(SmallLangNode RootNode)
{
    internal const BackingNumberType TrueValue = BackingNumberType.MaxValue;
    internal const BackingNumberType FalseValue = BackingNumberType.MinValue;

    internal Data Data { get; init; } = new();

    internal void Cast<T>(T self, GenericSmallLangType dstType)
        where T : IHasAttributeTypeOfExpression, ISmallLangNode
    {
        if (self.TypeOfExpression! == dstType) Exec(self);
        throw new NotImplementedException();
    }


    internal void Cast<T>(T self, SmallLangType dstType)
        where T : IHasAttributeTypeOfExpression, ISmallLangNode
    {
        Cast(self, new GenericSmallLangType(dstType));
    }

    public Data Parse()
    {
        Exec(RootNode);
        return Data;
    }

    internal static void Verify<T>(ISmallLangNode node) where T : ISmallLangNode
    {
        Debug.Assert(node is T);
    }

    internal void Exec(ISmallLangNode node)
    {
        var CurrentChunk = Data.CurrentChunk;
        DynamicDispatch(node)(node, this);
    }

    internal int[] GetRegisters(int Width = 1)
    {
        return Enumerable.Range(0, Width).Select(_ => Data.GetRegister()).ToArray();
    }

    internal int[] GetRegisters<T>(T Width) where T : INumber<T>
    {
        return GetRegisters(int.CreateTruncating(Width));
    }

    internal int[] GetRegisters(GenericSmallLangType Type)
    {
        return GetRegisters(Type.Size);
    }

    internal int[] GetRegisters(SmallLangType Type)
    {
        return GetRegisters(Type.Size);
    }

    internal int[] GetRegisters(IHasAttributeTypeOfExpression Node)
    {
        return GetRegisters((int)Node.TypeOfExpression!.Size);
    }

    internal TreeChunk GetChild(int ChunkID)
    {
        return Data.CurrentChunk.Children[ChunkID - 1];
    }

    private (Func<ISmallLangNode, bool>, Action<ISmallLangNode, CodeGenerator>) GetCase<T>(
        Action<T, CodeGenerator> Visitor)
        where T : ISmallLangNode
    {
        return (x => x is T, VisitFunctionWrapper(Visitor));
    }

    private Action<ISmallLangNode, CodeGenerator> DynamicDispatch(ISmallLangNode node)
    {
        return node.Dispatch(
            x => x,
            GetCase<SectionNode>(SectionVisitor.Visit),
            GetCase<IdentifierNode>(PrimaryVisitor.VisitIdentifier),
            GetCase<FunctionNode>(FunctionVisitor.Visit),
            GetCase<ForNode>(ForVisitor.Visit),
            GetCase<WhileNode>(WhileVisitor.Visit),
            GetCase<ReturnNode>(ReturnVisitor.Visit),
            GetCase<LoopCTRLNode>(LoopCtrlVisitor.Visit),
            GetCase<SwitchNode>(SwitchVisitor.Visit),
            GetCase<IfNode>(IfVisitor.Visit),
            GetCase<PrimaryNode>(PrimaryVisitor.Visit),
            GetCase<DeclarationNode>(DeclarationVisitor.Visit),
            GetCase<FactorialExpressionNode>(FactorialExpressionVisitor.Visit),
            GetCase<ElseNode>(ElseVisitor.Visit),
            GetCase<BinaryExpressionNode>(BinaryExpressionVisitor.Visit),
            GetCase<ComparisonExpressionNode>(ComparisonExpressionVisitor.Visit),
            GetCase<CopyExprNode>(CopyExpressionVisitor.Visit),
            GetCase<IndexNode>(IndexVisitor.Visit),
            GetCase<FunctionCallNode>(FunctionCallVisitor.Visit),
            GetCase<UnaryExpressionNode>(UnaryExpressionVisitor.Visit),
            GetCase<NewExprNode>(NewExpressionVisitor.Visit)
        );
    }
}