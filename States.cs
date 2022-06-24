namespace Calculator
{
    public enum BoState
    {
        Default,
        BoChoose,
        BoStarted,
        BOProcessed,
    }

    public enum UoState
    {
        Default,
        Logged
    }

    public enum NumState
    {
        Default,
        WaitForDot,
        WaitForLast,
        Overflow
    }

    public enum DotState
    {
        Exists,
        NotExists
    }

    public enum ExceptionState
    {
        Default,
        Overflow,
        InvalidInput
    }
}