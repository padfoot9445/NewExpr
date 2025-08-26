namespace SmallLang.IR.AST;

public interface ISmallLangNode
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    int GetHashCode();
    string ToString();
}