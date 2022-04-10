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
    /// Логика взаимодействия для wWorkAddRed.xaml
    /// </summary>
    public partial class wWorkAddRed : Window
    {
        public Applicants app;
        public ExpWorkUnemployed work;
        public wWorkAddRed(Applicants app, ExpWorkUnemployed work)
        {
            InitializeComponent();
            this.app = app;
            this.work = work;
            work.ID_Applicant = app.ID;
            DataContext = work;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(work.NameOrg == null)
                {
                    error += "Введите наименование организации\n";
                }
                if(work.OfficialDuties == null)
                {
                    error += "Введите должностные обязанности\n";
                }
                if(work.DateStart.ToShortDateString() == "01.01.0001")
                {
                    error += "Введите дату принятия на работу\n";
                }
                if(work.DateOfDismissal == null)
                {

                }
                else
                {
                    TimeSpan? ts = work.DateOfDismissal - work.DateStart;
                    if (work.DateStart >= work.DateOfDismissal)
                    {
                        error += "Дата увольнения не может быть раньше, чем дата принятия на работу\n";
                    }
                    if (ts.Value.Days < 28)
                    {
                        error += "Разница между датой принятия на работу и датой увольнения должна составлять как минимум один месяц\n";
                    }
                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(work.ID == 0)
                    {
                        wAuth.center.ExpWorkUnemployed.Add(work);
                        wAuth.center.SaveChanges();
                        app.WorkExp = work.ID;
                    }
                    wAuth.center.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
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
            if(work.ID == 0)
            {
                dtStart.SelectedDate = DateTime.Now;
                dtEnd.SelectedDate = null;
            }
        }
    }
}
