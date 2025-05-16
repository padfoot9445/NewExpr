using SmallLang;

namespace SmallLangTest.BackendComponentTests;
[TestFixture]
public class NewExprTest
{
    static IEnumerable<(string, string)> GetInternalValueAndValuePairs()
    {
        yield return ("float", "1.23");
        yield return ("int", "123");
        yield return ("long", "1234");
        yield return ("double", "1.234");
        yield return ("number", "1.2345");
        yield return ("number", "1.23455");
        yield return ("longint", "12345");
        yield return ("bigint", "123455");
        yield return ("byte", "255");
        yield return ("short", "12");
        yield return ("char", "\"a\"");
        yield return ("char", "\'a\'");
        yield return ("string", "\"abc\"");
    }
    [Test]
    public void Test__NewExpr__SingleGeneric__Does_Not_Throw(
        [Values("list", "array", "set", "dict")]
        string GenericType,
        [ValueSource(nameof(GetInternalValueAndValuePairs))]
        (string NestedType, string Values) V,
        [Values(true, false)] bool Init
    )
    {
        if (GenericType == "dict" && Init == true) Assert.Pass();

        string Program = $"new {GenericType}<[{V.NestedType}]>({(Init ? V.Values : "")});";
        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile(Program));
    }
}