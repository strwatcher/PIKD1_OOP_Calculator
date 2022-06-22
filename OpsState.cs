namespace Calculator
{
    public enum OpsState
    {
        Default,
        BOChoice,
        BOStarted,
        BOProcessed,
    }

    public enum BOArgState
    {
        Default,
        Changed,
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