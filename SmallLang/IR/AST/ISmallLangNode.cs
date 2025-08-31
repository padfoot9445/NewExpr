using Common.AST;
using SmallLang.IR.AST.Generated;

using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST;

public interface ISmallLangNode : ITreeNode<ISmallLangNode>, IHasAttributeScope
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    int GetHashCode();
    string ToString();
    T AcceptVisitor<T>(ISmallLangNode? Parent, ISmallLangNodeVisitor<T> visitor);
}