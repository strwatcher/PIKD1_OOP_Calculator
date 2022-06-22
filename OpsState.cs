namespace Calculator
{
    public enum OpsState
    {
        Default,
        BOChoose,
        BOStarted,
        BOProcessed,
    }

    public enum UOState
    {
        Default,
        Logged,
    }

    public enum NumState
    {
        Default,
        WaitForDot,
        WaitForLast,
        Overflow,
    }

    public enum DotState
    {
        Exists,
        NotExists
    }
}