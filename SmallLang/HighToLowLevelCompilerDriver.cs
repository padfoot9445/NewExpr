using Common.Evaluator;
using Common.LinearIR;
using SmallLang.Backend;

namespace SmallLang;
public class HighToLowLevelCompilerDriver
{
    public static Operation<uint>[] Compile(string Code)
    {
        var Ast = new Parser.Parser(Code).Parse();
        var Evaluator = new DynamicASTEvaluator();
        var AttributeAnalyser = new AttributeVisitor();
        var Optimiser = new OptimisingVisitor();
        Evaluator.Evaluate(Ast, AttributeAnalyser);
        Evaluator.Evaluate(Ast, Optimiser);
        var Generator = new CodeGenVisitor();
        Generator.Dispatch(Ast)(null, Ast);
        return Generator.Instructions.ToArray();
    }
}