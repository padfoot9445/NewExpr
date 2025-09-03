namespace RunStuff;

using SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;
using SmallLangTest.AttributeVisitorTests;
public static class Program
{

    static GUIDOfTargetLoopVisitor visitor = new GUIDOfTargetLoopVisitor();
    static List<(SmallLang.IR.AST.ISmallLangNode, string)> Cases = GUIDOfTargetLoopVisitorTests.GetTestCases().ToList();

    static void Main()
    {
        for (int i = 0; i < 1000; i++)
        {
            foreach (var Case in Cases)
            {
                visitor.BeginVisiting(Case.Item1);
            }
        }

        Console.WriteLine("Finished");
    }

}