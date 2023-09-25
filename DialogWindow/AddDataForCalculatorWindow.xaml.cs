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
using System.Windows.Shapes;

namespace StockroomBinar.DialogWindow
{
    /// <summary>
    /// Логика взаимодействия для AddDataForCalculatorWindow.xaml
    /// </summary>
    public partial class AddDataForCalculatorWindow : Window
    {
        public AddDataForCalculatorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (WeightSupport.Text == null || WeightSupport.Text == "") WeightSupport.Text = "0";
            this.DialogResult = true;

        }
        public string WeightDitales
        {
            get { return Weight.Text+" "+WeightSupport.Text;
            }
        }
    }
}
