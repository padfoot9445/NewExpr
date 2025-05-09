using SmallLang;
using SmallLang.LinearIR;

namespace SmallLangTest.BackendComponentTests;
[TestFixture]
public class PrimaryTest
{
    const string Char = "\"a\";";
    const string String = "\"abc\";";
    const string Int = "1;";
    const string Float = "0.1;";
    const string True = "true;";
    const string False = "false;";
    [Test]
    public void TestPrimary__Char__OutputToStack__PushesCorrect()
    {
        var res = HighToLowLevelCompilerDriver.Compile(Char, () => new PrimaryTestVisitorMock(RetToRegister: false)).Item1[0];
        Assert.That(res.Op.Value, Is.EqualTo((uint)Opcode.PushI));
        Assert.That((char)res.Operands[0].Value, Is.EqualTo('a'));
    }
    [Test]
    public void TestPrimary__Char__OutputToReg__LoadsCorrect()
    {
        var res = HighToLowLevelCompilerDriver.Compile(Char, () => new PrimaryTestVisitorMock(RetToRegister: true)).Item1[0];
        Assert.That(res.Op.Value, Is.EqualTo((uint)Opcode.LoadI));
        Assert.That((char)res.Operands[0].Value, Is.EqualTo('a'));
        Assert.That(res.Operands[1].Value, Is.EqualTo(1));
    }
}