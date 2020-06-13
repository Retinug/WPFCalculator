using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            calc.DidUpdateValue += Calc_DidUpdateValue;
        }

        private void Calc_DidUpdateValue(Calculator sender, double value, int precision)
        {
            output.Text = value.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int digit = -1;
            if (int.TryParse(button.Content.ToString(), out digit))
            {
                calc.AddDigit(digit);
            }
        }
    }
}
