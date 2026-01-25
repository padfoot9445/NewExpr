using System.Data;
using System.Diagnostics.CodeAnalysis;
using SmallLang.IR.Metadata;

namespace SmallLang.TreeWalkInterpreter;

public class ControlException(bool isBreak, bool isContinue, bool hasLabel, VariableName? dst) : Exception
{

  public bool BreakFlag => isBreak; //set to true when you want the looping method to break.
  public bool HasLabel => hasLabel;
  public bool ContinueFlag => isContinue;

  public bool TryGetDst([NotNullWhen(true)] out VariableName? Dst)
  {
    if (HasLabel)
    {
      Dst = dst ?? throw new NoNullAllowedException();
      return true;
    }

    Dst = null;
    return false;

  }
}