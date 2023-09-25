using MaterialDesignColors;
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
    /// Логика взаимодействия для PlasticStorage.xaml
    /// </summary>
    public partial class PlasticStorage : Page
    {
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();
        public PlasticStor plasticStor = new PlasticStor();
        public Notifications notifications = new Notifications();
        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        public PlasticStorage()
        {
            InitializeComponent();
            MyFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            var a = Connect.bd.PlasticType.Where(p => p.ID != 0).Count(); //считаем количество типов пластика
            PlastType.Items.Add("Все типы");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var a1 = Connect.bd.PlasticType.First(p => p.ID == j);
                PlastType.Items.Add(a1.NameType.ToString());
            }
            PlastType.SelectedIndex = 0;
            var plast = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count(); //считаем количество пластика
            plast++;
            for (int i = 0; i < int.Parse(plast.ToString()); i++)
            {
                var plast2 = Connect.bd.PlasticStor.Where(p => p.Weight.Value <= 0.2m ).Count();//считаем количество пластика для списания. МИНИМАЛЬНОЕ ЧИСЛО СПИСАНИЯ ТУТ
                if (plast2 != 0)
                {
                    var objX = Connect.bd.PlasticStor.First(p => p.Weight.Value <= 0.2m);
                    var objY = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == objX.ColorName && p.PlasticTypeRecucling==objX.PlasticType &&p.ManufacturerRecucling==objX.Manufacturer).Count();
                    if (objY != 0)
                    {
                        AddNatification(objX.ID);
                        var objY1 = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == objX.ColorName && p.PlasticTypeRecucling == objX.PlasticType && p.ManufacturerRecucling == objX.Manufacturer);
                        recyclingPlastic = objY1;
                        recyclingPlastic.WeightRecucling = recyclingPlastic.WeightRecucling + objX.Weight;                      
                        Connect.bd.SaveChanges();
                        Connect.bd.PlasticStor.Remove(objX);
                        Connect.bd.SaveChanges();
                    }
                    else
                    {
                        AddNatification(objX.ID);
                        recyclingPlastic.ID = objX.ID;
                        recyclingPlastic.ColorNameRecucling = objX.ColorName;
                        recyclingPlastic.PlasticTypeRecucling = objX.PlasticType;
                        recyclingPlastic.ManufacturerRecucling = objX.Manufacturer;
                        recyclingPlastic.WeightRecucling = objX.Weight;
                        var plast3 = Connect.bd.PlasticStor.Where(p => p.ColorName == objX.ColorName).Count();
                        if (plast3 > 1)
                        {
                            recyclingPlastic.PlasticStatus = 1;
                        }
                        else recyclingPlastic.PlasticStatus = 0;
                        Connect.bd.RecyclingPlastic.Add(recyclingPlastic);
                        Connect.bd.SaveChanges();
                        Connect.bd.PlasticStor.Remove(objX);
                        Connect.bd.SaveChanges();
                    }
                    
                }
            }

            var c = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.PlasticStor.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.PlasticStor.Select(q => q.ID).Max();
                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.PlasticStor.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.PlasticStor.First(p => p.ID == n);
                        NullElement.IDInsaid = st;
                        Connect.bd.SaveChanges();
                        n = NullElement.ID;
                        n++;
                        st++;
                    }
                    if (t == 0)
                    {
                        n++;
                    }
                }
            }
            PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
        }

        void AddNatification(int id)
        {
            var objX = Connect.bd.PlasticStor.First(p => p.ID == id);
            notifications.Descriptiont = $"Пластик {objX.ColorName} от производятеля {objX.Manufacturer} закончился";
            Connect.bd.Notifications.Add(notifications);
            Connect.bd.SaveChanges();
        }

        private void PlastType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = PlastType.SelectedIndex;
            if (PlastType.SelectedIndex == index)
            {
                if (index > 0)
                { 
                    var a1 = Connect.bd.PlasticType.First(p => p.ID == index);
                    TypeNamePlast = a1.NameType;
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
            }
            if (PlastType.SelectedIndex == 0)
            {   
                PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
            }
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.ColorName.StartsWith(SearchColor.Text)).ToList();
        }

        private void AddPlatic_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddPlasticPage());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var a = PlastitStoageView.SelectedItem as PlasticStor;
            if (a != null)
            {
                MyFrame.Navigate(new EditInfoPlastPage(a));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
