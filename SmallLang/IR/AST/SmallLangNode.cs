using System.Collections;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST;

public abstract record class SmallLangNode : ISmallLangNode, IHasAttributeScope, IHasAttributeScopeSettable
{
    protected abstract IEnumerable<ISmallLangNode?> Children { get; set; }
    public IEnumerable<ISmallLangNode> ChildNodes => Children.Where(x => x != null).Select(x => x!);

    public abstract T AcceptVisitor<T>(ISmallLangNode? Parent, ISmallLangNodeVisitor<T> visitor);

    public Scope? Scope { get; set; } = null;
}