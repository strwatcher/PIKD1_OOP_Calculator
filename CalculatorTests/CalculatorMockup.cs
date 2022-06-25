using Calculator;

namespace CalculatorTests;

public class CalculatorMockup
{
    private readonly Logger _logger = new(); 
    private readonly MathOperationsProcessor _processor = new();
    private State _state = new();
    private double _res;
    private double _prevArgument = Double.NaN;

    public double Res => _res;
    public string Log => _logger.Log;
    public State State => _state;

    public void BinOperation(string type, double argument, bool argumentChanged = false)
    {
        if (argumentChanged) _state.BoState = BoState.BoStarted;
        _prevArgument = argument;
        _logger.LogBinOperation(_state, type, argument);
        _res = _processor.ProcessBinOperation(_state, type, argument);
    }

    public void UnOperation(string type, double argument, bool argumentChanged = false)
    {
        if (argumentChanged) _state.BoState = BoState.BoStarted;
        _logger.LogUnOperation(_state, type, argument);
        _res = _processor.ProcessUnOperation(_state, type, argument);
    }

    public void PercentOperation(double argument, State? state = null)
    {
        if (state != null) _state = state;
        _res = _processor.ProcessPercentOperation(_state, argument);
        _logger.LogPercentOperation(_state, argument);
    }

    public void EqualsOperation(double argument, State? state = null)
    {
        if (state != null) _state = state;
        _res = _processor.ProcessEqualsOperation(_state, argument);
        _logger.Erase();
    }

}