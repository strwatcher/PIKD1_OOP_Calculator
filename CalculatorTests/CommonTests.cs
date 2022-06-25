using Calculator;

namespace CalculatorTests;

public class CommonTests
{
    private CalculatorMockup _calc = new();
    private State _changedState = new();

    [SetUp]
    public void Setup()
    {
        _calc = new();
        _changedState = new();
    }


    [Test]
    public void ChangeOperationThanExecuteNextOperation()
    {
        _calc.BinOperation("+", 10.0);
        _calc.BinOperation("-", 10.0);
        _calc.BinOperation("-", 15.0, true);
        _calc.BinOperation("*", 15.0);
        _calc.EqualsOperation(2.0);
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Log, Is.EqualTo(""));
            Assert.That(_calc.Res, Is.EqualTo(-10.0));
        });
    }

    [Test]
    public void SignOperationInEmptyLogSeveralTimes()
    {
        _calc.UnOperation(UnOperations.uo[0], 6);
        _calc.UnOperation(UnOperations.uo[0], 6);
        _calc.UnOperation(UnOperations.uo[0], 6);
        _calc.UnOperation(UnOperations.uo[0], 6);
        _calc.UnOperation(UnOperations.uo[0], 6);
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Log, Is.EqualTo(""));
            Assert.That(_calc.Res, Is.EqualTo(-6.0));
        });
    }

    [Test]
    public void BinAndUnOperationsCombine()
    {
        _calc.BinOperation("-", 100.0);
        _calc.UnOperation(UnOperations.uo[2], 16.0, true);
        _calc.BinOperation("*", 4.0, true);
        _calc.EqualsOperation(7.0);
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Log, Is.EqualTo(""));
            Assert.That(_calc.Res, Is.EqualTo(672.0));
        });
    }

    [Test]
    public void MultipleDivideOperationsWithDifferentArguments()
    {
        _calc.BinOperation("/", 1000000000);
        _calc.BinOperation("/", 10, true);
        _calc.BinOperation("/", 10, true);
        _calc.BinOperation("/", 10, true);
        _calc.BinOperation("/", 10, true);
        _calc.BinOperation("/", 10, true);
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Log, Is.EqualTo("1000000000 / 10 / 10 / 10 / 10 / 10 / "));
            Assert.That(_calc.Res, Is.EqualTo(10000));
        });
    }
}