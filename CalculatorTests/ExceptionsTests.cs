namespace CalculatorTests;

public class ExceptionsTests
{
    private CalculatorMockup _calc = new();
    
    [SetUp]
    public void Setup()
    {
        _calc = new();
    }

    [Test]
    public void SqrtFromNegative()
    {
        _calc.UnOperation(UnOperations.uo[0], 4);
        _calc.UnOperation(UnOperations.uo[2], _calc.Res, true);
        Assert.That(_calc.Res, Is.EqualTo(Double.NaN));
    }

    [Test]
    public void OverflowTest()
    {
        _calc.BinOperation("*", 1e300);
        _calc.BinOperation("*", 1e300, true);
        Assert.That(_calc.Res, Is.EqualTo(Double.PositiveInfinity));
    }

    [Test]
    public void DivideByZero()
    {
        _calc.BinOperation("/", 1);
        _calc.BinOperation("/", 0.0, true);
        Assert.That(_calc.Res, Is.EqualTo(Double.PositiveInfinity));
    }
}