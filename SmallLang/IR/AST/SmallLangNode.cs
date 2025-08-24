using System.Collections;

namespace SmallLang.IR.AST;

public abstract record class SmallLangNode : ISmallLangNode
{
    protected abstract IEnumerable<ISmallLangNode?> Children { get; set; }
    public IEnumerable<ISmallLangNode> ChildNodes => Children.Where(x => x != null).Select(x => x!);

    public abstract T AcceptVisitor<T>(ISmallLangNode? Parent, ISmallLangNodeVisitor<T> visitor);
}