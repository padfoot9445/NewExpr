using System.Runtime.InteropServices;
using Common.AST;
using Common.Logger;

namespace Parser;
partial class Parser
{
    class SafeParser
    {

        Stack<int> State = new();
        ILogger Log { get; init; }
        public SafeParser(ILogger logger)
        {
            Log = logger;
        }
        #region SafeParse
        void SaveState(ref int Current)
        {
            State.Push(Current);
        }
        void RollBack(ref int Current)
        {
            Current = State.Pop();
        }
        public bool SafeParse(ParsingFunction fn, out AnnotatedNode<Annotations>? K, ref int Current, bool Suppress = true) //return true on successful parse, else false. Node is undefined on faliure.
        {
            SaveState(ref Current);
            if (Suppress)
            {
                Log.SuppressLog();
            }
            bool Result = fn(out AnnotatedNode<Annotations>? Node);
            Log.EnableLog();
            if (Result)
            {
                State.Pop();
                K = Node;
                return true;
            }
            else
            {
                RollBack(ref Current);
                K = null;
                return false;
            }
        }
        #endregion

    }
}