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
    /// Логика взаимодействия для PlasticDitalesPage.xaml
    /// </summary>
    public partial class PlasticDitalesPage : Page
    {
        public PlasticDitalesPage()
        {
            InitializeComponent();
            PlastitDitelisView.ItemsSource = Connect.bd.PlasticProducts.ToList();
        }

        private void AddDitalis_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddPlasticDitalesPage());
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PlastitDitelisView.ItemsSource = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID.StartsWith(SearchColor.Text)).ToList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
