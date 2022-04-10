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

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wAddressAddRed.xaml
    /// </summary>
    public partial class wAddressAddRed : Window
    {
        public RegistrationAddress address;
        public wAddressAddRed(RegistrationAddress address, Applicants app, Users user)
        {
            InitializeComponent();
            this.address = address;
            DataContext = address;
            cmbType.ItemsSource = wAuth.center.RegistrationOfResidence.ToList();
            cmbReg.ItemsSource = wAuth.center.Regions.ToList();
            cmbLoc.ItemsSource = wAuth.center.Localities.ToList();
            if(user.Roles.Name == "Соискатель" ||
                user.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(cmbType.SelectedIndex == -1)
                {
                    error += "Выберите тип прописки\n";
                }
                if(cmbReg.SelectedIndex == -1)
                {
                    error += "Выберите регион\n";
                }
                if(cmbLoc.SelectedIndex == -1)
                {
                    error += "Выберите город\n";
                }
                if(address.Street == null)
                {
                    error += "Введите улицу\n";
                }
                if(address.Flat == null)
                {
                    error += "Введите квартиру\n";
                }
                if(address.PostalCode == null)
                {
                    error += "Введите почтовый индекс\n";
                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(address.ID == 0)
                    {
                        wAuth.center.RegistrationAddress.Add(address);
                    }
                    wAuth.center.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (address.ID == 0)
            {
                MessageBox.Show("Адрес проживания не занесен в базу данных, поэтому удаление невозможно",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        wAuth.center.RegistrationAddress.Remove(address);
                        wAuth.center.SaveChanges();
                        MessageBox.Show("Успешно удалено", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(txtCode.Text, out int code))
            {

            }
            else
            {
                MessageBox.Show("Почтовый индекс состоит из 6 цифр от 0 до 9",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCode.Text = "";
                return;
            }
        }

        private void txtFlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtFlat.Text, out int code))
            {

            }
            else
            {
                MessageBox.Show("Квартира состоит из 2 цифр от 0 до 9",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtFlat.Text = "";
                return;
            }
        }

        private void cmbReg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Regions region = cmbReg.SelectedItem as Regions;
            var items = wAuth.center.Localities.Where(p => p.ID_Region == region.ID).ToList();
            if(items.Count == 0)
            {
                cmbLoc.ItemsSource = wAuth.center.Localities.ToList();
            }
            else
            {
                cmbLoc.ItemsSource = items;
            }
        }
    }
}
