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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using StockroomBinar.BD;
using StockroomBinar.Class;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            var objA = Connect.bd.Deliveries.Where(p => p.ID != 0).Count(); //проверяем данные о поставках для отображения
            if(objA==0)
            {
                  ListViewItem lv = new ListViewItem();
                lv.Content = "Данные о поставках отсутсвуют";
                DeliversView.Items.Add(lv);
            }
            else DeliversView.ItemsSource = Connect.bd.Deliveries.ToList();

            objA = Connect.bd.Notifications.Where(p => p.ID != 0).Count(); //проверяем данные о уведомлениях для отображения
            if (objA == 0)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = "Уведомлений нет";
                NotificationsView.Items.Add(lv);
            }
            else NotificationsView.ItemsSource = Connect.bd.Notifications.ToList(); 


            CountPlastOnStock.Text = (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()).ToString();

            if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() == 1|| Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() == 21)
            {
                ColorCount.Text = "цвет";
            }
            if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()> 1&& Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()<5|| Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() > 21 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <=24)
            {
                ColorCount.Text = "цвета";
            }
            if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >=5 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <=20)
            {
                ColorCount.Text = "цветов";
            }
            if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >= 21 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <= 20)
            {
                ColorCount.Text = "цветов";
            }
            if(Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >= 25)
            {
                ColorCount.Text = "цвета";
            }

            CountDitalsOnStock.Text = (Connect.bd.DitalesProduction.Where(p => p.ID != 0).Count()+ Connect.bd.PlasticProducts.Where(p => p.ID != 0).Count()).ToString();

            pieChart();
            DiagramPlast();
            DioagramDitals();
            
        }
        public void DiagramPlast()
        {
            int n = 0;

            var SumCount=Connect.bd.PlasticStor.Where(p=>p.ID != 0).Count();
            if (SumCount != 0)
            {
                int[] arr = new int[SumCount];

                int minID = Connect.bd.PlasticStor.Select(q => q.ID).Min();

                for (int j = 0; j < SumCount; j++)
                {
                    var objA = Connect.bd.PlasticStor.Where(p => p.ID == minID).Count();

                    if (objA != 0)
                    {
                        var objB = Connect.bd.PlasticStor.First(p => p.ID == minID);
                        arr[j] = int.Parse(objB.NumberСoils.ToString());
                        minID++;
                    }
                    else
                    {
                        minID++;
                    }
                }

                int max = arr[0];
                int imax = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] > max)
                    {
                        max = arr[i];
                        imax = i;
                        break;
                    }
                }
                int max1 = max;
                int imax1 = imax + 1;
                for (int i = imax; i < arr.Length; i++)
                {
                    if (arr[i] > max)
                    {
                        max1 = arr[i];
                        imax1 = i;
                        break;
                    }
                }
                int max2 = max1;
                int imax2 = imax1 + 1;
                for (int i = imax2; i < arr.Length; i++)
                {
                    if (arr[i] > max)
                    {
                        max2 = arr[i];
                        break;
                    }
                }
                var objMax1 = Connect.bd.PlasticStor.First(p=>p.NumberСoils==max);
                var objMax2 = Connect.bd.PlasticStor.First(p => p.NumberСoils == max1);
                var objMax3 = Connect.bd.PlasticStor.First(p => p.NumberСoils == max2);
                string MaxName1 = objMax1.ColorName;
                string MaxName2 = objMax2.ColorName;
                string MaxName3 = objMax3.ColorName;
                seriesCollection = new SeriesCollection
                {

                new PieSeries
                {
                    
                    Title=MaxName1,
                    Values=new ChartValues<ObservableValue> {new ObservableValue(max)},
                    DataLabels=true
                },
                new PieSeries
                {
                    Title=MaxName2,
                    Values=new ChartValues<ObservableValue> {new ObservableValue(max1)},
                    DataLabels=true
                },
                new PieSeries
                {
                    Title=MaxName3,
                    Values=new ChartValues<ObservableValue> {new ObservableValue(max2)},
                    DataLabels=true
                },
                new PieSeries
                {
                    Title="Другие...",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(2)},
                    DataLabels=true
                },
            };
                DataContext = this;
            }
           
        }

        public void DioagramDitals()
        {

            seriesCollection2 = new SeriesCollection
            {

                new PieSeries
                {
                    Title="Salut1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(3)},
                    DataLabels=true
                },
                new PieSeries
                {
                    Title="Salut2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(7)},
                    DataLabels=true
                },
                new PieSeries
                {
                    Title="Salut3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(2)},
                    DataLabels=true
                },
            };
            DataContext = this;
        }

        public void pieChart()
        {
            Pointlabel = ChartPoint => string.Format("{0}({1:P)", ChartPoint.Y, ChartPoint.Participation);
            DataContext = this;
        }

        public Func<ChartPoint, string> Pointlabel { get; set; }
        public SeriesCollection seriesCollection { get; set; }
        public SeriesCollection seriesCollection2 { get; set; }
        private void info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchInfoNat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LockInfoNatif_Click(object sender, RoutedEventArgs e)
        {
            var a = DeliversView.SelectedItem as Deliveries;
            if (a != null)
            {
                MyFrame.Navigate(new DeliveresInfoPage(a));
            }

        }

        private void DeleteNotifications_Click(object sender, RoutedEventArgs e)
        {
            var a = NotificationsView.SelectedItem as Notifications;
            if (a != null)
            {
                Connect.bd.Notifications.Remove(a);
                Connect.bd.SaveChanges();
                NotificationsView.ItemsSource = Connect.bd.Notifications.ToList();
            }
           //var objA = Connect.bd.Notifications.Where(p => p.ID != 0).Count(); //проверяем данные о уведомлениях для отображения
           // if (objA == 0)
           // {
           //     ListViewItem lv = new ListViewItem();
           //     lv.Content = "Уведомлений нет";
           //     NotificationsView.Items.Add(lv);
           // }
           // else NotificationsView.ItemsSource = Connect.bd.Notifications.ToList();
        }
    }
}
