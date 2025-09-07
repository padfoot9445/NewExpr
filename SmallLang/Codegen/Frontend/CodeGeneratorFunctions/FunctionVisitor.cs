using System.Collections;
using System.Diagnostics;
using Common.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionVisitor
{
    public static void Visit(FunctionNode Self, CodeGenerator Driver)
    {
        var chunk = Driver.Data.CurrentChunk;
        Driver.EnteringChunk(() =>
        {
            //assume that Entering chunk does not have JMP CHUNK1
        });
        Console.WriteLine("FunctionVisitor!");
        Debug.Assert(chunk == Driver.Data.CurrentChunk);
        Console.WriteLine("T1 executed");
        List<(int, uint)> registerAndWidths = [];


        foreach (var (type, variableName) in Self.TypeAndIdentifierCSV.Select(x => (x.Type.TypeLiteralType!, x.Identifier.VariableName!)))
        {
            int StartRegister = Driver.Data.AllocateRegisters(variableName, (int)type.Size);
            registerAndWidths.Add((StartRegister, type.Size));
        }

        Driver.NewChunk(1, () =>
        {
            Debug.Assert(Driver.Data.CurrentChunk.GetParent(Driver.Data.Sections) == chunk, Driver.Data.CurrentChunk.GetParent(Driver.Data.Sections)!.ToString());
            Console.WriteLine("T2 executed");

            //load arguments from stack into registers.
            var chunk2 = Driver.Data.CurrentChunk;
            Driver.Exec(Self.FunctionBody);
            Debug.Assert(Driver.Data.CurrentChunk == chunk2);
            Console.WriteLine("T2.1 executed");

            foreach (var (register, width) in (registerAndWidths as IEnumerable<(int, uint)>).Reverse())
            {
                Driver.Emit(HighLevelOperation.LoadFromStack(register, width));
            }
        });

        Debug.Assert(Driver.Data.CurrentChunk == chunk);
        Console.WriteLine("T2.2 executed");

        Driver.NewChunk(2, () => { Driver.Next(); });


        Debug.Assert(Driver.Data.CurrentChunk == chunk);

        Console.WriteLine("All tests executed");
    }
}