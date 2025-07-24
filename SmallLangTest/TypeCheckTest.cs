using SmallLang;
using SmallLang.Constants;
using SmallLang.Metadata;

namespace SmallLangTest;

[TestFixture]
public class TypeCheckTest
{
    public static IEnumerable<TestCaseData> InvalidPrograms()
    {
        yield return new TestCaseData("int x = \"abc\";", TypeData.Data.IntTypeCode, TypeData.Data.StringTypeCode, 0, null);
        yield return new TestCaseData("string x = 1;", TypeData.Data.StringTypeCode, TypeData.Data.IntTypeCode, 0, null);
        yield return new TestCaseData("int x = 1;\n string y = x;", TypeData.Data.StringTypeCode, TypeData.Data.IntTypeCode, 1, null);
        yield return new TestCaseData("SOut(1);", TypeData.Data.StringTypeCode, TypeData.Data.IntTypeCode, 0, null);
        yield return new TestCaseData("list<[int]> x = new list<[int]>(\"abc\", \"def\");", TypeData.Data.IntTypeCode, TypeData.Data.StringTypeCode, 0, null);
    }
    [TestCaseSource(nameof(InvalidPrograms))]
    public void Test__InvalidPrograms__Throws_Correct(string Program, SmallLangType Expected, SmallLangType Actual, int Position, string? Message)
    {
        Assert.That(() => HighToLowLevelCompilerDriver.Compile(Program), Throws.TypeOf<TypeErrorException>());
        try
        {
            HighToLowLevelCompilerDriver.Compile(Program);
            Assert.Fail("Expected exception TypeErrorException thrown but none was thrown");
        }
        catch (TypeErrorException e)
        {
            Assert.Multiple(() =>
            {
                Assert.That(e.Expected, Is.EqualTo(Expected));
                Assert.That(e.Actual, Is.EqualTo(Actual));
                Assert.That(e.Position, Is.EqualTo(Position));
                Console.WriteLine(e.Message);
            });
        }
        catch (AssertionException) { throw; }
        catch (Exception e)
        {
            Assert.Fail($"Expected exception TypeErrorException but got {e.GetType()}");
            throw;
        }
    }
}