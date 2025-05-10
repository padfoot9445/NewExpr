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
    uint FourCharsGrouping(char a, char b, char c, char d) => (uint)(a | b << 8 | c << 16 | d << 24); // strings are stored in the form (char4, char3, char2, char1), (char8, char7, char6, char5), (padding, padding, char10, char9)
    [TestCase(false)]
    [TestCase(true)]
    public void TestPrimary__String__OutputToStackAndReg__PushesCorrect_And_HasCorrectStaticData(bool OutToReg)
    {
        (var ins, var data) = HighToLowLevelCompilerDriver.Compile(String, () => new PrimaryTestVisitorMock(RetToRegister: OutToReg));
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
        if (OutToReg)
        {
            Assert.That(ins[0].Op.Value, Is.EqualTo((uint)Opcode.LoadI));
            Assert.That(ins[0].Operands.Select(x => x.Value), Is.EquivalentTo(new List<uint>() { 0, 1 }));
        }
        else
        {

            Assert.That(ins[0].Op.Value, Is.EqualTo((uint)Opcode.PushI));
            Assert.That(ins[0].Operands[0].Value, Is.EqualTo(0));
        }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestPrimary__Int__OutputToStackAndReg__PushesCorrect(bool OTR)
    {
        (var ins, var _) = HighToLowLevelCompilerDriver.Compile(Int, () => new PrimaryTestVisitorMock(OTR));
        Assert.That(ins[0].Op.Value, Is.EqualTo((uint)(OTR ? Opcode.LoadI : Opcode.PushI)));
        Assert.That(ins[0].Operands.First().Value, Is.EqualTo(1));
        if (OTR)
        {
            Assert.That(ins[0].Operands.Skip(1).First().Value, Is.EqualTo(1));
        }
    }
    [TestCase(true)]
    [TestCase(false)]
    public void TestPrimary__Float__OutputToStackAndReg__PushesCorrect(bool OTR)
    {
        (var ins, var _) = HighToLowLevelCompilerDriver.Compile(Float, () => new PrimaryTestVisitorMock(OTR));
        Assert.That(ins[0].Op.Value, Is.EqualTo((uint)(OTR ? Opcode.LoadI : Opcode.PushI)));
        Assert.That(BitConverter.GetBytes(ins[0].Operands.First().Value), Is.EquivalentTo(BitConverter.GetBytes((float)0.1)));
        if (OTR)
        {
            Assert.That(ins[0].Operands.Skip(1).First().Value, Is.EqualTo(1));
        }
    }
}