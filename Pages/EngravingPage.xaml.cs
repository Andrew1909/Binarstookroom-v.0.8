using StockroomBinar.BD;
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
    /// Логика взаимодействия для EngravingPage.xaml
    /// </summary>
    public partial class EngravingPage : Page
    {
        public ProductsForEngraving forEngraving = new ProductsForEngraving();
        public EngravingPage()
        {
            InitializeComponent();
            EngravingView.ItemsSource = Connect.bd.ProductsForEngraving.ToList();
        }

        private void PlusEngravirig_Click(object sender, RoutedEventArgs e)
        {
            if(forEngraving.ReadyCount <= forEngraving.Count)
            {
                forEngraving.ReadyCount=forEngraving.ReadyCount+1;
                //проверка, из какой таблицы пришла деталь (из платика или производства)
                if (forEngraving.TypeDitalesID== 1)
                {
                    var objC = Connect.bd.PlasticProducts.First(p => p.ID == forEngraving.IDInside);
                    objC.EngravingStatus = objC.EngravingStatus + 1;
                    Connect.bd.SaveChanges();
                }
                if (forEngraving.TypeDitalesID == 2)
                {
                    var objC = Connect.bd.DitalesProduction.First(p => p.ID == forEngraving.IDInside);
                    objC.EngravingStatus = objC.EngravingStatus + 1;
                    Connect.bd.SaveChanges();
                }
                Connect.bd.SaveChanges();
                ReayEngraving.Text = forEngraving.ReadyCount.ToString();
                CountStock.Text = forEngraving.Count.ToString();
                EngravingView.ItemsSource = Connect.bd.ProductsForEngraving.ToList();
                if (forEngraving.ReadyCount == forEngraving.Count)
                {
                    Connect.bd.ProductsForEngraving.Remove(forEngraving);
                    Connect.bd.SaveChanges();
                    ReayEngraving.Text = "-";
                    CountStock.Text = "-";
                    IDDitalesText.Text = "-";
                    EngravingView.ItemsSource = Connect.bd.ProductsForEngraving.ToList();
                }
            }
           
        }

        private void MinusEngravirig_Click(object sender, RoutedEventArgs e)
        {
            if (forEngraving.ReadyCount > 0)
            {
                forEngraving.ReadyCount = forEngraving.ReadyCount - 1;
                Connect.bd.SaveChanges();

                //проверка, из какой таблицы пришла деталь (из платика или производства)
                if (forEngraving.TypeDitalesID == 1)
                {
                    var objC = Connect.bd.PlasticProducts.First(p => p.ID == forEngraving.IDInside);
                    objC.EngravingStatus = objC.EngravingStatus - 1;
                    Connect.bd.SaveChanges();
                }
                if (forEngraving.TypeDitalesID == 2)
                {
                    var objC = Connect.bd.DitalesProduction.First(p => p.ID == forEngraving.IDInside);
                    objC.EngravingStatus = objC.EngravingStatus - 1;
                    Connect.bd.SaveChanges();
                }
                ReayEngraving.Text = forEngraving.ReadyCount.ToString();
                CountStock.Text = forEngraving.Count.ToString();
                EngravingView.ItemsSource = Connect.bd.ProductsForEngraving.ToList();
            }
        }

        private void Engravirig_Click(object sender, RoutedEventArgs e)
        {
            var a = EngravingView.SelectedItem as ProductsForEngraving;
            if (a != null)
            {
                IDDitalesText.Text = a.ProductTypeID;
                ReayEngraving.Text = a.ReadyCount.ToString();
                CountStock.Text = a.Count.ToString();
                forEngraving = a;
            }
        }
    }
}
