using SmallLang.Drivers;
using SmallLangTest.Generated;

namespace SmallLangTest;

[TestFixture]
public class FrontendCodegenTest
{
    [Test]
    public void Compile_All_Programs__Does_Not_Throw()
    {
        foreach (var Program in ExamplePrograms.AllPrograms)
            Assert.That(() => HighToLowLevelCompilerDriver.Compile(Program), Throws.Nothing, Program);
    }
}