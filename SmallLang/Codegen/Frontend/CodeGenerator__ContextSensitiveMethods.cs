using System.Diagnostics;
using JetBrains.Annotations;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend;

partial class CodeGenerator
{
    private bool IsNextFlag { get; set; }
    private bool NextWasCalledFlag { get; set; }
    private bool IsInChunkFlag { get; set; }
    private Stack<(bool IsNextCopy, bool NextWasCalledCopy, bool InChunkCopy)> Stack = new();

    private void StoreState()
    {
        Stack.Push((IsNextFlag, NextWasCalledFlag, IsInChunkFlag));
    }
    private void RestoreState()
    {
        (IsNextFlag, NextWasCalledFlag, IsInChunkFlag) = Stack.Pop();
    }
    private bool InChunk([InstantHandle] Action Code)
    {
        StoreState();
        IsNextFlag = false;
        IsInChunkFlag = true;
        Code();

        var ret = IsNextFlag;

        RestoreState();
        if (ret) NextWasCalledFlag = true;

        return ret;
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
        if (!InChunk(code)) Data.Rewind();
    }

    internal void Next()
    {
        if (!IsInChunkFlag) throw new InvalidOperationException("Must be in a chunk to call Next");
        IsNextFlag = true;
    }

    internal Action<ISmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
        where T : ISmallLangNode
    {
        return (x, y) =>
        {
            StoreState();
            Verify<T>(x);
            visitor((T)x, y);
            if (!NextWasCalledFlag)
            {
                throw new Exception($"Next must be called at some point within the Visitor {typeof(T)}. Call Next.");
            }
            RestoreState();
        };
    }
}