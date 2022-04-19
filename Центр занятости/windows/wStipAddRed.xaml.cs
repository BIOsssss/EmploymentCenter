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
    /// Логика взаимодействия для wStipAddRed.xaml
    /// </summary>
    public partial class wStipAddRed : Window
    {
        public Stipend stipend;
        public Users user;
        public wStipAddRed(Stipend stipend, Users user)
        {
            InitializeComponent();
            this.stipend = stipend;
            this.user = user;
            var cmbA = wAuth.center.Applicants.Where(p => p.Registered == true).ToList();
            List<Applicants> lst = new List<Applicants>();
            foreach(var item in wAuth.center.ApplicationOfUnemployed.ToList())
            {
                foreach(var it in cmbA)
                {
                    if(it.ID == item.ID_Applicant
                        && item.StatusUnemployed.Name == "Назначено пособие")
                    {
                        lst.Add(it);
                    }
                }
            }
            cmbApp.ItemsSource = lst.Distinct();
            cmbWork.ItemsSource = wAuth.center.Workers.ToList();
            stipend.DateStipend = DateTime.Now;
            DataContext = stipend;
        }

        MediaPlayer mp = new MediaPlayer();

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(cmbApp.SelectedIndex == -1)
                {
                    error += "Выберите соискателя\n";
                }
                else
                {
                    var app = cmbApp.SelectedItem as Applicants;
                    if(app.PaymentAccount == null || app.PaymentAcc == null
                        || app.PaymentAcc == 0)
                    {
                        error += "У соискателя нет информации о рассчетном счете\n";
                    }
                }
                if(cmbWork.SelectedIndex == -1)
                {
                    error += "Выберите инспектора\n";
                }
                if(stipend.DateStart.ToShortDateString() == "01.01.0001")
                {
                    error += "Выберите дату начала выплаты\n";
                }
                else if (stipend.DateFinish.ToShortDateString() == "01.01.0001")
                {
                    error += "Выберите дату завершения выплаты\n";
                }
                else
                {
                    TimeSpan ts = stipend.DateFinish - stipend.DateStart;
                    int m = ts.Days / 28;
                    if(m <= 0)
                    {
                        error += "Разница между датой завершения выплаты и датой начала выплаты должна составлять минимум 1 месяц\n";
                        dtSt.SelectedDate = DateTime.Now;
                        dtF.SelectedDate = dtSt.SelectedDate.Value.AddDays(30);
                    }
                }
                if(stipend.Payment == 0)
                {
                    error += "Введите сумму пособия\n";
                }
                else if (stipend.Payment > 0 && (stipend.Payment < 1500 || stipend.Payment > 12130))
                {
                    error += "Сумма пособия должна быть больше или равно 1500 рублей и меньше или равно 12 130 рублей\n";
                }
                else
                {

                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(stipend.ID == 0)
                    {
                        wAuth.center.Stipend.Add(stipend);
                    }
                    wAuth.center.SaveChanges();
                    string s = Environment.CurrentDirectory;
                    if (s.Contains("Debug"))
                    {
                        s = s.Remove(s.Length - 10, 10);
                    }
                    if (s.Contains("EmploymentCenter"))
                    {

                    }
                    mp.Open(new Uri($@"{s}" + @"\" + "music" + @"\" + "money.mp3"));
                    mp.Volume = 0.1;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(stipend.ID == 0)
            {
                dtSt.SelectedDate = DateTime.Now;
                dtF.SelectedDate = dtSt.SelectedDate.Value.AddDays(90);
                txtSum.Text = "1500";
                stipend.Payment = 1500;

            }
            if (user.Roles.Name == "Инспектор")
            {
                var worker = wAuth.center.Workers.Where(p => p.ID_User == user.ID).ToList();
                cmbWork.ItemsSource = worker;
                cmbWork.SelectedIndex = 0;
                cmbWork.IsEnabled = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mp.Close();
        }
    }
}
