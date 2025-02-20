using SmallLang.Parser;

namespace SmallLangTest;
[TestFixture]
public class ParserTest
{
    [Test]
    public void Ctor__Any_Input__Does_Not_Throw()
    {
        Assert.That(() => new Parser(""), Throws.Nothing);
    }
}