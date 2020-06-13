using System.Windows;
using System.Windows.Controls;
using WindowsFormsApp1;
namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calc;
        public MainWindow()
        {
            InitializeComponent();
            calc = new Calculator();
            calc.didUpdateValue += CalculatorDidUpdateValue;
            calc.InputError += CalculatorError;
            calc.ComputationError += CalculatorError;
            calc.Clear();
        }

        private void CalculatorError(Calculator sender, string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CalculatorDidUpdateValue(Calculator sender, double value, int precision)
        {
            if (precision > 0)
                output.Text = string.Format("{0:F" + precision + "}", value);
            else
                output.Text = $"{value}";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int digit = -1;

            if (int.TryParse(button.Content.ToString(), out digit))
            {
                calc.AddDigit(digit);
            }
            else
            {
                switch (button.Tag)
                {
                    case "Add":
                        calc.ProcessBinOp(CalculatorOperations.Add);
                        break;
                    case "Sub":
                        calc.ProcessBinOp(CalculatorOperations.Sub);
                        break;
                    case "Mul":
                        calc.ProcessBinOp(CalculatorOperations.Mul);
                        break;
                    case "Div":
                        calc.ProcessBinOp(CalculatorOperations.Div);
                        break;
                    case "Eq":
                        calc.ProcessBinOp(CalculatorOperations.Eq);
                        break;
                    case "Invert":
                        calc.ProcessUnOp(CalculatorUnOperations.Invert);
                        break;
                    case "Point":
                        calc.AddPoint();
                        break;
                    case "Reciprocal":
                        calc.ProcessUnOp(CalculatorUnOperations.Reciprocal);
                        break;
                    case "CE":
                        calc.Clear();
                        break;
                    case "C":
                        calc.ClearAll();
                        break;
                    case "Percent":
                        calc.ProcessBinOp(CalculatorOperations.Percent);
                        break;
                    case "Sqr":
                        calc.ProcessUnOp(CalculatorUnOperations.Sqr);
                        break;
                    case "Sqrt":
                        calc.ProcessUnOp(CalculatorUnOperations.Sqrt);
                        break;
                    case "Backspace":
                        calc.ProcessUnOp(CalculatorUnOperations.Backspace);
                        break;
                }
            }

        }

        //Calculator calc;
        //public MainWindow()
        //{
        //    InitializeComponent();
        //    calc = new Calculator();
        //    calc.DidUpdateValue += Calc_DidUpdateValue;
        //}

        //private void Calc_DidUpdateValue(Calculator sender, double value, int precision)
        //{
        //    output.Text = value.ToString();
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var button = sender as Button;
        //    int digit = -1;
        //    if (int.TryParse(button.Content.ToString(), out digit))
        //    {
        //        calc.AddDigit(digit);
        //    }
        //}
    }
}
