using System.Diagnostics;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST;

public static class ASTExtensions
{
    public static GenericSmallLangType GetGenericSLTFromLiteralType(this ITypeNode type)
    {
        Debug.Assert(type.TypeLiteralType is not null);
        return GenericSmallLangType.ParseType(type);
    }
}