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
    /// Логика взаимодействия для wWorkersAddRed.xaml
    /// </summary>
    public partial class wWorkersAddRed : Window
    {
        public Workers worker;

        public wWorkersAddRed(Workers worker, Users user, Users user_main)
        {
            InitializeComponent();
            this.worker = worker;
            DataContext = worker;
            this.worker.ID_User = user.ID;
            cmbGender.ItemsSource = windows.wAuth.center.Genders.ToList();
            cmbCenter.ItemsSource = windows.wAuth.center.Centers.ToList();
            if(user_main.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                TimeSpan dr = DateTime.Today - worker.Birthday;
                int age = dr.Days / 365;
                if(worker.LastName == null)
                {
                    error += "Введите фамилию\n";
                }
                if(worker.FirstName == null)
                {
                    error += "Введите имя\n";
                }
                if(worker.Wages == 0)
                {
                    error += "Введите зарплату\n";
                }
                if(worker.Wages <= 0)
                {
                    error += "Зарплата не может быть меньше или равна нулю\n";
                }
                if(age < 18)
                {
                    error += "Возраст инспектора должен быть не менее 18 лет\n";
                    dateBirthday.SelectedDate = DateTime.Today;
                }
                if(error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(worker.ID == 0)
                    {
                        windows.wAuth.center.Workers.Add(worker);
                    }
                    windows.wAuth.center.SaveChanges();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (worker.ID == 0)
            {
                dateBirthday.SelectedDate = DateTime.Now;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (worker.ID == 0)
            {
                MessageBox.Show("Менеджер не зарегистрирован, поэтому удаление невозможно",
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
                        wAuth.center.Workers.Remove(worker);
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
    }
}
