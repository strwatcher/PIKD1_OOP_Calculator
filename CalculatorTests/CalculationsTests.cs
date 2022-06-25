using Calculator;

namespace CalculatorTests;

public class CalculationsTests
{
    private CalculatorMockup _calc = new();
    [SetUp]
    public void Setup()
    {
        _calc = new();
    }

    [Test]
    public void AddWithPositive()
    {
        _calc.BinOperation("+", 999999999999);
        _calc.EqualsOperation(1);
        Assert.That(_calc.Res, Is.EqualTo(1000000000000));
    }

    [Test]
    public void AddWithNegative()
    {
        _calc.BinOperation("+", 100);
        _calc.EqualsOperation(-50);
        Assert.That(_calc.Res, Is.EqualTo(50));
    }

    [Test]
    public void AddWithResOfUnOp()
    {
        _calc.BinOperation("+", 100);
        _calc.UnOperation(UnOperations.uo[1], 0.1);
        _calc.EqualsOperation(_calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(110));
    }

    [Test]
    public void MinusWithPositive()
    {
        _calc.BinOperation("-", 333);
        _calc.EqualsOperation(111);
        Assert.That(_calc.Res, Is.EqualTo(222));
    }

    [Test]
    public void MinusWithFloat()
    {
        _calc.BinOperation("-", 1);
        _calc.EqualsOperation(0.000000001);
        Assert.That(_calc.Res, Is.EqualTo(0.999999999));
    }

    [Test]
    public void MinusWithNegative()
    {
        _calc.BinOperation("-", 100);
        _calc.EqualsOperation(-100);
        Assert.That(_calc.Res, Is.EqualTo(200));
    }

    [Test]
    public void MultBigEnoughNumbers()
    {
        _calc.BinOperation("*", 1e90);
        _calc.EqualsOperation(1e110);
        Assert.That(_calc.Res, Is.EqualTo(1e200));
    }

    [Test] 
    public void MultSmallEnoughWithBigEnough()
    {
        _calc.BinOperation("*", 1e-100);
        _calc.EqualsOperation(1e100);
        Assert.That(_calc.Res, Is.EqualTo(1));
    }

    [Test]
    public void MultSmallWithSmall()
    {
        _calc.BinOperation("*", 1e-100);
        _calc.EqualsOperation(1e-123);
        Assert.True(_calc.IsResEqualTo(1e-223));
    }

    [Test]
    public void DivideWithPositive()
    {
        _calc.BinOperation("/", 121);
        _calc.EqualsOperation(23);
        Assert.True(_calc.IsResEqualTo(5.260869565217391));
    }

    [Test]
    public void DivideWithNegative()
    {
        _calc.BinOperation("/", 74);
        _calc.EqualsOperation(-65);
        Assert.True(_calc.IsResEqualTo(-1.138461538461538));
    }

    [Test]
    public void DivideWithSmall()
    {
        _calc.BinOperation("/", 99999.65464);
        _calc.EqualsOperation(0.1510155);
        Assert.True(_calc.IsResEqualTo(662181.3962142959));
    }

    [Test]
    public void MultOneNumber10TimesUsingEqualsOperation()
    {
        _calc.BinOperation("*", 5);
        _calc.EqualsOperation(5);
        for (int i = 0; i < 9; ++i)
        {
            _calc.EqualsOperation(_calc.Res);
        }

        Assert.That(_calc.Res, Is.EqualTo(48828125));
    }

    [Test]
    public void Invert2Times()
    {
        _calc.UnOperation(UnOperations.uo[1], 4);
        _calc.UnOperation(UnOperations.uo[1], _calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(4));
    }

    [Test]
    public void Plus25Percents()
    {
        _calc.BinOperation("+", 100);
        var state = new State {BoState = BoState.BoStarted};
        _calc.PercentOperation(25);
        _calc.EqualsOperation(_calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(125));
    }

    [Test]
    public void Minus30Percents()
    {
        _calc.BinOperation("-", 12424);
        var state = new State {BoState = BoState.BoStarted};
        _calc.PercentOperation(30, state);
        _calc.EqualsOperation(_calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(8696.8));
    }

    [Test]
    public void MultWith5Percents()
    {
        _calc.BinOperation("*", 500);
        var state = new State {BoState = BoState.BoStarted};
        _calc.PercentOperation(5, state);
        _calc.EqualsOperation(_calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(12500));
    }

    [Test]
    public void DivideWithPercent()
    {
        _calc.BinOperation("/" ,100);
        var state = new State {BoState = BoState.BoStarted};
        _calc.PercentOperation(10, state);
        _calc.EqualsOperation(_calc.Res);
        Assert.That(_calc.Res, Is.EqualTo(10));
    }
}