using Common.AST;

namespace SmallLang.IR.AST;

public interface ISmallLangNode : ITreeNode<ISmallLangNode>
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    int GetHashCode();
    string ToString();
    T AcceptVisitor<T>(ISmallLangNode? Parent, ISmallLangNodeVisitor<T> visitor);
}