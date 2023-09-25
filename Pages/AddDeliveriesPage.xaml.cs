using LiveCharts.Wpf;
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
    /// Логика взаимодействия для AddDeliveriesPage.xaml
    /// </summary>
    public partial class AddDeliveriesPage : Page
    {
        string[,] DetalesNeme = new string[99, 2];
        public DeliveriesProducts deliveriesProducts = new DeliveriesProducts();
        public Deliveries deliveries = new Deliveries();
        public AddDeliveriesPage()
        {
            InitializeComponent();
            AddCodeDitales.Text = "ВН";
            string a = DateTime.Now.ToString("yyyy");
            AddDate.Text = $"__.__.{a}";
        }

        private void AddDelivereies_Click(object sender, RoutedEventArgs e)
        {

            if (AddCustomer == null || AddDate.Text == null || DetalesNeme[0, 0] == null) MessageBox.Show("Не все поля заполнены!");
            else
            {
                deliveries.СustomerТame = AddCustomer.Text;
                deliveries.Date = DateTime.ParseExact(AddDate.Text, "dd.MM.yyyy", null);
                deliveries.Status = 0;
                Connect.bd.Deliveries.Add(deliveries);
                Connect.bd.SaveChanges();
                int position = 1;
                int maxID = (Connect.bd.Deliveries.Select(q => q.ID).Max());
                for (int c = 0; c < 99; c++)
                {
                    if (DetalesNeme[c, 0] != null)
                    {
                        deliveriesProducts.IDInside = maxID;
                        deliveriesProducts.CodeDitals = DetalesNeme[c, 0];
                        deliveriesProducts.ReadyDitals = 0;
                        deliveriesProducts.NecessaryCountDitals= int.Parse(DetalesNeme[c, 1]);
                        deliveriesProducts.Status = 0;
                        deliveriesProducts.NumberPosition = position;
                        Connect.bd.DeliveriesProducts.Add(deliveriesProducts);
                        Connect.bd.SaveChanges();
                        position++;
                    }
                }
                MessageBox.Show("Поставка добавлена!");
                var objA = Connect.bd.Deliveries.First(p => p.ID == maxID);
                MyFrame.Navigate(new DeliveresInfoPage(objA));
            }
        }
        public class AddtData
        {
            public string CodeDitals { get; set; }
            public string NeseseryDitales { get; set; }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (AddCodeDitales.Text != "ВН" && AddNececeryCount != null && AddCodeDitales.Text != null && AddNececeryCount.Text != "")
            {
                for (int j = 0; j < 99; j++)
                {
                    if (DetalesNeme[j, 0] == null)
                    {
                        DetalesNeme[j, 0] = AddCodeDitales.Text;
                        DetalesNeme[j, 1] = AddNececeryCount.Text;
                        DeliversProductView.Items.Add(new AddtData { CodeDitals = DetalesNeme[j, 0], NeseseryDitales = DetalesNeme[j, 1] });
                        break;
                    }
                }
                AddCodeDitales.Text = "ВН";
                AddNececeryCount.Text = string.Empty;

            }
            
        }

        private void AddCodeDitales_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
