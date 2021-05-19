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

namespace InterNetProvider.Windows
{
    /// <summary>
    /// Логика взаимодействия для ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window, INotifyPropertyChanged
    {
        public ServiceWindow(Product product)
        {
            InitializeComponent();
            CurrentProduct = product;
            this.DataContext = this;
        }
        public Product CurrentProduct { get; set; }
        public string WindowName
        {
            get
            {
                return CurrentProduct.Id == 0 ? "Новая услуга" : "Редактирование услуги";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
