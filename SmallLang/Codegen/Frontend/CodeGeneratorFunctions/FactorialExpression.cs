using Common.Tokens;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions.PrimaryParserSubFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static ImportantASTNodeType;
internal static class FactorialExpressionVisitor
{
    internal static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.SETCHUNK();

        //ENTERING CHUNK
        Driver.Exec(Self.Children[0]);
        Driver.Emit(HighLevelOperation.Factorial<BackingNumberType, int>(Self.Children[0].Attributes.TypeOfExpression!, Self.Children.Count - 1));

    }
}