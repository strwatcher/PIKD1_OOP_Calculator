using Calculator;

namespace CalculatorTests;

public class Tests
{
    private Logger _logger = new();
    [SetUp]
    public void Setup()
    {
        _logger.Erase();
    }

    [Test]
    public void LoggerChangeOperationThanExecuteNextOperation()
    {
        _logger.LogBinOperation(BoState.Default, UoState.Default, "+", "10");
        _logger.LogBinOperation(BoState.BoChoose, UoState.Default, "-", "10");
        _logger.LogBinOperation(BoState.BoStarted, UoState.Default, "-", "15");
        Assert.That(_logger.Log, Is.EqualTo("10 - 15 - "));
    }

    [Test]
    public void LoggerSignOperationInEmptyLogSeveralTimes()
    {
        _logger.LogUnOperation(BoState.Default, "±", "10");
        _logger.LogUnOperation(BoState.Default, "±", "10");
        _logger.LogUnOperation(BoState.Default, "±", "10");
        _logger.LogUnOperation(BoState.Default, "±", "10");
        Assert.That(_logger.Log, Is.EqualTo(""));
    }

    [Test]
    public void Logger()
    {
        _logger.LogBinOperation(BoState.Default, UoState.Default, "-", "10");
        _logger.LogUnOperation(BoState.BoChoose, "√", "100");
        _logger.LogBinOperation(BoState.BoChoose, UoState.Logged, "*", "100");
        Assert.That(_logger.Log, Is.EqualTo("10 - sqrt(100) * "));
    }
}