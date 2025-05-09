using Common.Evaluator;
using Common.LinearIR;
using SmallLang.Backend;

namespace SmallLang;
public class HighToLowLevelCompilerDriver
{
    public static Operation<uint>[] Compile(string Code, Func<CodeGenVisitor>? GetCodeGenVisitor = null)
    {
        GetCodeGenVisitor ??= () => new CodeGenVisitor();
        var Ast = new Parser.Parser(Code).Parse();
        var Evaluator = new DynamicASTEvaluator();
        var AttributeAnalyser = new AttributeVisitor();
        var Optimiser = new OptimisingVisitor();
        Evaluator.Evaluate(Ast, AttributeAnalyser);
        Evaluator.Evaluate(Ast, Optimiser);
        var Generator = GetCodeGenVisitor();
        Generator.Dispatch(Ast)(null, Ast);
        return Generator.Instructions.ToArray();
    }
}