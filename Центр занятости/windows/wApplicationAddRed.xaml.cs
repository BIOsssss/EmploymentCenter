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
    /// Логика взаимодействия для wApplicationAddRed.xaml
    /// </summary>
   
    
    public partial class wApplicationAddRed : Window
    {
        public ApplicationOfUnemployed application;
        public Users user;
        public Applicants app;
        public wApplicationAddRed(ApplicationOfUnemployed application, Users user)
        {
            InitializeComponent();
            this.application = application;
            this.user = user;
            cmbApp.ItemsSource = wAuth.center.Applicants.
                Where(p => p.Passport != 0 &&
                p.PaymentAcc != null &&
                p.Passport1 != null
                && p.Address != 0 &&
                p.Address != null &&
                p.RegistrationAddress != null).ToList();
            cmbWork.ItemsSource = wAuth.center.Workers.ToList();
            cmbStatus.ItemsSource = wAuth.center.StatusUnemployed.ToList();
            DataContext = application;
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
                if(cmbStatus.SelectedIndex == -1)
                {
                    error += "Выберите статус заявления\n";
                }
                if(application.CommentWorker != null)
                {
                    if(cmbWork.SelectedIndex == -1)
                    {
                        error += "Комментарий не может существовать без инспектора\n";
                    }
                    else
                    {
                        application.DateComment =
                        application.TimeComment = DateTime.Now;
                    }
                }
                if(error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(application.ID == 0)
                    {
                        wAuth.center.ApplicationOfUnemployed.Add(application);
                        app.Registered = true;
                    }
                    else
                    {
                        application.DateComment = DateTime.Now;
                        application.TimeComment = DateTime.Now;
                    }
                    wAuth.center.SaveChanges();
                    mp.Open(new Uri("pismo.mp3", UriKind.Relative));
                    mp.Play();
                    MessageBox.Show("Данные сохранены", "Внимание",
                        MessageBoxButton.OK);
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
            if(application.ID == 0)
            {
                cmbStatus.SelectedIndex = 0;
                cmbStatus.IsEnabled = false;
                application.DateApplicant =
                application.TimeApplicant = DateTime.Now;
            }
            if (user.Roles.Name == "Инспектор")
            {
                var worker = wAuth.center.Workers.Where(p => p.ID_User == user.ID).ToList();
                cmbWork.ItemsSource = worker;
                cmbWork.SelectedIndex = 0;
                cmbWork.IsEnabled = false;
                if(application.ID != 0)
                {
                    application.DateComment =
                    application.TimeComment = DateTime.Now;
                }
            }
            if(user.Roles.Name == "Соискатель")
            {
                var appl = wAuth.center.Applicants.Where(p => p.ID_User == user.ID).ToList();
                cmbApp.ItemsSource = appl;
                cmbApp.SelectedIndex = 0;
                cmbApp.IsEnabled = false;
                cbDis.IsEnabled = false;
                cbNoWork.IsEnabled = false;
                txtComment.IsReadOnly = true;
                cmbStatus.IsEnabled = false;
                cmbWork.IsEnabled = false;
                btnOK.Visibility = Visibility.Hidden;
            }
        }

        private void cmbApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            app = cmbApp.SelectedItem as Applicants;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mp.Close();
        }
    }
}
