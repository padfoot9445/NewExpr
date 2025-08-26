using System.Collections;

namespace SmallLang.IR.AST;

public abstract record class SmallLangNode : ISmallLangNode
{
    protected abstract IEnumerable<ISmallLangNode?> Children { get; set; }
}