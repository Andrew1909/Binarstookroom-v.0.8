using StockroomBinar.BD;
using StockroomBinar.Class;
using StockroomBinar.DialogWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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
    /// Логика взаимодействия для AddPlasticDitalesPage.xaml
    /// </summary>
    public partial class AddPlasticDitalesPage : Page
    {
        public IDPlasticProducts idPlasticProducts = new IDPlasticProducts();
        public PlasticProducts plasticProducts = new PlasticProducts();
        public ProductsForEngraving forEngraving = new ProductsForEngraving();

        decimal SupportsWidth;
        int CountEngraving;
        string NameDitaliesID = "";
        string ColorNamePlast = "";
        string Manufactr = "";
        string TypePlast = "";
        decimal SupportsWight1=0;
        int IDPlast;

        public AddPlasticDitalesPage()
        {
            InitializeComponent();

            var a = Connect.bd.IDPlasticProducts.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            AddNameDitalies.Items.Add("Выберите изделие");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDPlasticProducts.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProduct.ToString());
                }
            }

             a = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count();
            AddColordNamePlastic.Items.Add("Выберите цвет платика");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.PlasticStor.Where(p => p.IDInsaid == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.PlasticStor.First(p => p.IDInsaid == j);
                    AddColordNamePlastic.Items.Add(a1.ColorName.ToString()+" Тип: "+a1.PlasticType.ToString()+" Производитель: "+a1.Manufacturer.ToString());
                }
            }
            AddColordNamePlastic.SelectedIndex = 0;
            AddNameDitalies.SelectedIndex = 0;


            Plus.Visibility = Visibility.Hidden;
            NextTextSupports.Visibility = Visibility.Hidden;
            EngravingText.Visibility = Visibility.Hidden;
        }

        private void AddNewNameDitales_Click(object sender, RoutedEventArgs e)
        {
            AddNewDitalesWindow DitalesWindow = new AddNewDitalesWindow();
            if (DitalesWindow.ShowDialog() == true)
            {
                var check = Connect.bd.IDPlasticProducts.Where(p=>p.NameProduct== DitalesWindow.NewDitales.ToString()).Count();
                var objA = Connect.bd.IDPlasticProducts.Where(p => p.ID != 0).Count();
                if (check == 0)
                {

                    idPlasticProducts.NameProduct = DitalesWindow.NewDitales.ToString();
                    idPlasticProducts.IDInside = objA + 1;
                    Connect.bd.IDPlasticProducts.Add(idPlasticProducts);
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Изделие добавлено добавлен!");
                    MyFrame.Navigate(new AddPlasticDitalesPage());
                }
                else MessageBox.Show("Такое изделие уже есть!");
            }
        }

        private void AddDitalis_Click(object sender, RoutedEventArgs e)
        {
            if (AddNameDitalies.SelectedIndex == 0 || AddColordNamePlastic.SelectedIndex == 0 || AddWidthDitales.Text == null  || AddTimeDitalis.Text == null || AddCountDitalis.Text == null)
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
                        var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index1);
                        NameDitaliesID = a1.NameProduct;
                    }
                }
                index1 = AddColordNamePlastic.SelectedIndex;

       
                if (AddColordNamePlastic.SelectedIndex == index1)
                {
                    if (index1 > 0)
                    {
                        var a1 = Connect.bd.PlasticStor.First(p => p.IDInsaid == index1);
                        ColorNamePlast = a1.ColorName;
                        Manufactr = a1.Manufacturer;
                        TypePlast = a1.PlasticType;
                        IDPlast = index1;



                    }
                }

                var objA = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == NameDitaliesID && p.ColorName == ColorNamePlast && p.ManufacturerPlasticPrint == Manufactr && p.TypePlasticPrint==TypePlast).Count(); //проверяем, есть ли в БД уже эта деталь
                var objC = Connect.bd.PlasticStor.First(p => p.IDInsaid == IDPlast);
                decimal WeightSum = (decimal.Parse(SupportsWight1.ToString()) + decimal.Parse(AddWidthDitales.Text)) * decimal.Parse(AddCountDitalis.Text);
                if (WeightSum <= objC.Weight)//хватит ли платика на складе
                {
                    objC.Weight = objC.Weight - WeightSum;
                    Connect.bd.SaveChanges();
                    if (objA != 0)
                    {
                        //есть ли такая деталь с той же датой производства

                        var objB = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == NameDitaliesID && p.ColorName == ColorNamePlast && p.ManufacturerPlasticPrint == Manufactr && p.TypePlasticPrint == TypePlast);


                        if (CountEngraving != 0)
                        {
                            EngraveringFunc(objB.ID, 1);
                            objB.ProductWeight = (decimal.Parse(AddWidthDitales.Text));
                            objB.SupportsWeight = SupportsWight1;
                            objB.CountOnStoock = int.Parse(AddCountDitalis.Text) + objB.CountOnStoock;
                            objB.EngravingStatus = objB.EngravingStatus + (int.Parse(AddCountDitalis.Text) - CountEngraving);
                            Connect.bd.SaveChanges();
                            MessageBox.Show("Детали добавлены к существующей записи!");
                            MyFrame.Navigate(new PlasticDitalesPage());
                        }
                        else
                        {
                            objB.ProductWeight = decimal.Parse(AddWidthDitales.Text);
                            objB.SupportsWeight = SupportsWight1;
                            objB.CountOnStoock = int.Parse(AddCountDitalis.Text) + objB.CountOnStoock;
                            objB.EngravingStatus = objB.CountOnStoock;
                            Connect.bd.SaveChanges();
                            MessageBox.Show("Детали добавлены к существующей записи!");
                            MyFrame.Navigate(new PlasticDitalesPage());
                        }
                    }
                    else
                    {
                        AddNewDeitales();
                    }
                }
                else MessageBox.Show("Нехватает платика!");
            }
        }

        void AddNewDeitales()
        {
            
            plasticProducts.ProductWeight = decimal.Parse(AddWidthDitales.Text);
            if (SupportsWight1 == 0) plasticProducts.SupportsWeight = 0;
            else plasticProducts.SupportsWeight = SupportsWight1;
            plasticProducts.CountOnStoock = int.Parse(AddCountDitalis.Text);
            if (CountEngraving != 0) plasticProducts.EngravingStatus = int.Parse(AddCountDitalis.Text) - CountEngraving;
            else plasticProducts.EngravingStatus = int.Parse(AddCountDitalis.Text); ;
            plasticProducts.TimePrint = AddTimeDitalis.Text;
            plasticProducts.ProductTypeID = NameDitaliesID;
            plasticProducts.ColorName = ColorNamePlast;
            plasticProducts.TypePlasticPrint = TypePlast;
            plasticProducts.ManufacturerPlasticPrint = Manufactr;
            Connect.bd.PlasticProducts.Add(plasticProducts);
            Connect.bd.SaveChanges();
            MessageBox.Show("Деталь добавлена!");
            if (CountEngraving != 0)
            {
                var objB = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == NameDitaliesID && p.ColorName == ColorNamePlast && p.ManufacturerPlasticPrint == Manufactr && p.TypePlasticPrint == TypePlast);
                EngraveringFunc(objB.ID, 1);//отправляем чать на гравировку
            }
            MyFrame.Navigate(new PlasticDitalesPage());
        }

        void EngraveringFunc(int id, int TableID)
        {
            var objA = Connect.bd.ProductsForEngraving.Where(p => p.IDInside == id && p.TypeDitalesID == TableID).Count();
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
                forEngraving.TypeDitalesID = 1;//указывем из какой таблицы пришла деталь
                Connect.bd.ProductsForEngraving.Add(forEngraving);
                Connect.bd.SaveChanges();
            }   
        }

        private void AddColordNamePlastic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           



        }

        private void Suports_Checked(object sender, RoutedEventArgs e)
        {
            SupportsAddWindow SupportsWindow = new SupportsAddWindow();
            if (SupportsWindow.ShowDialog() == true)
            {
                Plus.Visibility = Visibility.Visible;
                NextTextSupports.Visibility = Visibility.Visible;
                SupportsWight.Text = SupportsWindow.Supports.ToString();
                SupportsWight1= decimal.Parse(SupportsWindow.Supports.ToString());
            }
            else
            {
                Suports.IsChecked = false;
            }
        }

        private void Suports_Unchecked(object sender, RoutedEventArgs e)
        {
            SupportsWight.Text = string.Empty;
            Plus.Visibility = Visibility.Hidden;
            NextTextSupports.Visibility = Visibility.Hidden;
            SupportsWight1 = 0;
        }

        private void AddNameDitalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index2 = AddNameDitalies.SelectedIndex;
          
            if (AddNameDitalies.SelectedIndex == index2)
            {
                if (index2 > 0)
                {
                    var objB = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index2);
                    var objA = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == objB.NameProduct).Count();
                    if (objA != 0)
                    {
                        var ObjB = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == objB.NameProduct);
                       
                        AddWidthDitales.Text = ObjB.ProductWeight.ToString();
                       // AddTimeDitalis.Text = ObjB.TimePrint.ToString();

                        if (ObjB.SupportsWeight != 0)
                        {       
                            SupportsWight1 = decimal.Parse(ObjB.SupportsWeight.ToString());
                            Plus.Visibility = Visibility.Visible;
                            NextTextSupports.Visibility = Visibility.Visible;
                            SupportsWight.Text = SupportsWight1.ToString();
                        }
                        else
                        {
                            Plus.Visibility = Visibility.Hidden;
                            NextTextSupports.Visibility = Visibility.Hidden;
                            SupportsWight.Text = string.Empty;
                        }
                        
                    }
                }
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
    }
}
