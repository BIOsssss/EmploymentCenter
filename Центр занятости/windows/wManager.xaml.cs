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
using System.Windows.Threading;
using System.Diagnostics;

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wManager.xaml
    /// </summary>
    public partial class wManager : Window
    {
        public Users user;

        public List<MenuItem> menuItems = new List<MenuItem>();

        public wManager(Users user)
        {
            InitializeComponent();
            this.user = user;
            RoutedEventArgs e = new RoutedEventArgs();
            object sender = menuMain;
            menuItems.Add(menuMain);
            menuItems.Add(menuVacancy);
            menuMain_Click(sender, e);
            Update(user);
        }

        public void Update(Users user)
        {
            var res = wAuth.center.ManagerOrg.Where(p => p.ID_User == user.ID);
            ManagerOrg manager = null;
            foreach (var v in res)
            {
                manager = v;
            }
            if (manager == null)
            {
                txtFIO.Text = "";
            }
            else
            {
                DataContext = manager;
                txtFIO.Text = $"{manager.FIO}";
            }
        }

        private void wind_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToLongDateString();
            string time = DateTime.Now.ToLongTimeString();
            txtToday.Text = $"{date} {time}";
        }

        private void txtAuth_Click(object sender, RoutedEventArgs e)
        {
            windows.wAuth w = new windows.wAuth();
            w.Show();
            this.Close();
        }

        private void menuUpdate()
        {
            BrushConverter bc = new BrushConverter();
            foreach (var item in menuItems)
            {
                item.Background = (Brush)bc.ConvertFromString("#c6fbff");
                item.BorderBrush = (Brush)bc.ConvertFromString("#c6fbff");
            }
        }

        private void menuMain_Click(object sender, RoutedEventArgs e)
        {
            menuUpdate();
            frm.Navigate(new pages.pMain());
            foreach (var item in menuItems)
            {
                if (item.Name == menuMain.Name)
                {
                    item.Background = Brushes.White;
                    item.BorderBrush = Brushes.White;
                    break;
                }
            }
        }

        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            Process info = new Process();
            info.StartInfo.ErrorDialog = true;
            info.StartInfo.FileName = @"center.chm";
            info.Start();
        }

        private void txtProfile_Click(object sender, RoutedEventArgs e)
        {
            wUsersAddRed red = new wUsersAddRed(user, user);
            red.ShowDialog();
            if(red.DialogResult == true)
            {
                Update(red.user);
            }
        }

        private void menuVacancy_Click(object sender, RoutedEventArgs e)
        {
            menuUpdate();
            frm.Navigate(new pages.pVacancies(user));
            foreach (var item in menuItems)
            {
                if (item.Name == menuVacancy.Name)
                {
                    item.Background = Brushes.White;
                    item.BorderBrush = Brushes.White;
                    break;
                }
            }
        }

        private void wind_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1)
            {
                Process info = new Process();
                info.StartInfo.ErrorDialog = true;
                info.StartInfo.FileName = @"center.chm";
                info.Start();
            }
        }
    }
}
