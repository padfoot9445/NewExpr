using SmallLang.CodeGen.Frontend;
using SmallLang.IR.AST;
using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLang.IR.AST.Generated;

namespace SmallLang.Drivers;

public class HighToLowLevelCompilerDriver
{
    protected HighToLowLevelCompilerDriver() { }
    public static Data Compile(string Code, Func<SmallLangNode, CodeGenerator>? GetCodeGenerator = null)
    {
        GetCodeGenerator ??= x => new CodeGenerator(x);
        var Ast = new Parser.Parser(Code).Parse<SectionNode>();
        new AttributeEvaluator().BeginVisiting(Ast);
        var CodeGenerator = GetCodeGenerator(Ast);
        return CodeGenerator.Parse();
    }
}