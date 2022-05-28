using System;
using System.Collections.Generic;
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
        private List<Button> _digits;
        private List<Button> _binOperations;
        private List<Button> _unOperations;

        public MainWindow()
        {
            InitializeComponent();
            bindDigitButtons();
            bindUnOperations();
            bindBinOperations();
        }

        private void bindDigitButtons()
        {
            _digits = new List<Button>
            {
                Button1, Button2, Button3,
                Button4, Button5, Button6,
                Button7, Button8, Button9,
                DotButton, Button0
            };
            foreach (var button in _digits)
                button.Click += DigitButton_OnClick;
            
        }

        private void bindUnOperations()
        {
            _unOperations = new List<Button>
            {

            };

            foreach (var unOperation in _unOperations)
                unOperation.Click += UnOperationButton_OnClick;
            
        }
        
        private void bindBinOperations()
        {
            _binOperations = new List<Button>
            {

            };
            foreach (var button in _binOperations)
                button.Click += BinOperationButton_OnClick;
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
            if (LabelCurrentNum.Content.ToString().Length > 1)
            {
                string currentNumContent = LabelCurrentNum.Content.ToString();
                LabelCurrentNum.Content = currentNumContent.Substring(0, currentNumContent.Length - 1);
            }
            else
                CeButton_OnClick(sender, e);
        }

        private void CeButton_OnClick(object sender, RoutedEventArgs e)
        {
            LabelCurrentNum.Content = 0;
        }

        private void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            LabelLog.Content = null;
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DigitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (LabelCurrentNum.Content?.ToString() == "0") 
                LabelCurrentNum.Content = null;
            
            LabelCurrentNum.Content += (sender as Button)?.Content.ToString();
        }

        private void BinOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UnOperationButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}