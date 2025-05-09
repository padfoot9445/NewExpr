using SmallLang;

namespace SmallLangTest;
[TestFixture]
public class HTLDriverTest
{
    public const string Program1 = "input();";
    [TestCase(Program1)]
    public void ValidPrograms__DoesNotThrowAndReturnsNonEmptyInstructions(string program)
    {
        Assert.That(() => HighToLowLevelCompilerDriver.Compile(program), Throws.Nothing);
        Assert.That(HighToLowLevelCompilerDriver.Compile(program).Item1, Has.Length.GreaterThanOrEqualTo(1));
    }
}