using StockroomBinar.BD;
using StockroomBinar.Class;
using StockroomBinar.DialogWindow;
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
    /// Логика взаимодействия для СalculatorPage.xaml
    /// </summary>
    public partial class СalculatorPage : Page
    {
        double Weight=0;
        double WeightSupport=0;
        string DetalesName;
        public СalculatorPage()
        {
            InitializeComponent();
            var a = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            AddNameDitalies.Items.Add("Выберите изделие");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDProductsProduction.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProduct.ToString());
                }
            }


            AddNameDitalies.SelectedIndex = 0;

           // EngravingText.Visibility = Visibility.Hidden;
        }

        private void Сalculate_Click(object sender, RoutedEventArgs e)
        {
            
            //if(Weight!=0|| Count.Text == null)
            //{


                int index1 = AddNameDitalies.SelectedIndex;
                if (AddNameDitalies.SelectedIndex == index1)
                {
                    if (index1 > 0)
                    {
                        var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index1);
                        DetalesName = a1.NameProduct;
                    var objA = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == DetalesName);
                    Weight = double.Parse(objA.ProductWeight.ToString());
                    if (double.Parse(objA.SupportsWeight.ToString()) != 0) WeightSupport = double.Parse(objA.SupportsWeight.ToString());
                }
                }
               

                SummWeght.Text = (((Weight + WeightSupport)* int.Parse(Count.Text))/1000).ToString()+ "кг.";
                if (((Weight + WeightSupport) * int.Parse(Count.Text))/1000  < 1)
                {
                    CoilsCount.Text = "> 1 катушки";
                }
                else
                {
                    
                    CoilsCount.Text = (((int)((Weight + WeightSupport) * int.Parse(Count.Text)) / 1000)+1).ToString();
                    //CoilsCount.Text = ((int)Math.Round(((Weight + WeightSupport) * int.Parse(Count.Text)) / 1000)).ToString()+" катушек";
                }


            //}
        }

        private void AddNameDitalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index1 = AddNameDitalies.SelectedIndex;
            if (AddNameDitalies.SelectedIndex == index1)
            {
                if (index1 > 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index1);
                    var objA = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == a1.NameProduct);
                    WeightOneDitales.Text = objA.ProductWeight.ToString();
                    WeightSupports.Text = objA.SupportsWeight.ToString();
                }
            }
            
        }

        private void EnterData_Click(object sender, RoutedEventArgs e)
        {
            AddDataForCalculatorWindow DataWindow = new AddDataForCalculatorWindow();
            if (DataWindow.ShowDialog() == true)
            {
                AddNameDitalies.SelectedIndex = 0;
                string a = DataWindow.WeightDitales.ToString();
                string[] words = a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Weight = double.Parse(words[0].ToString());
                WeightSupport = double.Parse(words[1].ToString());
                WeightOneDitales.Text = Weight.ToString();
                WeightSupports.Text = WeightSupport.ToString();
            }
        }

        private void AddCountDitalis_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Count.Text = AddCountDitalis.Text;
        }
    }
}
