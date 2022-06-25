using System;
using System.Collections.Generic;

namespace Calculator
{
    public class MathOperationsProcessor
    {
        private double _accumulator;
        private double _curArgument;
        private string _curOperation;
        
        private readonly Dictionary<string, Func<List<double>, double>> _operations =
            new Dictionary<string, Func<List<double>, double>>
            {
                {"+", arguments => arguments[0] + arguments[1]},
                {"-", arguments => arguments[0] - arguments[1]},
                {"*", arguments => arguments[0] * arguments[1]},
                {"/", arguments => arguments[0] / arguments[1]},
                {"±", arguments => -arguments[0]},
                {"1/x", arguments => 1 / arguments[0]},
                {"√", arguments => Math.Sqrt(arguments[0])},
                {"%", arguments => arguments[0] * (arguments[1] / 100.0)}
            };
        
        private double ProcessOperation(string type, List<double> arguments) => _operations[type](arguments);

        public double ProcessBinOperation(State state, string type, double argument)
        {
            if (state.BoState == BoState.Default || state.BoState == BoState.BOProcessed)
            {
                _curOperation = type;
                _curArgument = argument;
                _accumulator = argument;
                state.BoState = BoState.BoChoose;
                state.UoState = UoState.Default;
                return argument;
            }

            if (state.BoState == BoState.BoChoose)
            {
                _curOperation = type;
                return argument;
            }

            if (state.BoState == BoState.BoStarted)
            {
                _curArgument = argument;
                _accumulator = ProcessOperation(_curOperation, new List<double>{_accumulator, _curArgument});
                _curOperation = type;
                state.BoState = BoState.BoChoose;
                state.UoState = UoState.Default;
                return _accumulator;
            }
            
            return Double.NaN;
        }

        public double ProcessUnOperation(State state, string type, double argument)
        {
            state.BoState = state.BoState == BoState.BoStarted 
                ? BoState.BoStarted
                : BoState.Default;
            return ProcessOperation(type, new List<double> {argument});
        }

        public double ProcessPercentOperation(State state, double argument, string type = "%")
        {
            state.BoState = state.BoState == BoState.BoStarted 
                ? BoState.BoStarted
                : BoState.Default;
            return ProcessOperation(type, new List<double> {_accumulator, argument});
        }

        public double ProcessEqualsOperation(State state, double argument)
        {
            if (string.IsNullOrEmpty(_curOperation)) return argument; 
            if (state.BoState == BoState.BoChoose || state.BoState == BoState.BoStarted) 
            { 
                _curArgument = argument;
                state.BoState = BoState.BOProcessed;
            }
            else if (state.BoState == BoState.BOProcessed || state.BoState == BoState.Default) 
            { 
                _accumulator = argument;
            }

            _accumulator = ProcessOperation(_curOperation, new List<double> {_accumulator, _curArgument});
            return _accumulator;
        }

        public void ClearState()
        {
            _accumulator = 0.0;
            _curArgument = 0.0;
            _curOperation = "";
        }
    }
}