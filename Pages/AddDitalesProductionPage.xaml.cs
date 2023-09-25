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
    /// Логика взаимодействия для AddDitalesProductionPage.xaml
    /// </summary>
    public partial class AddDitalesProductionPage : Page
    {
        public IDProductsProduction productsProduction = new IDProductsProduction();
        public ProductsForEngraving forEngraving = new ProductsForEngraving();
        public DitalesProduction ditalesProduction = new DitalesProduction();
        int CountEngraving;
        string NameDitaliesID = "";
        public AddDitalesProductionPage()
        {
            InitializeComponent();

            var a = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            AddNameDitalies.Items.Add("Выберите изделие");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDProductsProduction.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDProductsProduction.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProducts.ToString());
                }
            }

           
            AddNameDitalies.SelectedIndex = 0;

            EngravingText.Visibility = Visibility.Hidden;
        }

        private void AddNameDitalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddDitalis_Click(object sender, RoutedEventArgs e)
        {
            if (AddNameDitalies.SelectedIndex == 0 || AddCountDitalis.Text == null)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                int index1 = AddNameDitalies.SelectedIndex;
                if (AddNameDitalies.SelectedIndex == index1)
                {
                    if (index1 > 0) //запиывем в переменные выборы из комбобокса
                    {
                        var a1 = Connect.bd.IDProductsProduction.First(p => p.IDInside == index1);
                        NameDitaliesID = a1.NameProducts;
                    }
                }
                var objA = Connect.bd.DitalesProduction.Where(p => p.CodeDitales == NameDitaliesID).Count(); //проверяем, есть ли в БД уже эта деталь
                if (objA != 0)
                {
                    //есть ли такая деталь с той же датой производства

                    var objB = Connect.bd.DitalesProduction.First(p => p.CodeDitales == NameDitaliesID);


                    if (CountEngraving != 0)
                    {
                        EngraveringFunc(objB.ID, 2);

                        objB.CountOnStoock = int.Parse(AddCountDitalis.Text) + objB.CountOnStoock;
                        objB.EngravingStatus = objB.EngravingStatus + (int.Parse(AddCountDitalis.Text) - CountEngraving);//количество програвированных
                        Connect.bd.SaveChanges();
                        MessageBox.Show("Детали добавлены к существующей записи!");
                        MyFrame.Navigate(new DeitalesProductionPage());
                    }
                    else
                    {

                        objB.CountOnStoock = int.Parse(AddCountDitalis.Text) + objB.CountOnStoock;
                        objB.EngravingStatus = objB.CountOnStoock;
                        Connect.bd.SaveChanges();
                        MessageBox.Show("Детали добавлены к существующей записи!");
                        MyFrame.Navigate(new DeitalesProductionPage());
                    }
                }
                else
                {
                    AddNewDeitales();
                }
            }
        }

        void AddNewDeitales()
        {
            if (CountEngraving != 0) ditalesProduction.EngravingStatus = int.Parse(AddCountDitalis.Text) - CountEngraving;
            else ditalesProduction.EngravingStatus = int.Parse(AddCountDitalis.Text);
            ditalesProduction.CodeDitales = NameDitaliesID;
            ditalesProduction.CountOnStoock = int.Parse(AddCountDitalis.Text);
            Connect.bd.DitalesProduction.Add(ditalesProduction);
            Connect.bd.SaveChanges();
            MessageBox.Show("Деталь добавлена!");
            if (CountEngraving != 0)
            {
                var objB = Connect.bd.DitalesProduction.First(p => p.CodeDitales == NameDitaliesID);
                EngraveringFunc(objB.ID, 1);//отправляем чать на гравировку
            }
            MyFrame.Navigate(new DeitalesProductionPage());
        }

        void EngraveringFunc(int id, int TableID)
        {
            var objA = Connect.bd.ProductsForEngraving.Where(p => p.IDInside == id&&p.TypeDitalesID==TableID).Count();
            if (objA != 0)
            {
                var objA1 = Connect.bd.ProductsForEngraving.First(p => p.IDInside == id && p.TypeDitalesID == TableID);
                objA1.Count = objA1.Count + CountEngraving;
                Connect.bd.SaveChanges();

            }
            else
            {
                forEngraving.ProductTypeID = NameDitaliesID;
                forEngraving.IDInside = id;
                forEngraving.Count = CountEngraving;
                forEngraving.ReadyCount = 0;
                forEngraving.TypeDitalesID = 2;//указывем из какой таблицы пришла деталь
                Connect.bd.ProductsForEngraving.Add(forEngraving);
                Connect.bd.SaveChanges();
            }
        }

        private void Engraving_Checked(object sender, RoutedEventArgs e)
        {
            CountEngravingWindow EngiviringsWindow = new CountEngravingWindow();
            if (EngiviringsWindow.ShowDialog() == true)
            {

                EngravingCountsText.Text = EngiviringsWindow.EniviringCount.ToString();
                EngravingText.Visibility = Visibility.Visible;
                CountEngraving = int.Parse(EngiviringsWindow.EniviringCount);
            }
            else
            {
                Engraving.IsChecked = false;
            }
        }
    

        private void Engraving_Unchecked(object sender, RoutedEventArgs e)
        {
            CountEngraving = 0;
            EngravingText.Visibility = Visibility.Hidden;
        }

        private void AddNewNameDitales_Click(object sender, RoutedEventArgs e)
        {
            AddNewDitalesWindow DitalesWindow = new AddNewDitalesWindow();
            if (DitalesWindow.ShowDialog() == true)
            {
                var check = Connect.bd.IDProductsProduction.Where(p => p.NameProducts == DitalesWindow.NewDitales.ToString()).Count();
                var objA = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();
                if (check == 0)
                {

                    productsProduction.NameProducts = DitalesWindow.NewDitales.ToString();
                    productsProduction.IDInside = objA + 1;
                    Connect.bd.IDProductsProduction.Add(productsProduction);
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Изделие добавлено добавлен!");
                    MyFrame.Navigate(new AddDitalesProductionPage());
                }
                else MessageBox.Show("Такое изделие уже есть!");
            }
        }
    }
}
