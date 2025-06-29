using SmallLang;

namespace SmallLangTest.BackendComponentTests;

[TestFixture]
class DeclarationTest
{
    public static IEnumerable<(string, string)> GetTypeAndValuePairs()
    {
        foreach (var i in NewExprTest.GetInternalValueAndValuePairs())
        {
            yield return i;
        }
        yield return ("list<[int]>", "new list<[int]>(1,2,3,4,5)");
        yield return ("dict<[int, string]>", "new dict<[int, string]>(1, \"abc\")");
        yield return ("set<[float]>", "new set<[float]>(1.1, 2, 4)");
        yield return ("array<[int]>", "new array<[int]>(1,2,3)");
        yield return ("dict<[set<[string]>, list<[int]>]>", "new dict<[set<[string]>, list<[int]>]>(new set<[string]>(\"abc\", \"def\"), new list<[int]>(1,2))");
    }
    [Test]
    public void Different_Variable_Names__Does_Not_Throw([Values(
        "abc", "_123", "AB12"
    )]string name)
    {
        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile($"int {name};"));
    }
    [Test]
    public void SimpleTypeDeclaration__Does_Not_Throw(
        [ValueSource(nameof(GetTypeAndValuePairs))] (string type, string value) tv,
        [Values(true, false)] bool assign)
    {
        string Compile = assign ? $"{tv.type} abc" : $"{tv.type} abc = {tv.value}";
        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile(Compile));
    }
}