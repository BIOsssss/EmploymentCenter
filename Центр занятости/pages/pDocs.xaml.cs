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
    /// Логика взаимодействия для pDocs.xaml
    /// </summary>
    public partial class pDocs : Page
    {
        public Users user;
        public pDocs(Users user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pRef(user));
        }

        private void btnApp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pZayav(user));
        }

        private void btnStip_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.pStipend(user));
        }
    }
}
