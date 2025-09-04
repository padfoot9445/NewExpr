using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class TypeLiteralTypeVisitor : BaseASTVisitor
{
    protected override ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self)
    {
        self.TypeLiteralType = GenericSmallLangType.ParseType(self);
        return base.VisitBaseType(Parent, self);
    }

    protected override ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self)
    {
        self.TypeLiteralType = GenericSmallLangType.ParseType(self);
        return base.VisitGenericType(Parent, self);
    }
}