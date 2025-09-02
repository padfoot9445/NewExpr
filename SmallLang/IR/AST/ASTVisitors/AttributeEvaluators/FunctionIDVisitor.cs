using System.Diagnostics.CodeAnalysis;
using Common.Metadata;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

using FunctionID = FunctionID<BackingNumberType>;
using FunctionSignature = FunctionSignature<BackingNumberType, SmallLangType>;
internal class FunctionIDVisitor : BaseASTVisitor
{
    protected override ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self)
    {
        NotNull(self.Scope, self.FunctionName.VariableName, self.Type.TypeLiteralType);
        self.Scope.DefineFunction(new FunctionSignature(self.FunctionName.Data.Lexeme, FunctionID.GetNext(), self.Type.TypeLiteralType, self.TypeAndIdentifierCSV.Select(x => x.Type.TypeLiteralType!).ToList()));

        return base.VisitFunction(Parent, self);
    }

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        NotNull(self.Scope);

        if (self.Scope.FunctionIsDefined(self.Identifier.Data.Lexeme))
        {
            self.FunctionID = self.Scope.GetID(self.Identifier.Data.Lexeme);
        }

        return base.VisitFunctionCall(Parent, self);
    }
}