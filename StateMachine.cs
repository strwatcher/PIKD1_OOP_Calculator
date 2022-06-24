using System;

namespace Calculator
{
    public class StateMachine
    {
        private BoState _boState = BoState.Default;
        private UoState _uoState = UoState.Default;
        private NumState _numState = NumState.Default;
        private DotState _dotState = DotState.NotExists;

        public void ClearNumStates()
        {
            _dotState = DotState.NotExists;
            _numState = NumState.Default;
        }

        public void ClearCommonStates()
        {
            _boState = BoState.Default;
            _uoState = UoState.Default;
        }

        public void UpdateStatesAfterBackspace(bool isDot, Action backspace)
        {
            if (_boState == BoState.BoChoose) return;
            if (isDot) _dotState = DotState.NotExists;
            backspace();
        }

        public void UpdateStatesAfterDo(string operation, Action changeLastLog,
            Action clearNum, Action enterDigit)
        {
            if (_uoState == UoState.Logged)
            {
                clearNum();
                changeLastLog();
                _uoState = UoState.Default;
            }

            if (_boState == BoState.BoChoose)
            {
                clearNum();
                _boState = BoState.BoStarted;
            }

            if (_boState == BoState.BOProcessed)
            {
                clearNum();
                _boState = BoState.Default;
            }

            if (_numState == NumState.Default ||
                _numState == NumState.WaitForLast ||
                _numState == NumState.WaitForDot && operation == ".")
            {
                if (operation == "." && _dotState == DotState.Exists) return;
                if (operation == ".") _dotState = DotState.Exists;
                enterDigit();
            }
        }

        public void UpdateStatesAfterEo(Action boProcessed, Action repeatOperation)
        {
            if (_boState == BoState.BoChoose || _boState == BoState.BoStarted)
            {
                boProcessed();
                _boState = BoState.BOProcessed;
            }
            else if (_boState == BoState.BOProcessed || _boState == BoState.Default) repeatOperation();
        }

        public void UpdateStatesAfterBo(Action<BoState, UoState> log, Action boDefault,
            Action boChoose, Action boStarted)
        {
            log(_boState, _uoState);
            if (_boState == BoState.Default || _boState == BoState.BOProcessed)
            {
                boDefault();
                _boState = BoState.BoChoose;
                _uoState = UoState.Default;
            }
            else if (_boState == BoState.BoChoose)
            {
                boChoose();
            }
            else if (_boState == BoState.BoStarted)
            {
                boStarted();
                _boState = BoState.BoChoose;
                _uoState = UoState.Default;
            }
        }

        public void UpdateStatesAfterUo(Action<BoState> action, bool stateCondition)
        {
            action(_boState);
            _uoState = stateCondition ? UoState.Default : UoState.Logged;
        }

        public void UpdateStatesAfterNumLenChange(string num)
        {
            if (num.Length == 16)
            {
                _numState = _dotState == DotState.Exists
                    ? NumState.WaitForLast
                    : NumState.WaitForDot;
            }
            else if (num.Length < 16) _numState = NumState.Default;
            else _numState = NumState.Overflow;
        }

        public void UpdateStatesAfterPo(Action<UoState> action)
        {
            action(_uoState);
            _uoState = UoState.Default;
        }
    }
}