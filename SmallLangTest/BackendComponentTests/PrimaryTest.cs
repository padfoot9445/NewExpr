using SmallLang;
using SmallLang.Backend;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.LinearIR;

namespace SmallLangTest.BackendComponentTests;
[TestFixture]
public class PrimaryTest
{
    const string Char = "\"a\";";
    const string String = "\"abcdefghijklmnopqrs\";";
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
    uint FourCharsGrouping(string a) => FourCharsGrouping(a[0], a[1], a[2], a[3]);
    uint FourCharsGrouping(char a, char b, char c, char d) => (uint)(a << 24 | b << 16 | c << 8 | d);
    [Test]
    public void TestPrimary__String__OutputToStack__PushesCorrect_And_HasCorrectStaticData()
    {
        (var ins, var data) = HighToLowLevelCompilerDriver.Compile(String, () => new PrimaryTestVisitorMock(RetToRegister: false));
        Assert.That(ins[0].Op.Value, Is.EqualTo((uint)Opcode.PushI));
        //subtract 3 from string.length to account for quotes (2) and semicolon (1) # 1 + 2 = 3
        uint[] ExpectedData = {(1 << 16) | ((uint)Math.Ceiling((String.Length - 3) / 4.0) + 1),//String type << 16 | wordCount
        (uint)(String.Length - 3),
        FourCharsGrouping("abcd"),
        FourCharsGrouping("efgh"),
        FourCharsGrouping("ijkl"),
        FourCharsGrouping("mnop"),
        FourCharsGrouping('q', 'r', 's', (char)0),
        };
        Assert.That(data[0..ExpectedData.Length].SequenceEqual(ExpectedData));
        Assert.That(ins[0].Operands[0].Value, Is.EqualTo(0));
    }
}