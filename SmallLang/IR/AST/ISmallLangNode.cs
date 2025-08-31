using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST;

public interface ISmallLangNode : IHasAttributeScope
{
    bool Equals(object? obj);
    bool Equals(SmallLangNode? other);
    int GetHashCode();
    string ToString();
    IEnumerable<ISmallLangNode> ChildNodes { get; }
}