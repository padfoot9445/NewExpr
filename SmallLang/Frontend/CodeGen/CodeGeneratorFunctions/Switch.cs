using System.Diagnostics;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Frontend.CodeGen;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseSwitch(Node Self)
    {
        SETCHUNK();
        Node Expression = Self.Children[0];
        var Expressions = Self.Children.Skip(1).Select(x => x.Children[0]).ToArray();
        var Statements = Self.Children.Skip(1).Select(x => x.Children[1]).ToArray();
        int Length = Self.Children.Count - 1;

        //ENTERING CHUNK CHUNK0
        DynamicDispatch(Expression);
        Emit(JMP, ACHUNK(Length * 2 + 1));

        for (int i = 1; i <= Length; i++)
        {

            //CHUNK [i * 2 - 1]
            NewChunk();
            DynamicDispatch(Expressions[i - 1]);


            //CHUNK [i * 2]
            NewChunk();
            DynamicDispatch(Statements[i - 1]);
        }


        //CHUNK [LENGTH * 2 + 1]
        NewChunk();
        Emit<int, int>(SWITCH, Length, ACHUNK(0));


        //CHUNK [LENGTH * 2 + 2]
        NewChunk();
        //NEXT
    }
}