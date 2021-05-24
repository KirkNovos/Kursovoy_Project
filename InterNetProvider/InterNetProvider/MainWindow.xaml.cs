using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace InterNetProvider
{
    public partial class Product
    {
        public Uri ImagePreview
        {
            get
            {
                var imageName = System.IO.Path.Combine(Environment.CurrentDirectory, Image ?? "");
                return System.IO.File.Exists(imageName) ? new Uri(imageName) : new Uri("pack://application:,,,/Images/picture.png");
            }
        }

        public string PriceString
        {
            get
            {
                return Price.ToString("#.##");
            }
        }

        public Boolean MinPrice
        {
            get
            {
                return Price < 2000;
            }
        }

        public float PriceFloat
        {
            get
            {
                return Convert.ToSingle(Price);
            }
        }

        public string ProductString
        {
            get
            {
                return Title ?? "";
            }
        }


    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Product> _ProductList;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Product> ProductList
        {
            get
            {
                

                var FilteredProductList = _ProductList.FindAll(item =>
                item.PriceFloat >= CurrentPriceFilter.Item1 &&
                item.PriceFloat < CurrentPriceFilter.Item2);


                if (SearchFilter != "")
                    FilteredProductList = FilteredProductList.Where(item =>
                        item.Title.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1 ||
                        item.ProductString.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1).ToList();



                if (SortPriceAscending)
                    return FilteredProductList
                        .OrderBy(item => Double.Parse(item.PriceString))
                        .ToList();
                else
                    return FilteredProductList
                .OrderByDescending(item => Double.Parse(item.PriceString))
                .ToList();
            }
            set
            {
                _ProductList = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ProductList = Core.DB.Product.ToList();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void OrdProvidClick(object sender, RoutedEventArgs e)
        {
            var Ord = new Windows.Order();
            Ord.ShowDialog();
        }


        private Boolean _SortPriceAscending = true;
        public Boolean SortPriceAscending
        {
            get { return _SortPriceAscending; }
            set
            {
                _SortPriceAscending = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductList"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ServicesCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredServicesCount"));

                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortPriceAscending = (sender as RadioButton).Tag.ToString() == "1";
        }

        

        private List<Tuple<string, float, float>> FilterByPriceValuesList =
        new List<Tuple<string, float, float>>() {
        Tuple.Create("Все цены", 0f, 100000f),
        Tuple.Create("от 0 до 500", 0f, 500f),
        Tuple.Create("от 500 до 1000", 500f, 1000f),
        Tuple.Create("от 1000 до 1500", 1000f, 1500f),
        Tuple.Create("от 1500 до 2000", 1500f, 2000f),
        Tuple.Create("от 2000 до 2500", 2000f, 2500f)
        };

        public List<string> FilterByPriceNamesList
        {
            get
            {
                return FilterByPriceValuesList
                    .Select(item => item.Item1)
                    .ToList();
            }
        }

        private Tuple<float, float> _CurrentPriceFilter = Tuple.Create(float.MinValue, float.MaxValue);

        public Tuple<float, float> CurrentPriceFilter
        {
            get
            {
                return _CurrentPriceFilter;
            }
            set
            {
                _CurrentPriceFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductList"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ServicesCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredServicesCount"));

                }
            }
        }

        private void PriceFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPriceFilter = Tuple.Create(
                FilterByPriceValuesList[PriceFilterComboBox.SelectedIndex].Item2,
                FilterByPriceValuesList[PriceFilterComboBox.SelectedIndex].Item3
            );
        }


        private string _SearchFilter = "";
        public string SearchFilter
        {
            get { return _SearchFilter; }
            set
            {
                _SearchFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductList"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ServicesCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductCount"));
                }
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
        }

        public int ProductCount
        {
            get
            {
                return _ProductList.Count;
            }

        }
        public int FilteredProductCount
        {
            get
            {
                return ProductList.Count;
            }
        }
    }
}
