using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AutoService.ApplicationData;
using AutoService.SystemAppClass;

namespace AutoService.PageClients
{
    /// <summary>
    /// Логика взаимодействия для PageListClients.xaml
    /// </summary>
    public partial class PageListClients : Page
    {
        List<ApplicationData.Client> CoutFilter = new List<ApplicationData.Client>(AppConnect.modelOdb.Client);


        /// <summary>
        /// Конструктор класса PageListClients
        /// </summary>
        public PageListClients()
        {
            InitializeComponent();
            lvClients.ItemsSource = CoutFilter;
        }

        /// <summary>
        /// Метод для обновления данных в таблице Клиент
        /// </summary>
        private void Filter()
        {
            if (lvClients != null)
            {
                var FilterFio = AppConnect.modelOdb.Client.ToList();
                if (TbSearch.Text != "Поиск по ФИО")
                {
                    FilterFio = FilterFio.Where(x => x.FIO.Contains(TbSearch.Text)).ToList();
                    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^Не работает^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                }
                if (ListGenderBox != null)
                {
                    switch (ListGenderBox.SelectedIndex)
                    {
                        case 0:
                            FilterFio = FilterFio.Where(x => x.IdGender.Equals("м")).ToList();
                            break;
                        case 1:
                            FilterFio = FilterFio.Where(x => x.IdGender.Equals("ж")).ToList();
                            break;
                    }
                }
                if (CheckDateBird.IsChecked == true)
                {
                    FilterFio = FilterFio.Where(x => x.BirhDate.Date.Month == DateTime.Now.Month).ToList();
                }
                lvClients.ItemsSource = FilterFio;
            }
        }

        /// <summary>
        /// Метод события изменения текста в поле для поиска.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Метод события фокусирования на тексте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbSearch.Text == "Поиск по ФИО")
            {
                TbSearch.Text = "";
            }
        }

        /// <summary>
        /// метод события расфокусирование текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_LostFocus(object sender, RoutedEventArgs e)
        {

            if (TbSearch.Text == "")
            {
                TbSearch.Text = "Поиск по ФИО";
            }
        }

        /// <summary>
        /// Метод события изменения выборки 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListGenderBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }


        //-------------------------------------------Не работает--------------------------------------------------------------

        /// <summary>
        /// Метод события чека дня рождения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDateBird_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void CheckDateBird_Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void lvClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvClients.SelectedItems.Count > 0)
            {
                Client ClientEdit = lvClients.SelectedItem as Client;
                AppFrame.SelectedFrame.Navigate(new PageEditClient(ClientEdit));
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageEditClient());
        }

    }
}
