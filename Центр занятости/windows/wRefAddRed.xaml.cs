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
    /// Логика взаимодействия для wRefAddRed.xaml
    /// </summary>
    public partial class wRefAddRed : Window
    {
        public Users user;
        public ReferralToWork referral;

        public wRefAddRed(ReferralToWork referral, Users user)
        {
            InitializeComponent();
            this.referral = referral;
            this.user = user;
            cmbWork.ItemsSource = wAuth.center.Workers.ToList();
            DataContext = referral;

            if(user.Roles.Name == "Инспектор")
            {
                var worker = wAuth.center.Workers.Where(p => p.ID_User == user.ID).ToList();
                cmbWork.ItemsSource = worker;
                cmbWork.SelectedIndex = 0;
            }
        }

        MediaPlayer mp = new MediaPlayer();
        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(cmbApp.SelectedIndex == -1)
                {
                    error += "Выберите соискателя\n";
                }
                if(cmbOrg.SelectedIndex == -1)
                {
                    error += "Выберите работодателя\n";
                }
                if(cmbVac.SelectedIndex == -1)
                {
                    error += "Выберите вакансию\n";
                }
                if(cmbWork.SelectedIndex == -1)
                {
                    error += "Выберите инспектора\n";
                }
                if(error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(referral.ID == 0)
                    {
                        wAuth.center.ReferralToWork.Add(referral);
                    }
                    wAuth.center.SaveChanges();
                    var item = cmbVac.SelectedItem as Vacancy;
                    item.Valid = false;
                    wAuth.center.SaveChanges();
                    string s = Environment.CurrentDirectory;
                    if (s.Contains("Debug"))
                    {
                        s = s.Remove(s.Length - 10, 10);
                    }
                    if (s.Contains("EmploymentCenter"))
                    {

                    }
                    mp.Open(new Uri($@"{s}" + @"\" + "music" + @"\" + "pechat.mp3"));
                    mp.Volume = 0.15;
                    mp.Play();
                    MessageBox.Show("Данные сохранены",
                        "Внимание", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }

        private void dateSt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateF.SelectedDate = dateSt.SelectedDate.Value.AddDays(3);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var cmbA = wAuth.center.Applicants.Where(p => p.Registered == true).ToList();
            List<Applicants> lst = new List<Applicants>();
            foreach (var item in cmbA)
            {
                if (item.ApplicationOfUnemployed.Count == 1)
                {
                    lst.Add(item);
                }
            }
            cmbApp.ItemsSource = lst;

            var cmbB = wAuth.center.Vacancy.Where(p => p.Valid == true).ToList();
            List<Organization> list = new List<Organization>();
            foreach (var item in cmbB)
            {
                list.Add(item.Organization);
            }
            cmbOrg.ItemsSource = list.Distinct();

            cmbVac.ItemsSource = cmbB.OrderBy(p => p.Header);

            if (referral.ID == 0)
            {
                dateSt.SelectedDate = DateTime.Now;
                dateF.SelectedDate = dateSt.SelectedDate.Value.AddDays(3);
                referral.Hired = true;
                cbHired.IsChecked = true;
            }
            else
            {
                if (dateF.SelectedDate < DateTime.Now)
                {
                    cbHired.IsChecked = false;
                    referral.Hired = false;
                    cbHired.IsEnabled = false;
                }

            }
            if (user.Roles.Name == "Инспектор")
            {
                var worker = wAuth.center.Workers.Where(p => p.ID_User == user.ID).ToList();
                cmbWork.ItemsSource = worker;
                cmbWork.SelectedIndex = 0;
                cmbWork.IsEnabled = false;
            }
        }

        private void cmbVac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = cmbVac.SelectedItem as Vacancy;
            cmbOrg.SelectedIndex = -1;
            bool itog = false;
            if(item == null)
            {
                return;
            }
            else
            {
                foreach (var i in wAuth.center.Vacancy)
                {
                    if (i.Header == item.Header)
                    {
                        itog = true;
                        break;
                    }
                    else
                    {
                        itog = false;
                        continue;
                    }
                }
                if (itog)
                {
                    var res = wAuth.center.Organization.Where(p => p.ID == item.ID_Org).ToList();
                    foreach (var item2 in res)
                    {
                        cmbOrg.SelectedItem = item2;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mp.Close();
        }
    }
}
