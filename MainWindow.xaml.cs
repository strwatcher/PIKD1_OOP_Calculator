using System;
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
        private CurNumState _curState = CurNumState.Default;
        private BOArgState _boArgState = BOArgState.Default;
        private DotState _dotState = DotState.NotExists;
        private string _curBinOp;
        private double _curArgument;
        private double _accumulator;

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
            TBCurrentNum.TextChanged += (sender, args) => FixCurrentNumTB();
        }

        private double CurrentNum => Convert.ToDouble(TBCurrentNum.Text);

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
            if (TBCurrentNum.Text[TBCurrentNum.Text.Length - 1] == '.') _dotState = DotState.NotExists;
            if (TBCurrentNum.Text.Length > 1)
                TBCurrentNum.Text = TBCurrentNum.Text.Substring(0, TBCurrentNum.Text.Length - 1);
            else
                CeButton_OnClick(sender, e);
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBCurrentNum.Text = "0";
            _dotState = DotState.NotExists;
            _curState = CurNumState.Default;
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBLog.Text = "";
            _accumulator = 0.0;
            _curBinOp = "";
            _opState = OpsState.Default;
            _curState = CurNumState.Default;
            _boArgState = BOArgState.Default;
            CeButton_OnClick(sender, e);
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_opState == OpsState.BOProcessing)
            {
                _curArgument = Convert.ToDouble(TBCurrentNum.Text);
                _opState = OpsState.BOProcessed;
            }
            if (_opState == OpsState.BOProcessed)
            {
                _accumulator = _processor.ProcessOperation(_curBinOp,
                    new List<double> {_accumulator, _curArgument});
                TBCurrentNum.Text = _accumulator.ToString();
            }
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_opState == OpsState.BOProcessing && _boArgState == BOArgState.Default)
            {
                CeButton_OnClick(sender, e);
                _boArgState = BOArgState.Changed;
            }
            if (TBCurrentNum.Text == "0") TBCurrentNum.Text = "";
            if (_curState == CurNumState.Overflow) return;
            if ((sender as Button)?.Content.ToString() == ".")
            {
                if (_dotState == DotState.Exists) return;
                _dotState = DotState.Exists;
            } 
            
            TBCurrentNum.Text += (sender as Button)?.Content.ToString() ?? string.Empty;
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            _curBinOp = (sender as Button)?.Content.ToString();
            _boArgState = BOArgState.Default;
            if (_opState == OpsState.Default || _opState == OpsState.BOProcessed)
            {
                _opState = OpsState.BOProcessing;
                _curBinOp = (sender as Button)?.Content.ToString();
                _accumulator = Convert.ToDouble(TBCurrentNum.Text);
            }
            else
            {
                _accumulator = _processor.ProcessOperation(_curBinOp,
                    new List<double> {_accumulator, Convert.ToDouble(TBCurrentNum.Text)});
                TBCurrentNum.Text = _accumulator.ToString();
                _curArgument = _accumulator;
            }

        }

        private void UnOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            double result = _processor.ProcessOperation(
                (sender as Button)?.Content.ToString(), new List<double> {CurrentNum});
            TBCurrentNum.Text = result.ToString();
            
        }

        private void FixCurrentNumTB()
        {
            FontSizeConverter fs = new FontSizeConverter();
            if (TBCurrentNum.Text.Length > 16)
                TBCurrentNum.FontSize = (double) fs.ConvertFrom("14pt");
            else if (TBCurrentNum.Text.Length > 12)
                TBCurrentNum.FontSize = (double) fs.ConvertFrom("18pt");
            else 
                TBCurrentNum.FontSize = (double) fs.ConvertFrom("20pt");
            
            _curState = TBCurrentNum.Text.Length == 16
                ? CurNumState.Overflow
                : CurNumState.Default;
        }
    }
}