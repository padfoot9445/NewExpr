using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(BaseTypeNode node)
    {
        node.TypeLiteralType = TypeData.GetType(node.Data.Lexeme);
    }
}