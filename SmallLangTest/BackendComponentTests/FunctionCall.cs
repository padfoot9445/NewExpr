using SmallLang;
using SmallLang.LinearIR;

namespace SmallLangTest.BackendComponentTests;
[TestFixture]
public class FunctionCall
{
    [Test]
    public void InputFunctionCall__NoArgs__GeneratesCorrectResult()
    {
        var res = HighToLowLevelCompilerDriver.Compile(HTLDriverTest.Program1, () => new FunctionCallDriverMock());
        Assert.That(res[0].Op.Value, Is.EqualTo((uint)(Opcode.ICall)));
        Assert.That(res[0].Operands[0].Value, Is.EqualTo(1));
    }
}