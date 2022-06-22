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
        private readonly MathOperationsProcessor _processor = new MathOperationsProcessor();
        private readonly Logger _logger = new Logger();
        private readonly StateMachine _sm = new StateMachine();
        
        private string _curBinOp;
        private double _curArgument;
        private double _accumulator;

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
                DivideButton, MultButton
            };
            _unOps = new List<Button>
            {
                SignButton, InvertButton, SqrtButton
            };
            BindDigitButtons();
            BindUnOperations();
            BindBinOperations();
            TBCurNum.TextChanged += (sender, args) => FixCurrentNumTb();
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
            _sm.UpdateStatesAfterBackspace(
                TBCurNum.Text[TBCurNum.Text.Length - 1] == '.',
                () =>
                {
                    if (TBCurNum.Text.Length > 1)
                        TBCurNum.Text = TBCurNum.Text.Substring(0, TBCurNum.Text.Length - 1);
                    else
                        CeButton_OnClick(sender, e);
                });
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBCurNum.Text = "0";
            _sm.ClearNumStates();
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            TBLog.Text = "";
            _curBinOp = "";
            _accumulator = 0.0;
            _sm.ClearCommonStates();
            _logger.Erase();
            CeButton_OnClick(sender, e);
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TBCurNum.Text == "0") TBCurNum.Text = "";
            string buttonText = (sender as Button)?.Content.ToString();
            _sm.UpdateStatesAfterDo(buttonText,
                () => TBCurNum.Text = "", 
                () => TBCurNum.Text += buttonText ?? String.Empty
                );
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_curBinOp != "")
            {
                TBLog.Text = "";
                _sm.UpdateStatesAfterEo(() => _curArgument = CurNum);
                _accumulator = _processor.ProcessOperation(_curBinOp, new List<double> {_accumulator, _curArgument});
                CurNum = _accumulator;
            }
            _logger.Erase();
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            string buttonText = (sender as Button)?.Content.ToString();
            _sm.UpdateStatesAfterBo(
                buttonText,
                (boState, uoState) =>
                {
                    _logger.LogBinOperation(boState, uoState, buttonText, CurNum.ToString());
                    TBLog.Text = _logger.Log;
                },
                () =>
                {
                    _curBinOp = buttonText;
                    _accumulator = CurNum;
                },
                () => _curBinOp = buttonText,
                () =>
                {
                     _curArgument = CurNum;
                     _curBinOp = buttonText;
                     _accumulator = _processor.ProcessOperation(_curBinOp, new List<double> {_accumulator, _curArgument});
                     CurNum = _accumulator;
                }
            );
        }

        private void UnOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            _sm.UpdateStatesAfterUo(boState =>
                {
                    string buttonText = (sender as Button)?.Content.ToString();
                    if (buttonText == null) return;
                    _logger.LogUnOperation(boState, buttonText, CurNum.ToString());
                    TBLog.Text = _logger.Log;
                    CurNum = _processor.ProcessOperation(buttonText, new List<double> {CurNum});
                }, _logger.IsEmptyLogged);
        }

        private void FixCurrentNumTb()
        {
            FontSizeConverter fs = new FontSizeConverter();
            if (TBCurNum.Text.Length > 16)
                TBCurNum.FontSize = (double) fs.ConvertFrom("14pt");
            else if (TBCurNum.Text.Length > 12)
                TBCurNum.FontSize = (double) fs.ConvertFrom("18pt");
            else 
                TBCurNum.FontSize = (double) fs.ConvertFrom("20pt"); 
            
            _sm.UpdateStatesAfterNumLenChange(TBCurNum.Text); 
        }
    }
}