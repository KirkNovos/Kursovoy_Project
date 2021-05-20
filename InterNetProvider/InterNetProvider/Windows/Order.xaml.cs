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
}


namespace InterNetProvider.Windows
{

    

    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
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
            // создаем новую услугу
            var NewOrder = new ProviderOrder();

            var NewOrderWindow = new OrderWindow(NewOrder);
            if ((bool)NewOrderWindow.ShowDialog())
            {
                // список услуг нужно перечитать с сервера
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
                // при успешном завершении не забываем перерисовать список услуг
                PropertyChanged(this, new PropertyChangedEventArgs("OrderList"));
                // и еще счетчики - их добавьте сами
            }
        }

        private void DelOrd_Click(object sender, RoutedEventArgs e)
        {
            // у DataGrid-a есть свойство SelectedItem - его приводим к типу Service
            var item = ProductListView.SelectedItem as ProviderOrder;

            // метод Remove нужно завернуть в конструкцию try..catch, на случай, если 
            // база спроектирована криво и нет каскадного удаления - это сделайте сами
            Core.DB.ProviderOrder.Remove(item);

            // сохраняем изменения
            Core.DB.SaveChanges();

            // перечитываем изменившийся список, не забывая в сеттере вызвать PropertyChanged
            OrderList = Core.DB.ProviderOrder.ToList();
        }


    }

    
}