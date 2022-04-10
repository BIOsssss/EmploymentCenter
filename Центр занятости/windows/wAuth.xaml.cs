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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class wAuth : Window
    {
        public static CenterEntities center = new CenterEntities();
        public bool check = false;
        public bool close = false;

        public wAuth()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text != "" && (txtPas.Text != "" || pasBx.Password != ""))
                {
                    foreach (var item in center.Users)
                    {
                        if (item.NameUser == txtName.Text &&
                            (item.Password == txtPas.Text ||
                            item.Password == pasBx.Password))
                        {
                            if (item.Roles.Name == "Администратор")
                            {
                                wAdmin admin = new wAdmin(item);
                                admin.Show();
                                Close();
                                close = true;
                            }
                            if (item.Roles.Name == "Инспектор")
                            {
                                wWorker worker = new wWorker(item);
                                worker.Show();
                                Close();
                                close = true;
                            }
                            if (item.Roles.Name == "Менеджер организации")
                            {
                                wManager manager = new wManager(item);
                                manager.Show();
                                Close();
                                close = true;
                            }
                            if (item.Roles.Name == "Соискатель")
                            {
                                wApplicant applicant = new wApplicant(item);
                                applicant.Show();
                                Close();
                                close = true;
                            }
                        }
                    }
                    if (close)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Неправильное имя пользователя или пароль\n" +
                            "Повторите ввод", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите имя пользователя и пароль",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (check)
            {
                pasBx.Visibility = Visibility.Visible;
                txtPas.Visibility = Visibility.Hidden;
                pasBx.Password = txtPas.Text;
                imgCheck.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.EyeCheckOutline;
                check = false;
            }
            else
            {
                pasBx.Visibility = Visibility.Hidden;
                txtPas.Visibility = Visibility.Visible;
                txtPas.Text = pasBx.Password;
                imgCheck.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.EyeCheck;
                check = true;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    if (txtName.Text != "" && (txtPas.Text != "" || pasBx.Password != ""))
                    {
                        foreach (var item in center.Users)
                        {
                            if (item.NameUser == txtName.Text &&
                                (item.Password == txtPas.Text ||
                                item.Password == pasBx.Password))
                            {
                                if (item.Roles.Name == "Администратор")
                                {
                                    wAdmin admin = new wAdmin(item);
                                    admin.Show();
                                    Close();
                                    close = true;
                                }
                                if (item.Roles.Name == "Инспектор")
                                {
                                    wWorker worker = new wWorker(item);
                                    worker.Show();
                                    Close();
                                    close = true;
                                }
                                if(item.Roles.Name == "Менеджер организации")
                                {
                                    wManager manager = new wManager(item);
                                    manager.Show();
                                    Close();
                                    close = true;
                                }
                                if(item.Roles.Name == "Соискатель")
                                {
                                    wApplicant applicant = new wApplicant(item);
                                    applicant.Show();
                                    Close();
                                    close=true;
                                }
                            }
                        }
                        if (close)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Неправильное имя пользователя или пароль\n" +
                                "Повторите ввод", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите имя пользователя и пароль",
                            "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
