using NUnit.Framework;
using MEXP.Parser;
using System;

namespace MEXPTests.ParserTest;
[TestFixture]
public class TypeProviderTests
{
    private static TypeProvider _typeProvider = new();

    [SetUp]
    public void Setup()
    {
        _typeProvider = new TypeProvider();
    }

    [Test]
    public void GetTypeFromTypeDenotingIdentifier_ShouldReturnUniqueTypeCodes()
    {
        uint intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        uint floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");
        Assert.That(intType, Is.Not.EqualTo(floatType));
    }

    [Test]
    public void GetTypeFromIdentifierLiteral_WithUnderscore_ShouldReturnZero()
    {
        Assert.That(_typeProvider.GetTypeFromIdentifierLiteral("_"), Is.EqualTo(0));
    }
    [Test]
    public void GetTypeFromIdentifierLiteral_WithUnderscore_ShouldReturnZero_EvenWhenStored()
    {
        _typeProvider.StoreIdentifierType("_", 1);
        Assert.That(_typeProvider.GetTypeFromIdentifierLiteral("_"), Is.EqualTo(0));
    }

    [Test]
    public void GetTypeFromIdentifierLiteral_WithNewIdentifier_ShouldReturnNull()
    {
        Assert.IsNull(_typeProvider.GetTypeFromIdentifierLiteral("unknown"));
    }

    [Test]
    public void StoreIdentifierType_ShouldStoreAndRetrieveType()
    {
        _typeProvider.StoreIdentifierType("x", 1);
        Assert.That(_typeProvider.GetTypeFromIdentifierLiteral("x"), Is.EqualTo(1));
    }

    [Test]
    public void BinOpResultantType_ShouldReturnHigherHierarchyType()
    {
        var intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        var floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");

        var resultantType = _typeProvider.BinOpResultantType(intType, floatType);

        Assert.That(resultantType, Is.EqualTo(floatType));
    }

    [Test]
    public void CanBeAssignedTo_ShouldReturnTrueForAssignableTypes()
    {
        var intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        var floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");

        Assert.That(_typeProvider.CanBeAssignedTo(floatType, intType), Is.True);
    }

    [Test]
    public void CanBeAssignedTo_ShouldReturnFalseForNonAssignableTypes()
    {
        var intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        var byteType = _typeProvider.GetTypeFromTypeDenotingIdentifier("byte");

        Assert.That(_typeProvider.CanBeAssignedTo(byteType, intType), Is.False);
    }

    [Test]
    public void GetTypeFromNumberLiteral_ShouldReturnCorrectType()
    {
        var floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");
        var byteType = _typeProvider.GetTypeFromTypeDenotingIdentifier("byte");

        Assert.Multiple(() =>
        {
            Assert.That(_typeProvider.GetTypeFromNumberLiteral("123.45"), Is.EqualTo(_typeProvider.NumberTypeCode));
            Assert.That(_typeProvider.GetTypeFromNumberLiteral("123"), Is.EqualTo(_typeProvider.IntTypeCode));
        });
    }

    [Test]
    public void CanBeDeclaredTo_ShouldAlwaysReturnTrue()
    {
        var intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        var floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");

        Assert.That(_typeProvider.CanBeDeclaredTo(intType, floatType), Is.True);
    }
    [Test]
    public void Test__GetTypeFromFloat__Returns_ArbPrec()
    {
        Assert.That(_typeProvider.GetTypeFromFloatLiteral(0), Is.EqualTo(_typeProvider.NumberTypeCode));
    }

    static IEnumerable<TestCaseData> Test__GetTypeFromIntLiteral__Returns__Correct_Cases()
    {
        yield return new TestCaseData(1, _typeProvider.ByteTypeCode);
        yield return new TestCaseData(2, _typeProvider.ByteTypeCode);
        yield return new TestCaseData(3, _typeProvider.IntTypeCode);
        yield return new TestCaseData(6, _typeProvider.IntTypeCode);
        yield return new TestCaseData(9, _typeProvider.IntTypeCode);
        yield return new TestCaseData(10, _typeProvider.LongTypeCode);
        yield return new TestCaseData(18, _typeProvider.LongTypeCode);
        yield return new TestCaseData(19, _typeProvider.LongIntTypeCode);
        yield return new TestCaseData(3524, _typeProvider.LongIntTypeCode);
    }
    [TestCaseSource(nameof(Test__GetTypeFromIntLiteral__Returns__Correct_Cases))]
    public void Test__GetTypeFromIntLiteral__Returns__Correct(int Length, uint ExpectedCode)
    {
        Assert.That(_typeProvider.GetTypeFromIntLiteral(Length), Is.EqualTo(ExpectedCode));
    }
}
