using SmallLang.CodeGen.Frontend;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;

namespace SmallLang.Drivers;

public class HighToLowLevelCompilerDriver
{
    public static Data Compile(string Code, Func<SmallLangNode, CodeGenerator>? GetCodeGenerator = null)
    {
        GetCodeGenerator ??= x => new CodeGenerator(x);
        var Ast = new Parser.Parser(Code).Parse<SectionNode>();
        var CodeGenerator = GetCodeGenerator(Ast);
        return CodeGenerator.Parse();
    }
}