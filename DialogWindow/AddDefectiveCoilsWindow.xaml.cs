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
    /// Логика взаимодействия для AddDefectiveCoilsWindow.xaml
    /// </summary>
    public partial class AddDefectiveCoilsWindow : Window
    {
        public AddDefectiveCoilsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }
        public string Defective
        {
            get { return CountDefectiveCoils.Text; }
        }
    }
}
