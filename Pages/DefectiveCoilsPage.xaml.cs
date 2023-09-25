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
    /// Логика взаимодействия для DefectiveCoilsPage.xaml
    /// </summary>
    public partial class DefectiveCoilsPage : Page
    {
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();
        public ColorPlastic colorPlastic = new ColorPlastic();

        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string[,] NameRecuclingPlast = new string[99, 99];//массив для хранения названий выбранных цветов для утелизации
        public DefectiveCoilsPage()
        {
            InitializeComponent();
            int x = 0;
            int y = 0;
            var a = Connect.bd.PlasticType.Where(p => p.ID != 0).Count();
            PlastType.Items.Add("Все типы");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var a1 = Connect.bd.PlasticType.First(p => p.ID == j);
                x++;
                PlastType.Items.Add(a1.NameType.ToString());
            }
            PlastType.SelectedIndex = 0;
            PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
        }

        private void AddRecyclingName_Checked(object sender, RoutedEventArgs e)
        {
            var a = PlastitDefectiveView.SelectedItem as DefectivePlastic;

            if (a != null)
            {
           
                if (NameRecuclingPlast[98, 0] != null) MessageBox.Show("Выбрано максимальное количество элементов для одной утелизации!");
                else
                {
                    for (int j = 0; j < 99; j++)
                    {
                        if (NameRecuclingPlast[j, 0] == null)
                        {
                            NameRecuclingPlast[j, 0] = a.ColorName;
                            break;
                        }
                    }
                }
            }
        }

        private void AddRecyclingName_Unchecked(object sender, RoutedEventArgs e)
        {
            var a = PlastitDefectiveView.SelectedItem as DefectivePlastic;
            if (a != null)
            {
                for (int j = 0; j < 99; j++)
                {
                    if (NameRecuclingPlast[j, 0] == a.ColorName)
                    {
                        NameRecuclingPlast[j, 0] = null;
                        break;
                    }
                }
            }
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
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
            }
            if (PlastType.SelectedIndex == 0)
            {
                PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
            }
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.ColorName.StartsWith(SearchColor.Text)).ToList();
        }

        private void RecyclingNameDel_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < 99; j++)
            {
                if (NameRecuclingPlast[j, 0] != null)
                {
                    string NamePlast = NameRecuclingPlast[j,0];
                    var objA = Connect.bd.DefectivePlastic.First(p => p.ColorName == NamePlast);
                    
                    var m = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == NamePlast&&p.PlasticTypeRecucling==objA.PlasticType&& p.ManufacturerRecucling==objA.Manufacturer).Count();
                    if (m > 0)
                    {
                        var m1 = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == NamePlast && p.PlasticTypeRecucling == objA.PlasticType && p.ManufacturerRecucling == objA.Manufacturer);
                        recyclingPlastic = m1;
                        recyclingPlastic.WeightRecucling = objA.Weight + recyclingPlastic.WeightRecucling; //!!!!!!!
                        Connect.bd.SaveChanges();
                        Connect.bd.DefectivePlastic.Remove(objA);
                        Connect.bd.SaveChanges();
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();

                    }
                    else
                    {
                        //var a = Connect.bd.RecyclingPlastic.Where(p => p.ID != 0).Count(); //считаем количество типов пластика
                        //recyclingPlastic.ID = int.Parse(a.ToString()) + 1;
                        recyclingPlastic.ColorNameRecucling = objA.ColorName;
                        recyclingPlastic.PlasticTypeRecucling = objA.PlasticType;
                        recyclingPlastic.ManufacturerRecucling = objA.Manufacturer;
                        recyclingPlastic.WeightRecucling = objA.Weight;
                        recyclingPlastic.PlasticStatus = objA.PlasticStatus;
                        Connect.bd.RecyclingPlastic.Add(recyclingPlastic);
                        Connect.bd.SaveChanges();
                        Connect.bd.DefectivePlastic.Remove(objA);
                        Connect.bd.SaveChanges();
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
                    }   
                }
            }
            for (int j = 0; j < 99; j++)
            {
                NameRecuclingPlast[j, 0] = null;
            }
            PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
        }
    }
}
