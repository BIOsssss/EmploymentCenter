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
    /// Логика взаимодействия для pUsers.xaml
    /// </summary>
    public partial class pUsers : Page
    {
        public Users user;
        public pUsers(Users user)
        {
            InitializeComponent();
            this.user = user;
            if(user.Roles.Name == "Инспектор")
            {
                btnWork.Visibility = Visibility.Hidden;
            }
        }

        private void btnWork_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pUsersTable("Инспектор", user));
        }

        private void btnApp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pUsersTable("Соискатель", user));
        }

        private void btnMan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pUsersTable("Менеджер организации", user));
        }
    }
}
