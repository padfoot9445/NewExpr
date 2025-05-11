using System.Diagnostics;
using NUnit.Framework.Constraints;
using SmallLang;
using SmallLang.CsIntepreter;
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
    [Test]
    public void SOutFunctionCall__NoArgs__Throws()
    {

        Assert.Throws<ExpaException>(() => HighToLowLevelCompilerDriver.Compile("SOut();", () => new FunctionCallDriverMock(false)));
    }
    [Test]
    public void SOutFunctionCall__WrongArgs__Throws()
    {

        Assert.Throws<ExpaException>(() => HighToLowLevelCompilerDriver.Compile("SOut(1);", () => new FunctionCallDriverMock(false)));
    }
    [Test]
    public void SOutFunctionCall__RightArgs__DoesNotThrow()
    {

        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile("SOut(\"abc\");", () => new FunctionCallDriverMock(false)));
    }
    [Test]
    public void SOutFunctionCall__RightArgs__OutputsCorrect()
    {
        (var res, var data) = HighToLowLevelCompilerDriver.Compile("SOut(\"abc\");", () => new FunctionCallDriverMock(false));
        var Out = new CustomTextWriter();
        var interp = new Interpreter(res, data, new StreamReader(Stream.Null), Out);
        interp.Interpret();
        Assert.That(Out.outStore.ToString(), Is.EqualTo("abc\r\n"));
    }
}