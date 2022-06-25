using System.Collections.Generic;

namespace Calculator
{
    public class Logger
    {
        private readonly List<string> _binOps = new List<string> {"+", "-", "*", "/"};
        private readonly Dictionary<string, string> _unOpsDict = new Dictionary<string, string>
        {
            {"√", "sqrt"},
            {"±", "negate"},
            {"1/x", "reciproc"}
        };
        private string _log = "";
        private string[] SplitLog => _log.Trim().Split(' ');
        public string Log => _log;

        public void LogBinOperation(
            State state,
            string operation, double argument
            )
        {
            if (state.BoState != BoState.BoChoose) 
            {
                string argumentView = state.UoState == UoState.Logged ? "" : $"{argument}";
                _log += $"{argumentView} {operation} ";
            }
            else if (state.BoState == BoState.BoChoose)
            {
                string[] splitLog = SplitLog;
                splitLog[splitLog.Length - 1] = operation + " ";
                _log = string.Join(" ", splitLog);
            }
        }

        public void LogUnOperation(
            State state, string operation, double argument
            )
        {
            string[] splitLog = SplitLog;
            string lastLogArg = splitLog[splitLog.Length - 1];

            if (state.BoState != BoState.BoChoose &&
                operation == "±" &&
                (_binOps.Contains(lastLogArg) || lastLogArg == ""))
            {
                state.UoState = UoState.Default;
            }

            else if (_binOps.Contains(lastLogArg))
            {
                _log += $"{_unOpsDict[operation]}({argument})";
                state.UoState = UoState.Logged;
            }
            else
            {
                string arg = lastLogArg == "" ? $"{argument}" : lastLogArg;
                splitLog[splitLog.Length - 1] = $"{_unOpsDict[operation]}({arg})";
                _log = string.Join(" ", splitLog);
                state.UoState = UoState.Logged;
            }
        }

        public void Erase()
        {
            _log = "";
        }

        public void DeleteLastLog()
        {
            string[] splitLog = SplitLog;
            splitLog[splitLog.Length - 1] = "";
            _log = string.Join(" ", splitLog);
        }

        public void LogPercentOperation(State state, double argument)
        {
            if (state.UoState == UoState.Default)
            {
                _log += $"{argument}";
            }
            else
            {
                string[] splitLog = SplitLog;
                splitLog[splitLog.Length - 1] = $"{argument}";
                _log = string.Join(" ", splitLog);
            }
            
            state.UoState = UoState.Default;
        }
    } 
}