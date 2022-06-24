﻿using System.Collections.Generic;

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

        public bool IsEmptyLogged => SplitLog[SplitLog.Length - 1] == "";
        public void LogBinOperation(
            BoState boState, UoState uoState,
            string operation, string argument
            )
        {
            if (boState == BoState.Default || boState == BoState.BoStarted || boState == BoState.BOProcessed)
            {
                string argumentView = uoState == UoState.Logged ? "" : argument;
                _log += $"{argumentView} {operation} ";
            }
            else if (boState == BoState.BoChoose)
            {
                string[] splitLog = SplitLog;
                splitLog[splitLog.Length - 1] = operation + " ";
                _log = string.Join(" ", splitLog);
            }
        }

        public void LogUnOperation(
            BoState boState, string operation, string argument
            )
        {
            string[] splitLog = SplitLog;
            string lastLogArg = splitLog[splitLog.Length - 1];
            
            if (boState != BoState.BoChoose &&
                operation == "±" &&
                (_binOps.Contains(lastLogArg) || lastLogArg == "")) {}
            
            else if (_binOps.Contains(lastLogArg))
                _log += $"{_unOpsDict[operation]}({argument})";
            
            else
            { 
                string arg = lastLogArg == "" ? argument : lastLogArg;
                splitLog[splitLog.Length - 1] = $"{_unOpsDict[operation]}({arg})";
                _log = string.Join(" ", splitLog);
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

        public void LogPercentOperation(UoState uoState, double argument)
        {
            if (uoState == UoState.Default)
            {
                _log += $"{argument}";
            }
            else
            {
                string[] splitLog = SplitLog;
                splitLog[splitLog.Length - 1] = $"{argument}";
                _log = string.Join(" ", splitLog);
            }
        }
    } 
}