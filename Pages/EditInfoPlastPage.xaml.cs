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
    /// Логика взаимодействия для EditInfoPlastPage.xaml
    /// </summary>
    public partial class EditInfoPlastPage : Page
    {
        public PlasticStor plasticStor = new PlasticStor();
        public ColorPlastic colorPlastic = new ColorPlastic();
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();

        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string OldNameColorPlast;//запоминаем название цвета
        public EditInfoPlastPage(PlasticStor item)
        {
            InitializeComponent();
            var b = Connect.bd.PlasticType.Where(p => p.ID != 0).Count();
            AddTypePlastic.Items.Add("Выберите тип пластика");
            for (int j = 1; j <= int.Parse(b.ToString()); j++)
            {
                var b1 = Connect.bd.PlasticType.First(p => p.ID == j);
                AddTypePlastic.Items.Add(b1.NameType.ToString());
            }
            var index = Connect.bd.PlasticType.First(p => p.NameType==item.PlasticType);
            AddTypePlastic.SelectedIndex = index.ID;
            AddColordNamePlastic.Text = item.ColorName;
            AddCoilsPlastic.Text = item.NumberСoils.ToString();
            AddWightPlastic.Text = item.Weight.ToString();
            AddManufactPlastic.Text = item.Manufacturer;
            AddNotesPlastic.Text = item.Notes;
            OldNameColorPlast = item.ColorName;
            TypeNamePlast = item.PlasticType;
            plasticStor = item;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var objA = Connect.bd.PlasticStor.First(p => p.ColorName == OldNameColorPlast && p.PlasticType == TypeNamePlast &&p.Manufacturer==AddManufactPlastic.Text);
            if (objA != null)
            {
                if (MessageBox.Show($"Вы действительно хотите списать пластик: {objA.ColorName} ?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var objY = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == AddColordNamePlastic.Text && p.PlasticTypeRecucling == objA.PlasticType && p.ManufacturerRecucling==AddManufactPlastic.Text).Count();
                    var objY1 = Connect.bd.PlasticStor.First(p => p.ColorName == AddColordNamePlastic.Text && p.PlasticType == objA.PlasticType && p.Manufacturer==AddManufactPlastic.Text);
                    if (objY != 0)
                        {
                            var objY2 = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == AddColordNamePlastic.Text && p.PlasticTypeRecucling == objA.PlasticType && p.ManufacturerRecucling == AddManufactPlastic.Text);
                            recyclingPlastic = objY2;
                            recyclingPlastic.WeightRecucling = recyclingPlastic.WeightRecucling + objY1.Weight;
                            recyclingPlastic.PlasticStatus = 0;
                            Connect.bd.SaveChanges();
                            Connect.bd.PlasticStor.Remove(objY1);
                            Connect.bd.SaveChanges();
                            MyFrame.Navigate(new PlasticStorage());
                    }
                        else
                        {
                            recyclingPlastic.ID = objY1.ID;
                            recyclingPlastic.ColorNameRecucling = objY1.ColorName;
                            recyclingPlastic.PlasticTypeRecucling = objY1.PlasticType;
                            recyclingPlastic.ManufacturerRecucling = objY1.Manufacturer;
                            recyclingPlastic.WeightRecucling = objY1.Weight;
                            recyclingPlastic.PlasticStatus = 0;
                            Connect.bd.RecyclingPlastic.Add(recyclingPlastic);
                            Connect.bd.SaveChanges();
                            Connect.bd.PlasticStor.Remove(objY1);
                            Connect.bd.SaveChanges();
                            MyFrame.Navigate(new PlasticStorage());
                    }
                }
            }
        }

        private void AddCoilsPlastic_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AddDefective_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AddDefective_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            plasticStor.ColorName = AddColordNamePlastic.Text;
            int index2 = AddTypePlastic.SelectedIndex;
            if (AddTypePlastic.SelectedIndex == index2)
            {
                if (index2 > 0)
                {
                    var a1 = Connect.bd.PlasticType.First(p => p.ID == index2);
                    plasticStor.PlasticType = a1.NameType;
                }
            }
            plasticStor.Weight = decimal.Parse(AddWightPlastic.Text);
            plasticStor.NumberСoils = int.Parse(AddCoilsPlastic.Text);
            plasticStor.Manufacturer = AddManufactPlastic.Text;
            plasticStor.Notes = AddNotesPlastic.Text;
            Connect.bd.SaveChanges();
            var c = Connect.bd.ColorPlastic.First(p => p.NameColor == OldNameColorPlast);
            c.NameColor= AddColordNamePlastic.Text;
            Connect.bd.SaveChanges();
            MessageBox.Show("Изменения сохранены!");
            OldNameColorPlast = "";
            MyFrame.Navigate(new PlasticStorage());
        }
    }
}
