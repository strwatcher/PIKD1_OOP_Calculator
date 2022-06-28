namespace CalculatorTests;

public class LoggerTests
{
    private CalculatorMockup _calc = new();
    
    [SetUp]
    public void Setup()
    {
        _calc = new CalculatorMockup();
    }

    [Test]
    public void UnOperationSeveralTimes()
    {
        _calc.UnOperation(UnOperations.uo[2], 16);
        _calc.UnOperation(UnOperations.uo[2], _calc.Res);
        _calc.UnOperation(UnOperations.uo[2], _calc.Res);
        Assert.That(_calc.Log, Is.EqualTo("sqrt(sqrt(sqrt(16)))"));
    }

    [Test]
    public void UnOperationThenBinOperation()
    {
        _calc.UnOperation(UnOperations.uo[1], 0.1);
        _calc.BinOperation("-", _calc.Res);
        _calc.BinOperation("+", 123, true);
        Assert.That(_calc.Log, Is.EqualTo("reciproc(0.1) - 123 + "));
    }

    [Test]
    public void BinOperationThenUnOperation()
    {
        _calc.BinOperation("+", 101);
        _calc.UnOperation(UnOperations.uo[0], _calc.Res);
        Assert.That(_calc.Log, Is.EqualTo("101 + negate(101)"));
    }

    [Test]
    public void BinOperationWithUnOperations()
    {
        _calc.UnOperation(UnOperations.uo[2], 16);
        _calc.BinOperation("*", _calc.Res);
        _calc.UnOperation(UnOperations.uo[1], 0.1);
        Assert.That(_calc.Log, Is.EqualTo("sqrt(16) * reciproc(0.1)"));
    }
}