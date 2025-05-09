using System.Reflection.Emit;
using Common.LinearIR;
using SmallLang;
using SmallLang.LinearIR;

namespace SmallLangTest;
[TestFixture]
public class SmallLangTest
{
    [Test]
    public void InputFunctionCall__NoArgs__GeneratesCorrectResult()
    {
        var res = HighToLowLevelCompilerDriver.Compile(HTLDriverTest.Program1).Item1;
        Assert.That(res[0].Op.Value, Is.EqualTo((uint)(Opcode.ICallS)));
        Assert.That(res[0].Operands[0].Value, Is.EqualTo(1));
    }
}