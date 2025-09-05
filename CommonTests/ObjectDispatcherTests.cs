using Common.Dispatchers;

namespace Common.Tests.Dispatchers;

[TestFixture]
public class ObjectDispatchersTests
{
    private class TestObject
    {
        public int Number { get; set; }
    }

    [Test]
    public void Dispatch_ShouldReturnMatchedResult()
    {
        var obj = new TestObject { Number = 10 };

        var result = obj.Dispatch(
            x => x.Number, (Predicate: n => n > 5,
                Result: o => "Greater than 5"), (Predicate: n => n < 5,
                Result: o => "Less than 5"));

        Assert.That(result, Is.EqualTo("Greater than 5"));
    }

    [Test]
    public void Dispatch_ShouldThrow_WhenNoMatchAndNoDefault()
    {
        var obj = new TestObject { Number = 2 };

        Assert.Throws<MatchNotFoundException>(() =>
        {
            obj.Dispatch(
                x => x.Number, (Predicate: n => n > 5,
                    Result: o => "Greater than 5"));
        });
    }

    [Test]
    public void Dispatch_ShouldUseDefault_WhenNoMatch()
    {
        var obj = new TestObject { Number = 3 };

        var result = obj.Dispatch(
            x => x.Number,
            _ => "Default Value", (Predicate: n => n > 5,
                Result: o => "Greater than 5"));

        Assert.That(result, Is.EqualTo("Default Value"));
    }

    [Test]
    public void Dispatch_WithActionCases_ShouldExecuteCorrectAction()
    {
        var obj = new TestObject { Number = 7 };
        var flag = false;

        obj.Dispatch(
            x => x.Number,
            _ => { flag = true; }, (Predicate: n => n > 5,
                Result: _ => flag = true));

        Assert.IsTrue(flag);
    }

    [Test]
    public void Switch_ShouldReturnMatchedResult()
    {
        var obj = new TestObject { Number = 42 };

        var result = obj.Switch(
            x => x.Number,
            (a, b) => a == b, (Value: 1, Result: "One"), (Value: 42, Result: "Forty-Two"));

        Assert.That(result, Is.EqualTo("Forty-Two"));
    }

    [Test]
    public void Switch_ShouldThrow_WhenNoMatch()
    {
        var obj = new TestObject { Number = 0 };

        Assert.Throws<MatchNotFoundException>(() =>
        {
            obj.Switch(
                x => x.Number,
                (a, b) => a == b, (Value: 1, Result: "One"), (Value: 42, Result: "Forty-Two"));
        });
    }

    [Test]
    public void Switch_WithValToFunc_ShouldReturnCorrectValue()
    {
        var obj = new TestObject { Number = 99 };

        var result = obj.Switch(
            x => x.Number,
            (a, b) => a == b, (Value: 99, Result: "Matched"));

        Assert.That(result, Is.EqualTo("Matched"));
    }
}