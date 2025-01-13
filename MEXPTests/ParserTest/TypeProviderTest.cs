using NUnit.Framework;
using MEXP.Parser;
using System;

namespace MEXPTests.ParserTest;
[TestFixture]
public class TypeProviderTests
{
    private TypeProvider _typeProvider = new();

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
            Assert.That(_typeProvider.GetTypeFromNumberLiteral("123.45"), Is.EqualTo(floatType));
            Assert.That(_typeProvider.GetTypeFromNumberLiteral("123"), Is.EqualTo(byteType));
        });
    }

    [Test]
    public void CanBeDeclaredTo_ShouldAlwaysReturnTrue()
    {
        var intType = _typeProvider.GetTypeFromTypeDenotingIdentifier("int");
        var floatType = _typeProvider.GetTypeFromTypeDenotingIdentifier("float");

        Assert.That(_typeProvider.CanBeDeclaredTo(intType, floatType), Is.True);
    }
}
