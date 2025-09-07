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
        List<(int, uint)> registerAndWidths = [];


        foreach (var (type, variableName) in Self.TypeAndIdentifierCSV.Select(x => (x.Type.TypeLiteralType!, x.Identifier.VariableName!)))
        {
            int StartRegister = Driver.Data.AllocateRegisters(variableName, (int)type.Size);
            registerAndWidths.Add((StartRegister, type.Size));
        }

        Driver.NewChunk(1, () =>
        {
            //load arguments from stack into registers.
            Driver.Exec(Self.FunctionBody);
            foreach (var (register, width) in (registerAndWidths as IEnumerable<(int, uint)>).Reverse())
            {
                Driver.Emit(HighLevelOperation.LoadFromStack(register, width));
            }
        });

        Debug.Assert(Driver.Data.CurrentChunk.GetParent(Driver.Data.Sections) == chunk);

        Driver.NewChunk(2, () => { Driver.Next(); });


        Debug.Assert(Driver.Data.CurrentChunk.GetParent(Driver.Data.Sections) == chunk);
    }
}