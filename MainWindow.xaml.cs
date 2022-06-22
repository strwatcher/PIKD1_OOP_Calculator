using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Calculator
{
    public partial class MainWindow
    {
        private readonly MathOperationsProcessor _processor = new MathOperationsProcessor();
        private readonly Logger _logger = new Logger();
        private readonly StateMachine _sm = new StateMachine();
        
        private string _curBinOp = "";
        private double _curArgument;
        private double _accumulator;

        private double CurNum
        {
            get => Convert.ToDouble(TbCurNum.Text);
            set => TbCurNum.Text = value.ToString();
        }

        public MainWindow()
        {
            InitializeComponent();
            TbCurNum.TextChanged += (sender, args) => FixCurrentNumTb();
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
                TbCurNum.Text[TbCurNum.Text.Length - 1] == '.',
                () =>
                {
                    if (TbCurNum.Text.Length > 1)
                        TbCurNum.Text = TbCurNum.Text.Substring(0, TbCurNum.Text.Length - 1);
                    else
                        CeButton_OnClick(sender, e);
                });
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            TbCurNum.Text = "0";
            _sm.ClearNumStates();
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            TbLog.Text = "";
            _curBinOp = "";
            _accumulator = 0.0;
            _sm.ClearCommonStates();
            _logger.Erase();
            CeButton_OnClick(sender, e);
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TbCurNum.Text == "0") TbCurNum.Text = "";
            string buttonText = (sender as Button)?.Content.ToString();
            _sm.UpdateStatesAfterDo(buttonText,
                () => TbCurNum.Text = "", 
                () => TbCurNum.Text += buttonText ?? String.Empty
                );
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_curBinOp != "")
            {
                TbLog.Text = "";
                
                _sm.UpdateStatesAfterEo(
                    () => _curArgument = CurNum,
                    () => _accumulator = CurNum); 
                
                _accumulator = _processor.ProcessOperation(
                    _curBinOp, new List<double> {_accumulator, _curArgument});
                
                CurNum = _accumulator;
                
                _logger.Erase();
            }
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            string buttonText = (sender as Button)?.Content.ToString();
            _sm.UpdateStatesAfterBo(
                (boState, uoState) =>
                {
                    _logger.LogBinOperation(boState, uoState, buttonText, CurNum.ToString());
                    TbLog.Text = _logger.Log;
                },
                () => _curBinOp = buttonText,
                () => _accumulator = CurNum,
                () =>
                {
                     _curArgument = CurNum;
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
                    TbLog.Text = _logger.Log;
                    CurNum = _processor.ProcessOperation(buttonText, new List<double> {CurNum});
                }, _logger.IsEmptyLogged);
        }

        private void FixCurrentNumTb()
        {
            FontSizeConverter fs = new FontSizeConverter();
            if (TbCurNum.Text.Length > 16)
                TbCurNum.FontSize = Convert.ToDouble(fs.ConvertFrom("14pt"));
            else if (TbCurNum.Text.Length > 12)
                TbCurNum.FontSize = Convert.ToDouble(fs.ConvertFrom("16pt"));
            else 
                TbCurNum.FontSize = Convert.ToDouble(fs.ConvertFrom("18pt")); 
            
            _sm.UpdateStatesAfterNumLenChange(TbCurNum.Text); 
        }
    }
}