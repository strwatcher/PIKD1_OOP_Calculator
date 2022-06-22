namespace Calculator
{
    public enum OpsState
    {
        Default,
        BOProcessing,
        BOProcessed,
    }

    public enum BOArgState
    {
        Default,
        Changed,
    }

    public enum CurNumState
    {
        Default,
        WaitForDot,
        Overflow,
    }

    public enum DotState
    {
        Exists,
        NotExists
    }
}