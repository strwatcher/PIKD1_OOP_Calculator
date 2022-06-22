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

        public void UpdateStatesAfterBackspace(bool isDot, Action action)
        {
            if (_boState == BoState.BoChoose) return;
            if (isDot) _dotState = DotState.NotExists;
            action();
        }
        public void UpdateStatesAfterDo(string operation, 
            Action boChoose, Action canEnter)
        {
            if (_boState == BoState.BoChoose)
            {
                boChoose();
                _boState = BoState.BoStarted;
            }

            if (_numState == NumState.Default ||
                _numState == NumState.WaitForLast ||
                _numState == NumState.WaitForDot && operation == ".")
            {
                 if (operation == "." && _dotState == DotState.Exists) return;
                 if (operation == ".") _dotState = DotState.Exists;
                 canEnter();
            }
        }

        public void UpdateStatesAfterEo(Action boDefault)
        {
            if (_boState != BoState.Default) boDefault();
            _boState = BoState.Default;
        }
        public void UpdateStatesAfterBo(string operation,
            Action<BoState, UoState> common, Action d,
            Action boChoose, Action boStarted)
        {
            common(_boState, _uoState);
            if (_boState == BoState.Default)
            {
                d();
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
    }
}