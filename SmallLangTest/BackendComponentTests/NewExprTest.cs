using SmallLang;

namespace SmallLangTest.BackendComponentTests;

[TestFixture]
public class NewExprTest
{
    public static IEnumerable<(string, string)> GetInternalValueAndValuePairs()
    {
        foreach (var v in _GetInternalValueAndValuePairs())
        {
            yield return v;
        }
    }
    public static IEnumerable<(string, string)> Short_GetInternalValueAndValuePairs()
    {
        foreach (var v in _GetInternalValueAndValuePairs(true))
        {
            yield return v;
        }
    }
    static IEnumerable<(string, string)> _GetInternalValueAndValuePairs(bool shorten = false)
    {

        yield return ("char", "\"a\"");
        yield return ("char", "\'a\'");
        yield return ("string", "\"abc\"");
        yield return ("float", "1.23");
        yield return ("int", "123");
        if (!shorten)
        {

            yield return ("long", "1234");
            yield return ("double", "1.234");
            yield return ("number", "1.2345");
            yield return ("number", "1.23455");
            yield return ("longint", "12345");
            //yield return ("bigint", "123455");
            yield return ("byte", "255");
            //yield return ("short", "12");
        }
    }
    [Test]
    public void Test__NewExpr__Generics__Does_Not_Throw(
        [Values("list", "array", "set", "")] string OuterGeneric,
        [Values("list", "array", "set", "dict")]
        string GenericType,
        [ValueSource(nameof(GetInternalValueAndValuePairs))]
        (string NestedType, string Values) V,
        [Values(true, false)] bool Init
    )
    {
        if (GenericType == "dict" && Init == true) Assert.Pass();
        string Type = OuterGeneric == "" ? $"{GenericType}<[{V.NestedType}]>" : $"{OuterGeneric}<[{GenericType}<[{V.NestedType}]>]>";
        string Program = OuterGeneric == "" ? $"new {Type}({(Init ? V.Values : "")});" : $"new {Type}({(Init ? $"new {GenericType}<[{V.NestedType}]>({V.Values})" : "")});";
        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile(Program));
    }
    // [Ignore("Takes a fucking long time")]
    // [Test]
    public void NestedDict__Does_Not_Throw(
        [Values("list", "dict")] string InnerType1,
        [Values("list", "dict")] string InnerType2,
        [ValueSource(nameof(Short_GetInternalValueAndValuePairs))] (string Type, string Value) VT1,
        [ValueSource(nameof(Short_GetInternalValueAndValuePairs))] (string Type, string Value) VT2,
        [ValueSource(nameof(Short_GetInternalValueAndValuePairs))] (string Type, string Value) VT3,
        [ValueSource(nameof(Short_GetInternalValueAndValuePairs))] (string Type, string Value) VT4,
        [Values(true, false)] bool Init
    )
    {
        string BIT1 = BuildType(InnerType1, VT1, VT2, false);
        string BIT2 = BuildType(InnerType2, VT3, VT4, false);
        string InitExpression = $"{BuildType(InnerType1, VT1, VT2, true)}, {BuildType(InnerType2, VT3, VT4, true)}";
        string Final = $"new dict<[{BIT1}, {BIT2}]>({(Init ? InitExpression : "")});";
        Assert.DoesNotThrow(() => HighToLowLevelCompilerDriver.Compile(Final));
    }
    static string BuildType(string Outer, (string Type, string Value) VT1, (string Type, string Value) VT2, bool Init = false)
    {
        if (Init == false) return BuildType(Outer, VT1.Type, VT2.Type);
        else if (Outer == "dict") return $"new {BuildType(Outer, VT1.Type, VT2.Type)}({VT1.Value}, {VT2.Value})";
        else return $"new {BuildType(Outer, VT1.Type, VT2.Type)}({VT1.Value})";
    }
    static string BuildType(string Outer, string VV1, string VV2)
    {
        if (Outer == "dict")
        {
            return $"{Outer}<[{VV1}, {VV2}]>";
        }
        else
        {
            return $"{Outer}<[{VV1}]>";
        }
    }
    [Ignore("Human Test")]
    [Test]
    public void OutputNewIntList()
    {
        (var x, var y) = HighToLowLevelCompilerDriver.Compile("new list<[int]>(1,2,3,4);");
        Console.WriteLine(x.Select(j => j.ToString()).Aggregate((i, j) => $"{i}\n{j}"));
    }
    [Test]
    public void NewList__Int_Four_Args__Outputs_Correct_ToString()
    {
        (var x, var y) = HighToLowLevelCompilerDriver.Compile("new list<[int]>(1,2,3,4);");
        Assert.That(x.Select(j => j.ToString()).Aggregate((i, j) => $"{i}\n{j}"), Is.EqualTo("PushI 1\nPushI 2\nPushI 3\nPushI 4\nNewPR 13 4 1"));
    }
}