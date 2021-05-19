using Microsoft.Win32;
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
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, INotifyPropertyChanged
    {
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
            CurrentOrder = order;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /*
        private void GetImageButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();
            // задаем фильтр для выбираемых файлов
            // до символа "|" идет произвольный текст, а после него шаблоны файлов раздеренные точкой с запятой
            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg)|*.png;*.jpg";
            // чтобы не искать по всему диску задаем начальный каталог
            GetImageDialog.InitialDirectory = Environment.CurrentDirectory;
            if (GetImageDialog.ShowDialog() == true)
            {
                // перед присвоением пути к картинке обрезаем начало строки, т.к. диалог возвращает полный путь
                // (тут конечно еще надо проверить есть ли в начале Environment.CurrentDirectory)
                CurrentOrder.Logo = GetImageDialog.FileName.Substring(Environment.CurrentDirectory.Length + 1);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentOrder"));
                }
            }
        }
        */
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
