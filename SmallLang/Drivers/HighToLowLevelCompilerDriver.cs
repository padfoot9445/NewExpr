using SmallLang.CodeGen.Frontend;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
namespace SmallLang.Drivers;

public class HighToLowLevelCompilerDriver
{
    public static Data Compile(string Code, Func<SmallLangNode, CodeGenerator>? GetCodeGenerator = null)
    {
        GetCodeGenerator ??= (x) => new CodeGenerator(x);
        var Ast = new Parser.Parser(Code).Parse<SectionNode>();
        // var Generator = GetCodeGenerator(Ast);
        // Generator.Dispatch(Ast)(null, Ast);
        // return (Generator.Instructions.ToArray(), Generator.StaticData.ToArray());
        throw new NotImplementedException();
    }
}