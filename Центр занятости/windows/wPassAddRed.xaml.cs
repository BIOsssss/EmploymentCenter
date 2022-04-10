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
    /// Логика взаимодействия для wPassAddRed.xaml
    /// </summary>
    public partial class wPassAddRed : Window
    {
        public Passport passport;
        public wPassAddRed(Passport passport, Applicants app, Users user)
        {
            InitializeComponent();
            this.passport = passport;
            DataContext = passport;
            if(user.Roles.Name == "Инспектор" || user.Roles.Name == "Соискатель")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (passport.ID == 0)
            {
                MessageBox.Show("Паспорт не добавлен в базу данных, поэтому удаление невозможно",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    windows.wAuth.center.Passport.Remove(passport);
                    windows.wAuth.center.SaveChanges();
                    MessageBox.Show("Успешно удалено", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(passport.Number == null)
                {
                    error += "Введите номер паспорта\n";
                }
                if(passport.Issued == null)
                {
                    error += "Введите лицо, которое выдало паспорт\n";
                }
                if(error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(passport.ID == 0)
                    {
                        wAuth.center.Passport.Add(passport);
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

        private void txtSer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(txtSer.Text, out int res))
            {

            }
            else
            {
                MessageBox.Show("Серия состоит из 4 цифр от 0 до 9", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtSer.Text = "";
                return;
            }
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(txtNum.Text, out int res))
            {

            }
            else
            {
                MessageBox.Show("Номер паспорта состоит из цифр от 0 до 9", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtNum.Text = "";
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(passport.ID == 0)
            {
                dateIss.SelectedDate = DateTime.Now;
            }
        }
    }
}
