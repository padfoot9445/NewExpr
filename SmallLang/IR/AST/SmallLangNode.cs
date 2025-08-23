using System.Collections;

namespace SmallLang.IR.AST;

public abstract record class SmallLangNode : IEnumerable<ISmallLangNode>, ISmallLangNode
{
    protected abstract IEnumerable<ISmallLangNode?> Children { get; set; }
    public IEnumerator<ISmallLangNode> GetEnumerator()
    {
        return Children.Where(x => x is not null).GetEnumerator()!;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}