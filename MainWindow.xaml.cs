using StockroomBinar.Pages;
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

namespace StockroomBinar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            MyFrame.Navigate(new HomePage());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlasticOnStock_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new PlasticStorage());
        }

        private void Recycling_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new WasteRecyclingPage());
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Defective_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new DefectiveCoilsPage());

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new HomePage());
        }

        private void SettingWindow_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new SettingPage());
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new СalculatorPage());
        }

        private void Delivering_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new DeliveresPage()) ;
        }

        private void PlasticDitals_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new PlasticDitalesPage());
        }

        private void DitalsFromProduction_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new DeitalesProductionPage());
        }

        private void Engraving_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new EngravingPage());
        }
    }
}
