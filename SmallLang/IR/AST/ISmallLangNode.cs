namespace SmallLang.IR.AST;

public interface ISmallLangNode
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    int GetHashCode();
    string ToString();
    IEnumerable<ISmallLangNode> ChildNodes { get; }
    T AcceptVisitor<T>(ISmallLangNodeVisitor<T> visitor);
}