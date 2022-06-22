﻿using System;
using System.Collections.Generic;

namespace Calculator
{ 
    public class MathOperationsProcessor
    {
        private Func<List<double>, double> _prevOperation;
        private List<double> _prevArguments;
        
        private readonly Dictionary<string, Func<List<double>, double>> _operations = 
            new Dictionary<string, Func<List<double>, double>>
            {
                {"+", arguments => arguments[0] + arguments[1]},
                {"-", arguments => arguments[0] - arguments[1]},
                {"*", arguments => arguments[0] * arguments[1]},
                {"/", arguments => arguments[0] / arguments[1]},
                {"±", arguments => -arguments[0]},
                {"1/x", arguments => 1/arguments[0]},
                {"√", arguments => Math.Sqrt(arguments[0])},
                
            };


        public double ProcessOperation(string type, List<double> arguments)
        {
            if (arguments.Count == 2)
            {
                _prevOperation = _operations[type];
                _prevArguments = arguments;
            }
            return _operations[type](arguments);  
        } 
    }
}