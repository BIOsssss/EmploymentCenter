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
using System.IO;

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wVacancyAddRed.xaml
    /// </summary>
    public partial class wVacancyAddRed : Window
    {
        public Vacancy vacancy;
        public Users user;
        public wVacancyAddRed(Vacancy vacancy, Users user)
        {
            InitializeComponent();
            this.vacancy = vacancy;
            this.user = user;
            DataContext = vacancy;
            cmbEd.ItemsSource = wAuth.center.Educations.ToList();
            cmbOpyt.ItemsSource = wAuth.center.ExpWorkType.ToList();
            cmbOrg.ItemsSource = wAuth.center.Organization.OrderBy(p => p.FullNameOrg).ToList();
            cmbProf.ItemsSource = wAuth.center.Professions.ToList();
            cmbReg.ItemsSource = wAuth.center.Regions.ToList();
            cmbSched.ItemsSource = wAuth.center.WorkSchedule.ToList();
            cmbType.ItemsSource = wAuth.center.TypeOfEmployment.ToList();
        }

        private void cmbOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var organization = (Organization)cmbOrg.SelectedItem;
            if(organization != null)
            {
                if (organization.Image == null)
                {
                    string path = $@"{Environment.CurrentDirectory}/image/no-photo.jpg";
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists)
                    {
                        img.Source = new BitmapImage(new Uri(path));
                    }
                }
                else
                {
                    string file = organization.Image;
                    string path = $@"{Environment.CurrentDirectory}{file}";
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists)
                    {
                        img.Source = new BitmapImage(new Uri(path));
                    }
                }
                org.Text = organization.AbbreviatedName;
            }
            else
            {
                return;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(vacancy.Header == null)
                {
                    error += "Введите название вакансии\n";
                }
                if(cmbProf.SelectedIndex == -1)
                {
                    error += "Выберите профессию\n";
                }
                if(vacancy.Wages <= 0)
                {
                    error += "Зарплата не может быть отрицательной либо равной нулю\n";
                }
                if(cmbReg.SelectedIndex == -1)
                {
                    error += "Выберите регион\n";
                }
                if(cmbType.SelectedIndex == -1)
                {
                    error += "Выберите тип занятости\n";
                }
                if(cmbSched.SelectedIndex == -1)
                {
                    error += "Выберите график работы\n";
                }
                if(cmbOpyt.SelectedIndex == -1)
                {
                    error += "Выберите опыт работы\n";
                }
                if(cmbOrg.SelectedIndex == -1)
                {
                    error += "Выберите организацию\n";
                }
                if(error != "")
                {
                    MessageBox.Show(error,"Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(vacancy.ID == 0)
                    {
                        wAuth.center.Vacancy.Add(vacancy);
                    }
                    wAuth.center.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
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
            tlEd.Content = cmbEd.Text;
            tlHeader.Content = txtHead.Text;
            tlOpyt.Content = cmbOpyt.Text;
            tlOrg.Content = cmbOrg.Text;
            tlProf.Content = cmbProf.Text;
            tlReg.Content = cmbReg.Text;
            tlSched.Content = cmbSched.Text;
            tlType.Content = cmbType.Text;
            if(vacancy.ID == 0)
            {
                vacancy.Valid = true;
                vacancy.Date = DateTime.Now;
            }
            if(user.Roles.Name == "Инспектор"
                || user.Roles.Name == "Администратор")
            {
                vacancy.Date = DateTime.Now;
            }
            if (vacancy.Valid == false)
            {
                cmbProf.IsEnabled = false;
                cmbProf.IsEditable = false;
                cmbEd.IsEnabled = false;
                cmbOpyt.IsEnabled = false;
                cmbOrg.IsEnabled = false;
                cmbOrg.IsEditable = false;
                cmbReg.IsEnabled = false;
                cmbReg.IsEditable = false;
                cmbSched.IsEnabled = false;
                cmbType.IsEnabled = false;
                txtHead.IsReadOnly = true;
                txtInfo.IsReadOnly = true;
                txtWages.IsReadOnly = true;
                cbDis.IsEnabled = false;
                btnOK.Visibility = Visibility.Hidden;
            }

            if (user.Roles.Name == "Соискатель")
            {
                btnOK.Visibility = Visibility.Hidden;
                if (vacancy.ID != 0)
                {
                    cmbProf.IsEnabled = false;
                    cmbProf.IsEditable = false;
                    cmbEd.IsEnabled = false;
                    cmbOpyt.IsEnabled = false;
                    cmbOrg.IsEnabled = false;
                    cmbOrg.IsEditable = false;
                    cmbReg.IsEnabled = false;
                    cmbReg.IsEditable = false;
                    cmbSched.IsEnabled = false;
                    cmbType.IsEnabled = false;
                    txtHead.IsReadOnly = true;
                    txtInfo.IsReadOnly = true;
                    txtWages.IsReadOnly = true;
                    cbDis.IsEnabled = false;
                    cbVal.IsEnabled = false;
                    btnFiltr.Visibility = Visibility.Hidden;
                }
                else
                {
                    cbVal.IsEnabled = false;
                }


            }
            if (user.Roles.Name == "Менеджер организации")
            {
                btnOK.Visibility = Visibility.Hidden;
                if (vacancy.ID != 0)
                {
                    cmbProf.IsEnabled = false;
                    cmbProf.IsEditable = false;
                    cmbEd.IsEnabled = false;
                    cmbOpyt.IsEnabled = false;
                    cmbOrg.IsEnabled = false;
                    cmbOrg.IsEditable = false;
                    cmbReg.IsEnabled = false;
                    cmbReg.IsEditable = false;
                    cmbSched.IsEnabled = false;
                    cmbType.IsEnabled = false;
                    txtHead.IsReadOnly = true;
                    txtInfo.IsReadOnly = true;
                    txtWages.IsReadOnly = true;
                    cbDis.IsEnabled = false;
                    cbVal.IsEnabled = false;
                    btnFiltr.Visibility = Visibility.Hidden;
                }
                else
                {
                    cbVal.IsEnabled = false;
                }
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var organization = (Organization)cmbOrg.SelectedItem;
                var man = wAuth.center.ManagerOrg.Where(p => p.ID == organization.ID_Manager);
                ManagerOrg manager = new ManagerOrg();
                foreach (var p in man)
                {
                    manager = p;
                }
                wOrgAddRed info = new wOrgAddRed(organization, manager, user);
                info.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnFiltr_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void cbVal_Checked(object sender, RoutedEventArgs e)
        {
            if(cbVal.IsChecked == true)
            {
                cmbProf.IsEnabled = true;
                cmbProf.IsEditable = true;
                cmbEd.IsEnabled = true;
                cmbOpyt.IsEnabled = true;
                cmbOrg.IsEnabled = true;
                cmbOrg.IsEditable = true;
                cmbReg.IsEnabled = true;
                cmbReg.IsEditable = true;
                cmbSched.IsEnabled = true;
                cmbType.IsEnabled = true;
                txtHead.IsReadOnly = false;
                txtInfo.IsReadOnly = false;
                txtWages.IsReadOnly = false;
                cbDis.IsEnabled = true;
                btnOK.Visibility = Visibility.Visible;
            }
            else
            {
                cmbProf.IsEnabled = false;
                cmbProf.IsEditable = false;
                cmbEd.IsEnabled = false;
                cmbOpyt.IsEnabled = false;
                cmbOrg.IsEnabled = false;
                cmbOrg.IsEditable = false;
                cmbReg.IsEnabled = false;
                cmbReg.IsEditable = false;
                cmbSched.IsEnabled = false;
                cmbType.IsEnabled = false;
                txtHead.IsReadOnly = true;
                txtInfo.IsReadOnly = true;
                txtWages.IsReadOnly = true;
                cbDis.IsEnabled = false;
                btnOK.Visibility = Visibility.Hidden;
            }
        }

        MediaPlayer mp = new MediaPlayer();
        private void cmbProf_DropDownOpened(object sender, EventArgs e)
        {
            mp.Open(new Uri("elevator.mp3", UriKind.Relative));
            mp.Volume = 0.1;
            mp.Play();
        }

        private void cmbProf_DropDownClosed(object sender, EventArgs e)
        {
            mp.Close();
        }
    }
}
