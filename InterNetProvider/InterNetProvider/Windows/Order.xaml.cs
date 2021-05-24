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
using System.Windows.Shapes;

namespace InterNetProvider
{
    public partial class Provider
    {
        public Uri ImagePre
        {
            get
            {
                var imageName = System.IO.Path.Combine(Environment.CurrentDirectory, Logo ?? "");
                return System.IO.File.Exists(imageName) ? new Uri(imageName) : new Uri("pack://application:,,,/Images/picture.png");
            }
        }
    }

    public partial class ProviderOrder
    {
        public string TotalString
        {
            get
            {
                return Total.ToString("#.##");
            }
        }
    }
}


namespace InterNetProvider.Windows
{

    public partial class Order : Window, INotifyPropertyChanged
    {

        private List<ProviderOrder> _OrderList;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ProviderOrder> OrderList
        {
            get
            {
                return _OrderList;
            }
            set
            {
                _OrderList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderList"));
                }
            }
        }

        public Order()
        {
            InitializeComponent();
            this.DataContext = this;
            OrderList = Core.DB.ProviderOrder.ToList();
        }


        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var NewOrder = new ProviderOrder();

            var NewOrderWindow = new OrderWindow(NewOrder);
            if ((bool)NewOrderWindow.ShowDialog())
            {
                OrderList = Core.DB.ProviderOrder.ToList();
                PropertyChanged(this, new PropertyChangedEventArgs("FilteredOrderCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("OrderCount"));
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            var SelectedOrder = ProductListView.SelectedItem as ProviderOrder;
            var EditOrderWindow = new OrderWindow(SelectedOrder);
            if ((bool)EditOrderWindow.ShowDialog())
            {
                PropertyChanged(this, new PropertyChangedEventArgs("OrderList"));
            }
        }

        private void DelOrd_Click(object sender, RoutedEventArgs e)
        {
            var item = ProductListView.SelectedItem as ProviderOrder;
            Core.DB.ProviderOrder.Remove(item);
            Core.DB.SaveChanges();
            OrderList = Core.DB.ProviderOrder.ToList();
        }


    }

    
}