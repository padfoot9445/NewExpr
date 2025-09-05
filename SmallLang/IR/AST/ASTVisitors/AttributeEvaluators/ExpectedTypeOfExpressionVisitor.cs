using System.Collections.Immutable;
using System.Runtime.InteropServices.JavaScript;
using Common.AST;
using SmallLang.Exceptions;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class ExpectedTypeOfExpressionVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        AssertPropertyIsNotNull<IHasAttributeGenericSLType>(x => x.GenericSLType is not null);
        base.PreVisit(node);
    }

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        var FunctionDefinitionNode = CurrentRootNode.Flatten()
            .OfType<FunctionNode>()
            .Single(x => x.FunctionName.VariableName == self.Identifier.VariableName);

        var IndexableArgList =
            FunctionDefinitionNode.TypeAndIdentifierCSV.Select(x => x.Type.GetGenericSLTFromLiteralType()).ToImmutableArray();

        var FunctionDefinitionArgList = FunctionDefinitionNode.TypeAndIdentifierCSV
            .Select(x => (x.Identifier.Data.Lexeme, x.Type.GetGenericSLTFromLiteralType()))
            .ToDictionary();

        bool SeenIdentifier = false;
        foreach (var (i, v) in self.ArgList.Index())
        {
            if (v.Identifier is not null)
            {
                SeenIdentifier = true;


                FunctionDefinitionArgList.TryGetValue(v.Identifier.Data.Lexeme, out var expectedTypeOfExpression);
                v.ExpectedTypeOfExpression =
                    expectedTypeOfExpression ??
                    throw new ExpaException($"Argument label {v.Identifier.Data} was not recognized.");
            }
            else
            {
                if (SeenIdentifier)
                    throw new ExpaException(
                        $"ERROR at {v.Expression}: Positional parameters must be before any named parameters.");
                else
                {
                    v.ExpectedTypeOfExpression = IndexableArgList[i];
                }
            }
        }
        return base.VisitFunctionCall(Parent, self);
    }
}