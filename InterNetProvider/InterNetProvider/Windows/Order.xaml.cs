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
using System.Windows.Shapes;

namespace InterNetProvider.Windows
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        private List<ProviderOrder> _OrderList;
        public List<ProviderOrder> OrderList
        {
            get
            {
                return _OrderList;
            }
            set
            {
                _OrderList = value;
            }
        }

        public Order()
        {
            InitializeComponent();
            this.DataContext = this;
            OrderList = Core.DB.ProviderOrder.ToList();
        }
    }
}
