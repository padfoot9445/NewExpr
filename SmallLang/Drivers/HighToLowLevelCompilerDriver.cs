using Common.Evaluator;
using Common.LinearIR;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang;
#if false
public class HighToLowLevelCompilerDriver
{
    public static (Operation<Opcode, BackingNumberType>[], uint[]) Compile(string Code, Func<CodeGenVisitor>? GetCodeGenVisitor = null)
    {
        throw new NotImplementedException();
        GetCodeGenVisitor ??= () => new CodeGenVisitor();
        var Ast = new Parser.Parser(Code).Parse();
        var Evaluator = new DynamicASTEvaluator();
        var AttributeAnalyser = new AttributeVisitor();
        var Optimiser = new PostProcessingVisitor();
        Evaluator.Evaluate(Ast, AttributeAnalyser);
        Evaluator.Evaluate(Ast, Optimiser);
        var Generator = GetCodeGenVisitor();
        Generator.Dispatch(Ast)(null, Ast);
        return (Generator.Instructions.ToArray(), Generator.StaticData.ToArray());
    }
}
#endif