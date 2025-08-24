using System.Diagnostics;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend;

partial class CodeGenerator
{
    private bool IsNextFlag { get; set; }
    private bool NextWasCalledFlag { get; set; }
    private bool IsInChunkFlag { get; set; } = false;

    private bool InChunk(Action Code)
    {
        IsNextFlag = false;
        IsInChunkFlag = true;
        Code();
        IsInChunkFlag = false;

        var ret = IsNextFlag;

        if (ret)
        {
            NextWasCalledFlag = true;
        }

        IsNextFlag = false;
        return ret;
    }

    internal void Emit(HighLevelOperation Op)
    {
        if (!IsInChunkFlag) throw new InvalidOperationException("Must be in a chunk to call Driver.Emit. Wrap the emit call in a suitable chunk.");
        Data.Emit(Op);
    }
    internal void EnteringChunk(Action code) => InChunk(code);

    internal void NewChunk(int chunkID, Action code)
    {
        Debug.Assert(Data.CurrentChunk.Children.Count == (chunkID - 1));
        Data.NewChunk();
        if (!InChunk(code))
        {
            Data.Rewind();
        }
    }

    internal void Next()
    {
        if (!IsInChunkFlag) throw new InvalidOperationException("Must be in a chunk to call Next");
        IsNextFlag = true;
    }

    internal Action<ISmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
where T : ISmallLangNode =>
    (x, y) =>
    {
        Verify<T>(x);
        visitor((T)x, y);
        if (NextWasCalledFlag)
        {
            NextWasCalledFlag = false;
        }
        else
        {
            throw new Exception("Next must be called at some point within the Visitor. Call Next.");
        }
    };
}