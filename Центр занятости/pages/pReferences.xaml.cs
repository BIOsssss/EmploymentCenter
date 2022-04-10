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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Центр_занятости.pages
{
    /// <summary>
    /// Логика взаимодействия для pReferences.xaml
    /// </summary>
    public partial class pReferences : Page
    {
        public Users user;
        public pReferences(Users user)
        {
            InitializeComponent();
            this.user = user;
            Update();
            if (user.Roles.Name == "Соискатель")
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDel.Visibility = Visibility.Hidden;
                btnRed.Visibility = Visibility.Hidden;
            }
        }

        private void Update()
        {
            grRef.ItemsSource = windows.wAuth.center.ReferralToWork.ToList();
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reff = grRef.SelectedItem as ReferralToWork;
                if (reff != null)
                {
                    windows.wRefAddRed red = new windows.wRefAddRed(reff, user);
                    red.ShowDialog();
                    Update();
                }
                else
                {
                    MessageBox.Show("Выберите элемент, прежде чем редактировать", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ReferralToWork reff = new ReferralToWork();
            windows.wRefAddRed add = new windows.wRefAddRed(reff, user);
            add.ShowDialog();
            Update();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grRef.SelectedItem as ReferralToWork;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.ReferralToWork.Remove(item);
                        windows.wAuth.center.SaveChanges();
                        Update();
                        MessageBox.Show("Успешно удалено", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите элемент, прежде чем удалять", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
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
