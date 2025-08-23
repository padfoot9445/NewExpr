using System.Collections;

namespace SmallLang.IR.AST;

public abstract record class SmallLangNode : IEnumerable<SmallLangNode>, ISmallLangNode
{
    protected abstract IEnumerable<SmallLangNode?> Children { get; set; }
    public IEnumerator<SmallLangNode> GetEnumerator()
    {
        return Children.Where(x => x is not null).GetEnumerator()!;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}