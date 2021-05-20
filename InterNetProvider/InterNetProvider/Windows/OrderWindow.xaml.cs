using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string ProvName
        {
            get
            {
                return Title;
            }
        }
    }

    public partial class ProviderOrder
    {
        public string DateTimeText
        {
            get
            {
                // в принципе то же самое вернет и просто ToString(), но его значение зависит
                // от культурной среды, поэтому лучше задать жестко
                return Date.ToString("dd.MM.yyyy hh:mm:ss");
            }
            set
            {
                // в круглых скобках регуляного выражения те значения, которые попадут в match.Groups
                // точка спецсимвол, поэтому ее экранируем
                // \s - пробел (любой разделитель)
                // \d - цифра
                // модификатор "+" означает что должен быть как минимум один элемент (можно больше)
                Regex regex = new Regex(@"(\d+)\.(\d+)\.(\d+)\s+(\d+):(\d+):(\d+)");
                Match match = regex.Match(value);
                if (match.Success)
                {
                    try
                    {
                            Date = new DateTime(
                            Convert.ToInt32(match.Groups[3].Value),
                            Convert.ToInt32(match.Groups[2].Value),
                            Convert.ToInt32(match.Groups[1].Value),
                            Convert.ToInt32(match.Groups[4].Value),
                            Convert.ToInt32(match.Groups[5].Value),
                            Convert.ToInt32(match.Groups[6].Value)
                            );
                    }
                    catch
                    {
                        MessageBox.Show("Не верный формат даты/времени");
                    }
                }
                else
                {
                    MessageBox.Show("Не верный формат даты/времени");
                }
            }
        }
    }

}


namespace InterNetProvider.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, INotifyPropertyChanged
    {


        public List<Provider> ProviderList { get; set; }
        public ProviderOrder CurrentOrder { get; set; }

        public string WindowName
        {
            get
            {
                return CurrentOrder.Id == 0 ? "Новый заказ" : "Редактирование заказа";
            }
        }

        public OrderWindow(ProviderOrder order)
        {
            InitializeComponent();
            DataContext = this;
            CurrentOrder = order;

            ProviderList = Core.DB.Provider.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (CurrentOrder.Total <= 0)
            {
                MessageBox.Show("Стоимость заказа должна быть больше ноля");
                return;
            }
            
            // если запись новая, то добавляем ее в список
            if (CurrentOrder.Id == 0)
                Core.DB.ProviderOrder.Add(CurrentOrder);

            // сохранение в БД
            try
            {
                Core.DB.SaveChanges();
            }
            catch
            {
            }
            DialogResult = true;
        }

    }
}
