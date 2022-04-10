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
    /// Логика взаимодействия для pVacancies.xaml
    /// </summary>
    public partial class pVacancies : Page
    {
        public Users user;

        public int currentPage = 1;
        public int maxPage;
        public int count = 12;

        public pVacancies(Users user)
        {
            InitializeComponent();
            this.user = user;
            Update();
            Refresh();
            if (user.Roles.Name == "Соискатель" ||
                user.Roles.Name == "Менеджер организации")
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDel.Visibility = Visibility.Hidden;
                btnRed.Visibility = Visibility.Hidden;
                btnArchiv.Visibility = Visibility.Hidden;
            }
            if(user.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void Update()
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
                .Where(p => p.Valid == true)
                .OrderByDescending(p => p.Date)
                .ToList();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vacancy = btn.DataContext as Vacancy;
            windows.wVacancyAddRed red = new windows.wVacancyAddRed(vacancy, user);
            red.ShowDialog();
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vacancy = grVacancy.SelectedItem as Vacancy;
                if (vacancy != null)
                {
                    windows.wVacancyAddRed red = new windows.wVacancyAddRed(vacancy, user);
                    red.ShowDialog();
                    Update();
                    Refresh();
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
            Vacancy vacancy = new Vacancy();
            windows.wVacancyAddRed add = new windows.wVacancyAddRed(vacancy, user);
            add.ShowDialog();
            Update();
            Refresh();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grVacancy.SelectedItem as Vacancy;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.Vacancy.Remove(item);
                        windows.wAuth.center.SaveChanges();
                        Update();
                        Refresh();
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

        private void txtName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtName.Text = "";
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = txtName.Text;
            if (name != "" && name != "Начните вводить название вакансии...")
            {
                var list = windows.wAuth.center.Vacancy
                    .Where(p => p.Valid == true).ToList();
                var res = list.Where(p => p.Header.ToLower().StartsWith(name.ToLower())).ToList();
                grVacancy.ItemsSource = res;

            }
            if (name == "")
            {
                Update();
                Refresh();
            }
        }

        private void btnFiltr_Click(object sender, RoutedEventArgs e)
        {
            Update();
            Vacancy filtr = new Vacancy();
            windows.wVacancyAddRed f = new windows.wVacancyAddRed(filtr, user);
            f.ShowDialog();
            if(f.DialogResult == true)
            {
                var list = grVacancy.ItemsSource as List<Vacancy>;
                var header = list;
                var zp = list;
                if(f.vacancy.Header == null)
                {
                    header = list.Where(p => p.Header == null).ToList();
                }
                else
                {
                    header = list.Where(p => p.Header.ToLower().StartsWith(f.vacancy.Header.ToLower())).ToList();
                }
                if(f.vacancy.Wages == 0)
                {
                    zp = list.Where(p => p.Wages < 0).ToList();
                }
                else
                {
                    zp = list.Where(p => p.Wages >= f.vacancy.Wages).ToList();
                }
                var org = list.Where(p => p.ID_Org == f.vacancy.ID_Org).ToList();
                var educ = list.Where(p => p.Education == f.vacancy.Education).ToList();
                var prof = list.Where(p => p.Specialization == f.vacancy.Specialization).ToList();
                var reg = list.Where(p => p.Region == f.vacancy.Region).ToList();
                var type = list.Where(p => p.TypeOfEmployemnt == f.vacancy.TypeOfEmployemnt).ToList();
                var work = list.Where(p => p.WorkSchedule == f.vacancy.WorkSchedule).ToList();
                var exp = list.Where(p => p.ExpWork == f.vacancy.ExpWork).ToList();
                var inv = list.Where(p => p.ForDisabled == f.vacancy.ForDisabled).ToList();
                if(inv.Count > 0)
                {
                    grVacancy.ItemsSource = inv;
                }
                if(zp.Count > 0)
                {
                    grVacancy.ItemsSource = zp;
                }
                if(header.Count > 0)
                {
                    grVacancy.ItemsSource = header;
                }
                if (org.Count > 0)
                {
                    grVacancy.ItemsSource = org;
                }
                if (educ.Count > 0)
                {
                    grVacancy.ItemsSource = educ;
                }
                if (prof.Count > 0)
                {
                    grVacancy.ItemsSource = prof;
                }
                if (reg.Count > 0)
                {
                    grVacancy.ItemsSource = reg;
                }
                if (type.Count > 0)
                {
                    grVacancy.ItemsSource = type;
                }
                if (work.Count > 0)
                {
                    grVacancy.ItemsSource = work;
                }
                if (exp.Count > 0)
                {
                    grVacancy.ItemsSource = exp;
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Update();
            Refresh();
            txtName.Text = "Начните вводить название вакансии...";
        }

        List<Vacancy> list;

        private void Refresh()
        {
            list = windows.wAuth.center.Vacancy.Where(p => p.Valid == true)
                .OrderByDescending(p => p.Date).ToList();
            maxPage = (int)Math.Ceiling(list.Count * 1.0 / count);
            var listPage = list.Skip((currentPage - 1) * count).Take(count).ToList();
            txtCount.Text = currentPage.ToString();
            txtTotal.Text = maxPage.ToString();
            grVacancy.ItemsSource = listPage;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Refresh();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(currentPage <= 1) currentPage = 1;
            else currentPage--;
            Refresh();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage >= maxPage) currentPage = maxPage;
            else currentPage++;
            Refresh();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            currentPage = maxPage;
            Refresh();
        }

        private void btnArchiv_Click(object sender, RoutedEventArgs e)
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
            .Where(p => p.Valid == false)
            .OrderByDescending(p => p.Date)
            .ToList();
        }

        private void btnInv_Click(object sender, RoutedEventArgs e)
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
                .Where(p => p.ForDisabled == true)
                .OrderByDescending(p => p.Date)
                .ToList();
        }
    }
}
