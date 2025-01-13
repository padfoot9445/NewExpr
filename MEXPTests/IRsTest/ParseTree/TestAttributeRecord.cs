using IRs.ParseTree;

namespace MEXPTests.IRsTest.ParseTree;
[TestFixture]
public class TestAttributeRecord
{
    public static IEnumerable<TestCaseData> OtherValues()
    {
        yield return new TestCaseData((uint?)12);
        yield return new TestCaseData(null);
    }
    [Test]
    public void TestCtor__SetsProperties()
    {
        Assert.That(new AttributeRecord((uint)12).TypeCode, Is.EqualTo(12));
    }
    [TestCaseSource(nameof(OtherValues))]
    public void TestMerge__NullSelf__NullAndNonNullOther__DoesNotThrowAndCorrectlyMerges(uint? otherValue)
    {
        AttributeRecord self = new();
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.Merge(other), Throws.Nothing);
            Assert.That(self.TypeCode, Is.EqualTo(otherValue));
        });
    }
    [TestCaseSource(nameof(OtherValues))]
    public void TestMerge__NonNullSelf__Throws(uint? otherValue)
    {
        AttributeRecord self = new(42);
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.That(() => self.Merge(other), Throws.InvalidOperationException);
    }
    [TestCaseSource(nameof(OtherValues))]
    public void TestForceMerge__NullSelfAndPrioritizeSelf__CorrectlyMergesAndDoesNotThrow(uint? otherValue)
    {
        AttributeRecord self = new();
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.ForcedMerge(other, PrioritizeOther: false), Throws.Nothing);
            Assert.That(self.TypeCode, Is.EqualTo(otherValue));
        });
    }
    [TestCaseSource(nameof(OtherValues))]
    public void TestForceMerge__NullSelfAndPrioritizeOther__CorrectlyMergesAndDoesNotThrow(uint? otherValue)
    {
        AttributeRecord self = new();
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.ForcedMerge(other, PrioritizeOther: true), Throws.Nothing);
            Assert.That(self.TypeCode, Is.EqualTo(otherValue));
        });
    }

    [TestCaseSource(nameof(OtherValues))]
    public void TestForceMerge__NonNullSelfAndPrioritizeSelf__DoesNotMergeAndDoesNotThrow(uint? otherValue)
    {
        AttributeRecord self = new(42);
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.ForcedMerge(other, PrioritizeOther: false), Throws.Nothing);
            Assert.That(self.TypeCode, Is.Not.EqualTo(otherValue));
        });
    }

    [TestCase((uint)12)]
    public void TestForceMerge__NonNullSelfAndPrioritizeOther_NonNullOther__Merges(uint? otherValue)
    {
        AttributeRecord self = new(42);
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.ForcedMerge(other, PrioritizeOther: true), Throws.Nothing);
            Assert.That(self.TypeCode, Is.EqualTo(otherValue));
        });
    }

    [TestCase(null)]
    public void TestForceMerge__NonNullSelfAndPrioritizeOther_NullOther__DoesNotMerge(uint? otherValue)
    {
        AttributeRecord self = new(42);
        AttributeRecord other = new AttributeRecord(otherValue);
        Assert.Multiple(() =>
        {
            Assert.That(() => self.ForcedMerge(other, PrioritizeOther: true), Throws.Nothing);
            Assert.That(self.TypeCode, Is.Not.EqualTo(otherValue));
        });
    }
}