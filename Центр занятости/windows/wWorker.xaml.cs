﻿using System;
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
using System.Windows.Threading;
using System.Diagnostics;

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wWorker.xaml
    /// </summary>
    public partial class wWorker : Window
    {
        public Users user;
        public wWorker(Users user)
        {
            InitializeComponent();
            frm.Navigate(new pages.pMain());
            this.user = user;
            Update(user);
        }

        public void Update(Users user)
        {
            var res = wAuth.center.Workers.Where(p => p.ID_User == user.ID);
            Workers worker = null;
            foreach (var v in res)
            {
                worker = v;
            }
            if (worker == null)
            {
                txtFIO.Text = "";
            }
            else
            {
                DataContext = worker;
                txtFIO.Text = $"{worker.FIO}";
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

        private void menuMain_Click(object sender, RoutedEventArgs e)
        {
            frm.Navigate(new pages.pMain());
        }

        private void menuUsers_Click(object sender, RoutedEventArgs e)
        {
            frm.Navigate(new pages.pUsers(user));
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

        private void menuDocs_Click(object sender, RoutedEventArgs e)
        {
            frm.Navigate(new pages.pDocs(user));
        }

        private void menuVacancy_Click(object sender, RoutedEventArgs e)
        {
            frm.Navigate(new pages.pVacancies(user));
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
