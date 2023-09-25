using StockroomBinar.Class;
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

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для DeitalesProductionPage.xaml
    /// </summary>
    public partial class DeitalesProductionPage : Page
    {
        public DeitalesProductionPage()
        {
            InitializeComponent();
            DitalesProductionView.ItemsSource = Connect.bd.DitalesProduction.ToList();
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DitalesProductionView.ItemsSource = Connect.bd.DitalesProduction.Where(p => p.CodeDitales.StartsWith(SearchColor.Text)).ToList();
        }

        private void AddDitalis_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddDitalesProductionPage());
        }
    }
}
