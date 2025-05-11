using NUnit.Framework.Constraints;
using SmallLang;
using SmallLang.LinearIR;

namespace SmallLangTest.BackendComponentTests;
[TestFixture]
public class FunctionCall
{
    [Test]
    public void InputFunctionCall__NoArgs_RetStack__GeneratesCorrectResult()
    {
        var res = HighToLowLevelCompilerDriver.Compile(HTLDriverTest.Program1, () => new FunctionCallDriverMock(false)).Item1;
        Assert.That(res[0].Op.Value, Is.EqualTo((uint)(Opcode.ICallS)));
        Assert.That(res[0].Operands[0].Value, Is.EqualTo(1));
    }
    [Test]
    public void InputFunctionCall__NoArgs_RetReg__GeneratesCorrectResult()
    {

        var res = HighToLowLevelCompilerDriver.Compile(HTLDriverTest.Program1, () => new FunctionCallDriverMock(true)).Item1;
        Assert.That(res[0].Op.Value, Is.EqualTo((uint)(Opcode.ICallR)));
        Assert.That(res[0].Operands[0].Value, Is.EqualTo(1));
        Assert.That(res[0].Operands[1].Value, Is.EqualTo(1));
    }
    [Test]
    public void InputFunctionCall__Args__Throws()
    {
        Assert.Throws<ExpaException>(() => HighToLowLevelCompilerDriver.Compile("input(1);", () => new FunctionCallDriverMock(false)));
    }
}