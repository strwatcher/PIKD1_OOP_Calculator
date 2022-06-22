﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow
    {
        private readonly List<Button> _digits;
        private readonly List<Button> _binOps;
        private readonly List<Button> _unOps;
        private readonly Dictionary<string, string> _unOpsDict = new Dictionary<string, string>
        {
            {"√", "sqrt"},
            {"±", "negate"},
            {"1/x", "reciproc"}
        };
        private MathOperationsProcessor _processor = new MathOperationsProcessor();
        
        private OpsState _opState = OpsState.Default;
        private NumState _numState = NumState.Default;
        private BOArgState _boArgState = BOArgState.Default;
        private DotState _dotState = DotState.NotExists;
        private string _curBinOp = null;
        private double? _curArgument = null;
        private double? _accumulator = null;

        private double CurNum
        {
            get => Convert.ToDouble(TBCurNum.Text);
            set => TBCurNum.Text = value.ToString();
        }

        public MainWindow()
        {
            InitializeComponent();
            _digits = new List<Button>
            {
                Button1, Button2, Button3, Button4, Button5, Button6, 
                Button7, Button8, Button9, DotButton, Button0
            };
            _binOps = new List<Button>
            {
                PlusButton, MinusButton,
                DivideButton, MultButton,
            };
            _unOps = new List<Button>
            {
                SignButton, InvertButton, SqrtButton
            };
            BindDigitButtons();
            BindUnOperations();
            BindBinOperations();
            TBCurNum.TextChanged += (sender, args) => FixCurrentNumTB();
        }

        private void BindDigitButtons()
        {
            foreach (var digit in _digits)
                digit.Click += DigitButton_OnClick;
            
        }
        private void BindUnOperations()
        {
            foreach (var unOperation in _unOps) 
                unOperation.Click += UnOperationButton_OnClick;
        }
        
        private void BindBinOperations()
        {
            foreach (var binOperation in _binOps)
                binOperation.Click += BinOperationButton_OnClick;
        }
        
        private void McButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MrButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MsButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MPlusButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MMinusButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BackspaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TBCurNum.Text[TBCurNum.Text.Length - 1] == '.') _dotState = DotState.NotExists;
            if (TBCurNum.Text.Length > 1)
                TBCurNum.Text = TBCurNum.Text.Substring(0, TBCurNum.Text.Length - 1);
            else
                CeButton_OnClick(sender, e);
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBCurNum.Text = "0";
            _dotState = DotState.NotExists;
            _numState = NumState.Default;
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBLog.Text = "";
            _accumulator = 0.0;
            _curBinOp = "";
            _opState = OpsState.Default;
            _boArgState = BOArgState.Default;
            CeButton_OnClick(sender, e);
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TBCurNum.Text == "0") TBCurNum.Text = "";
            string buttonText = (sender as Button)?.Content.ToString();
            if (_numState == NumState.Default ||
                _numState == NumState.WaitForLast || 
                _numState == NumState.WaitForDot && buttonText == ".")
            {
                if (buttonText == "." && _dotState == DotState.Exists) return;
                if (buttonText == ".") _dotState = DotState.Exists;
                
                TBCurNum.Text += buttonText ?? String.Empty;
            }
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void UnOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void FixCurrentNumTB()
        {
            FontSizeConverter fs = new FontSizeConverter();
            if (TBCurNum.Text.Length > 16)
                TBCurNum.FontSize = (double) fs.ConvertFrom("14pt");
            else if (TBCurNum.Text.Length > 12)
                TBCurNum.FontSize = (double) fs.ConvertFrom("18pt");
            else 
                TBCurNum.FontSize = (double) fs.ConvertFrom("20pt");
            
            if (TBCurNum.Text.Length == 16)
            {
                _numState = _dotState == DotState.Exists 
                    ? NumState.WaitForLast 
                    : NumState.WaitForDot;
            } 
            else if (TBCurNum.Text.Length < 16) _numState = NumState.Default;
            else _numState = NumState.Overflow;
        }
    }
}