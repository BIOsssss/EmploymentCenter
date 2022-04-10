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
    /// Логика взаимодействия для wManagersAddRed.xaml
    /// </summary>
    public partial class wManagersAddRed : Window
    {
        public ManagerOrg manager;
        public Users user;
        public Users user_main;
        public wManagersAddRed(ManagerOrg manager, Users user, Users user_main)
        {
            InitializeComponent();
            this.user = user;
            this.manager = manager;
            this.user_main = user_main;
            DataContext = manager;
            this.manager.ID_User = user.ID;
            cmbGender.ItemsSource = wAuth.center.Genders.ToList();
            if(user_main.Roles.Name == "Менеджер организации" ||
                  user_main.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddOrg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (manager.ID == 0)
                {
                    MessageBox.Show("Менеджер не зарегистрирован, поэтому добавление организации невозможно",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool res = false;
                    Organization org = null;
                    foreach (var organization in windows.wAuth.center.Organization)
                    {
                        if (manager.ID == organization.ID_Manager) 
                        { 
                            res = true;
                            org = organization;
                        }
                    }
                    if (res)
                    {
                        wOrgAddRed red = new wOrgAddRed(org, manager, user_main);
                        red.ShowDialog();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого менеджера нету информации об организации\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            org = new Organization();
                            wOrgAddRed add = new wOrgAddRed(org, manager, user_main);
                            add.ShowDialog();
                            txtOrg.Text = org.AbbreviatedName;
                            tlOrg.Content = txtOrg.Text;
                        }
                        else
                        {
                            return;
                        }
                    }
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
            if(manager.ID == 0)
            {
                dateBirthday.SelectedDate = DateTime.Now;
            }
            else
            {
                dateBirthday.SelectedDate = manager.Birthday;
                tlOrg.Content = txtOrg.Text;
            }
        }

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                TimeSpan dr = DateTime.Today - manager.Birthday;
                int age = dr.Days / 365;
                if (manager.LastName == null)
                {
                    error += "Введите фамилию\n";
                }
                if (manager.FirstName == null)
                {
                    error += "Введите имя\n";
                }
                if (age < 18)
                {
                    error += "Возраст менеджера организации должен быть не менее 18 лет\n";
                    dateBirthday.SelectedDate = DateTime.Today;
                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (manager.ID == 0)
                    {
                        windows.wAuth.center.ManagerOrg.Add(manager);
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if(manager.ID == 0)
            {
                MessageBox.Show("Менеджер не зарегистрирован, поэтому удаление невозможно",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    windows.wAuth.center.ManagerOrg.Remove(manager);
                    windows.wAuth.center.SaveChanges();
                    MessageBox.Show("Успешно удалено", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }
    }
}
