using System.Collections.Immutable;
using Common.AST;
using SmallLang.Exceptions;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class ExpectedTypeOfExpressionVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        AssertPropertyIsNotNull<IHasAttributeGenericSLType>(x => x.GenericSLType is not null);
        AssertPropertyIsNotNull<IHasAttributeTypeLiteralType>(x => x.TypeLiteralType is not null);
        base.PreVisit(node);
    }

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        ImmutableArray<GenericSmallLangType> IndexableArgList;
        Dictionary<string, GenericSmallLangType> FunctionDefinitionArgList;

        if (Functions.StdLibFunctions.Select(x => x.Name).Contains(self.Identifier.Data.Lexeme))
        {
            IndexableArgList = Functions.StdLibFunctions.Single(x => x.Name == self.Identifier.Data.Lexeme)
                .ArgTypes.ToImmutableArray();
            FunctionDefinitionArgList = [];
        }
        else
        {
            var FunctionDefinitionNode = CurrentRootNode.Flatten()
                .OfType<FunctionNode>()
                .Single(x => x.FunctionName.VariableName == self.Identifier.VariableName);
            //ReSharper disable All
            IndexableArgList =
                FunctionDefinitionNode.TypeAndIdentifierCSV.Select(x => x.Type.GetGenericSLTFromLiteralType())
                    .ToImmutableArray();
            //ReSharper enable All

            FunctionDefinitionArgList = FunctionDefinitionNode.TypeAndIdentifierCSV
                .Select(x => (x.Identifier.Data.Lexeme, x.Type.GetGenericSLTFromLiteralType()))
                .ToDictionary();
        }

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

    protected override ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self)
    {
        if (self.Type.TypeLiteralType!.OutmostType == TypeData.Dict)
        {
            foreach (var (i, arg) in self.ArgList.Index())
            {
                arg.ExpectedTypeOfExpression = self.Type.TypeLiteralType.ChildNodes.ElementAt(i % 2 == 0 ? 1 : 0);
            }
        }
        else
        {
            bool seenSize = false;
            foreach (var arg in self.ArgList)
            {
                if (arg.Identifier is not null && arg.Identifier.Data.Lexeme == "Size")
                {
                    if (seenSize) throw new ExpaException("Cannot specify parameter Size twice");
                    arg.ExpectedTypeOfExpression = new(TypeData.Int);
                    seenSize = true;
                }
                else
                {
                    arg.ExpectedTypeOfExpression = self.Type.TypeLiteralType.ChildNodes.First();
                }
            }
        }

        return base.VisitNewExpr(Parent, self);
    }
}