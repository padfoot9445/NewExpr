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

        Assert.Throws<TypeErrorException>(() => HighToLowLevelCompilerDriver.Compile("SOut(1);", () => new FunctionCallDriverMock(false)));
    }
    [Test]
    public void SOutFunctionCall__RightArgs__DoesNotThrow()
    {

        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile("SOut(\"abc\");", () => new FunctionCallDriverMock(false)));
    }
    [TestCase("SOut(\"abc\");")]
    [TestCase("1;SOut(\"abc\");")]
    [TestCase("\"rkj\"; SOut(\"abc\");")]
    public void SOutFunctionCall__RightArgs__OutputsCorrect(string Program)
    {
        (var res, var data) = HighToLowLevelCompilerDriver.Compile(Program, () => new FunctionCallDriverMock(false));
        var Out = new CustomTextWriter();
        var interp = new Interpreter(res, data, new StreamReader(Stream.Null), Out);
        interp.Interpret();
        Assert.That(Out.outStore.ToString(), Is.EqualTo("abc\r\n"));
    }
    [Test]
    public void ChainingFunctionCall__SOut_Input__ReturnsCorrect()
    {
        const string Program = "SOut(input());";
        (var res, var data) = HighToLowLevelCompilerDriver.Compile(Program, () => new FunctionCallDriverMock(false));
        var Out = new CustomTextWriter();
        var In = new CustomTextReader(new Stack<string>(["InputTestString1"]));
        var interp = new Interpreter(res, data, In, Out);
        interp.Interpret();
        Assert.That(Out.outStore.ToString(), Is.EqualTo("InputTestString1\r\n"));

    }
}