namespace Calculator
{
    public class State
    {
        public BoState BoState = BoState.Default;
        public UoState UoState = UoState.Default;
        public NumState NumState = NumState.Default;
        public DotState DotState = DotState.NotExists;
        public ExceptionState EState = ExceptionState.Default;

        public bool CanDoOperations()
        {
            return EState == ExceptionState.Default;
        }

        public void ClearNumStates()
        {
            DotState = DotState.NotExists;
            NumState = NumState.Default;
        }

        public void ClearCommonStates()
        {
            BoState = BoState.Default;
            UoState = UoState.Default;
            EState = ExceptionState.Default;
        }

        public void UpdateStatesAfterNumLenChange(string num)
        {
            if (num.Length == 16)
            {
                NumState = DotState == DotState.Exists
                    ? NumState.WaitForLast
                    : NumState.WaitForDot;
            }
            else if (num.Length < 16) NumState = NumState.Default;
            else NumState = NumState.Overflow;
        }
    }
}