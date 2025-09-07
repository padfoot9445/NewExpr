using System.Diagnostics;
using JetBrains.Annotations;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend;

partial class CodeGenerator
{
    private bool IsInChunkFlag { get; set; }
    private Stack<bool> Stack = new();

    private void StoreState()
    {
        Stack.Push(IsInChunkFlag);
    }
    private void RestoreState()
    {
        IsInChunkFlag = Stack.Pop();
    }
    private bool InChunk([InstantHandle] Action Code)
    {
        StoreState();
        IsInChunkFlag = true;
        Code();


        RestoreState();

        return true;
    }

    internal void Emit(HighLevelOperation Op)
    {
        if (!IsInChunkFlag)
            throw new InvalidOperationException(
                "Must be in a chunk to call Driver.Emit. Wrap the emit call in a suitable chunk.");
        Data.Emit(Op);
    }

    internal void EnteringChunk([InstantHandle] Action code)
    {
        InChunk(code);
    }

    internal void NewChunk(int chunkID, [InstantHandle] Action code)
    {
        Debug.Assert(Data.CurrentChunk.Children.Count == chunkID - 1);
        Data.NewChunk();
        InChunk(code);
        Data.Rewind(); //always rewind if we made a new chunk, and never rewind if we didn't, maybe?
    }
    internal int WrapperChunk(HighLevelOperation chunkOp, [InstantHandle] Action code)
    {
        //do not set InChunk as we do not want any code to be in this treechunk's chunk

        //TODO: wrap all newchunk sequences as in ForVisitor
        var Return = Data.CurrentChunk.NumberOfChildren;

        NewChunk(Data.CurrentChunk.NumberOfChildren, () =>
        {
            Emit(chunkOp);
            StoreState();
            IsInChunkFlag = false;
            code();
            RestoreState();
        });

        return Return;
    }
    internal Action<ISmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
        where T : ISmallLangNode
    {
        return (x, y) =>
        {
            StoreState();
            Verify<T>(x);
            visitor((T)x, y);

            RestoreState();
        };
    }
}