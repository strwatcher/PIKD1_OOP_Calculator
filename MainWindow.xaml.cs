using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Calculator
{
    public partial class MainWindow
    {
        private readonly MathOperationsProcessor _processor = new MathOperationsProcessor();
        private readonly Logger _logger = new Logger();
        private readonly State _state = new State();
        
        private double _savedValue;

        private double CurNum
        {
            get => Convert.ToDouble(TbCurNum.Text);
            set
            {
                if (Double.IsNaN(value))
                {
                    TbCurNum.Text = "Invalid input";
                    _state.EState = ExceptionState.InvalidInput;
                }
                else if (Double.IsInfinity(value))
                {
                    TbCurNum.Text = "Overflow";
                    _state.EState = ExceptionState.Overflow;
                }
                else 
                    TbCurNum.Text = value.ToString();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            TbCurNum.TextChanged += (sender, args) => FixCurrentNumTb();
        }
        
        private void McButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            _savedValue = 0.0;
            MLabel.Content = "";
        }

        private void MrButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            CurNum = _savedValue;
        }

        private void MsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            _savedValue = CurNum;
            MLabel.Content = "M";
        }

        private void MPlusButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            _savedValue += CurNum;
            MLabel.Content = "M";
        }

        private void MMinusButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            _savedValue -= CurNum;
            MLabel.Content = "M";
        }

        private void BackspaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations() || _state.BoState == BoState.BoChoose) return;
            if (TbCurNum.Text[TbCurNum.Text.Length - 1] == '.') _state.DotState = DotState.NotExists;
            if (TbCurNum.Text.Length > 1) 
                TbCurNum.Text = TbCurNum.Text.Substring(0, TbCurNum.Text.Length - 1);
            else CeButton_OnClick(sender, e);
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations())
            {
                CButton_OnClick(sender, e);
            }
            else
            {
                TbCurNum.Text = "0";
                _state.ClearNumStates();
            }
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            _logger.Erase();
            TbLog.Text = _logger.Log;
            _processor.ClearState();
            _state.ClearCommonStates();
            CeButton_OnClick(sender, e);
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            if (TbCurNum.Text == "0") TbCurNum.Text = "";
            string operationType = (sender as Button)?.Content.ToString();
            if (_state.UoState == UoState.Logged || _state.BoState == BoState.BoChoose ||
                _state.BoState == BoState.BOProcessed)
            {
                TbCurNum.Text = "";
            }

            if (_state.UoState == UoState.Logged)
            {
                _logger.DeleteLastLog();
                TbLog.Text = _logger.Log;
                _state.UoState = UoState.Default;
            }
            if (_state.BoState == BoState.BoChoose) _state.BoState = BoState.BoStarted;
            if (_state.BoState == BoState.BOProcessed) _state.BoState = BoState.Default;
            if (_state.NumState == NumState.Default || _state.NumState == NumState.WaitForLast ||
                _state.NumState == NumState.WaitForDot && operationType == ".")
            {
                if (operationType == "." && _state.DotState == DotState.Exists) return;
                if (operationType == ".") _state.DotState = DotState.Exists;
                if (operationType != null) TbCurNum.Text += operationType;
            }
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            _logger.Erase();
            TbLog.Text = _logger.Log;
            CurNum = _processor.ProcessEqualsOperation(_state, CurNum);
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            string operationType = (sender as Button)?.Content.ToString();
            _logger.LogBinOperation(_state, operationType, CurNum);
            TbLog.Text = _logger.Log;
            CurNum = _processor.ProcessBinOperation(_state, operationType, CurNum);
        }

        private void UnOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            string operationType = (sender as Button)?.Content.ToString();
            _logger.LogUnOperation(_state, operationType, CurNum);
            TbLog.Text = _logger.Log;
            CurNum = _processor.ProcessUnOperation(_state, operationType, CurNum);
        }

        private void PercentButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_state.CanDoOperations()) return;
            CurNum = _processor.ProcessPercentOperation(_state, CurNum);
            _logger.LogPercentOperation(_state, CurNum);
            TbLog.Text = _logger.Log;
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
            
            _state.UpdateStatesAfterNumLenChange(TbCurNum.Text); 
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                if (e.Key == Key.Q) MMinusButton_OnClick(MMinusButton, e);
                else if (e.Key == Key.P) MPlusButton_OnClick(MPlusButton, e);
                else if (e.Key == Key.M) MsButton_OnClick(MsButton, e);
                else if (e.Key == Key.R) MrButton_OnClick(MrButton, e);
                else if (e.Key == Key.L) McButton_OnClick(McButton, e);
            }
            else if (e.Key == Key.D2 && (Keyboard.Modifiers & ModifierKeys.Shift) != 0) 
                UnOperationButton_OnClick(SqrtButton, e);
            else if (e.Key == Key.F9) UnOperationButton_OnClick(SignButton, e);
            else if (e.Key == Key.R) UnOperationButton_OnClick(InvertButton, e);
            
            else if (e.Key == Key.D5 && (Keyboard.Modifiers & ModifierKeys.Shift) != 0) 
                PercentButton_OnClick(PercentButton, e);
            
            else if (e.Key == Key.Multiply || e.Key == Key.D8 && (Keyboard.Modifiers & ModifierKeys.Shift) != 0) 
                BinOperationButton_OnClick(MultButton, e);
            else if (e.Key == Key.Add || e.Key == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Shift) != 0) 
                BinOperationButton_OnClick(PlusButton, e);
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus) BinOperationButton_OnClick(MinusButton, e);
            else if (e.Key == Key.Divide || e.Key == Key.Oem2) BinOperationButton_OnClick(DivideButton, e);
                        
            else if (e.Key == Key.Enter || e.Key == Key.OemPlus) EqualsButton_OnClick(EqualsButton, e);
                        
            else if (e.Key == Key.Back) BackspaceButton_OnClick(BackspaceButton, e);
            else if (e.Key == Key.Delete) CeButton_OnClick(CeButton, e);
            else if (e.Key == Key.Escape) CButton_OnClick(CButton, e);

            else if (e.Key == Key.D0 || e.Key == Key.NumPad0) DigitButton_OnClick(Button0, e);
            else if (e.Key == Key.D1 || e.Key == Key.NumPad1) DigitButton_OnClick(Button1, e);
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2) DigitButton_OnClick(Button2, e);
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3) DigitButton_OnClick(Button3, e);
            else if (e.Key == Key.D4 || e.Key == Key.NumPad4) DigitButton_OnClick(Button4, e);
            else if (e.Key == Key.D5 || e.Key == Key.NumPad5) DigitButton_OnClick(Button5, e);
            else if (e.Key == Key.D6 || e.Key == Key.NumPad6) DigitButton_OnClick(Button6, e);
            else if (e.Key == Key.D7 || e.Key == Key.NumPad7) DigitButton_OnClick(Button7, e);
            else if (e.Key == Key.D8 || e.Key == Key.NumPad8) DigitButton_OnClick(Button8, e);
            else if (e.Key == Key.D9 || e.Key == Key.NumPad9) DigitButton_OnClick(Button9, e);
            else if (e.Key == Key.OemPeriod || e.Key == Key.OemComma) DigitButton_OnClick(DotButton, e);
        }
    }
}