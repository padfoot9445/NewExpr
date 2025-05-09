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

    }
}