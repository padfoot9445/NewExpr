namespace SmallLang.IR.AST;

public interface ISmallLangNode
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    IEnumerator<SmallLangNode> GetEnumerator();
    int GetHashCode();
    string ToString();
}