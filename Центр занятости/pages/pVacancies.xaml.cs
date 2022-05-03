//**********************************************************************************************************
//* Название программы: "Центр занятости"                                                           ********
//*                                                                                                 ********
//* Назначение прорграмммы: программа предназначена для                                             ********
//* автоматизации типовых процессов центра занятости                                                ********
//*                                                                                                 ********
//* Для начала использования программы нажмите F5                                                   ********
//*                                                                                                 ********
//* Правила постановки на учет в центре занятости:                                                  ********
//* https://riavrn.ru/economy/kak-vstat-na-birzhu-truda-po-bezrabotice/                             ********
//* Общероссийская база вакансий:                                                                   ********
//* https://trudvsem.ru/                                                                            ********
//*                                                                                                 ********
//* Разработчик: студент группы ПР-365/б Бодин И.О.                                                 ********
//*                                                                                                 ********
//* Дата написания: 24.04.2022                                                                      ********
//*                                                                                                 ********
//* Версия: 1.0                                                                                     ********
//**********************************************************************************************************


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

        /// <summary>
        /// Основной метод страницы
        /// </summary>
        /// <param name="user"></param>
        public pVacancies(Users user)
        {
            InitializeComponent();
            this.user = user;
            Update();
            Refresh();
            
            //Проверка роли пользователя

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
        /// <summary>
        /// Обновление тааблицы "Вакансии"
        /// </summary>
        private void Update()
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
                .Where(p => p.Valid == true)
                .OrderByDescending(p => p.Date)
                .ToList();
        }
        /// <summary>
        /// Просмотр вакансии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vacancy = btn.DataContext as Vacancy;
            windows.wVacancyAddRed red = new windows.wVacancyAddRed(vacancy, user);
            red.ShowDialog();
        }
        /// <summary>
        /// Редактирование вакансии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Добавление вакансии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Vacancy vacancy = new Vacancy();
            windows.wVacancyAddRed add = new windows.wVacancyAddRed(vacancy, user);
            add.ShowDialog();
            Update();
            Refresh();
        }
        /// <summary>
        /// Удаление вакансии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        //проверка существующих записей
                        var res = windows.wAuth.center.ReferralToWork.Where(p => p.ID_Vacancy == item.ID).ToList();
                        if(res.Count > 0)
                        {
                            MessageBox.Show("Удаление невозможно, поскольку с данной вакансией есть связанные направления на работу",
                                "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            windows.wAuth.center.Vacancy.Remove(item);
                            windows.wAuth.center.SaveChanges();
                            Update();
                            Refresh();
                            MessageBox.Show("Успешно удалено", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите элемент, прежде чем удалять", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Метод: при нажатии на поле поиска, очишает это поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtName.Text = "";
        }

        /// <summary>
        /// Метод для текстового поля поиска вакансии по названию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        /// <summary>
        /// Метод, предназначенный для фильтрации таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Метод на кнопку "Очистить фильтры"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Update();
            Refresh();
            txtName.Text = "Начните вводить название вакансии...";
        }

        List<Vacancy> list;

        /// <summary>
        /// Метод для пагинации таблицы
        /// </summary>
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

        /// <summary>
        /// Метод, возвращающий на 1 страницу таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Refresh();
        }

        /// <summary>
        /// Метод, возвращающий назад на одну страницу таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(currentPage <= 1) currentPage = 1;
            else currentPage--;
            Refresh();
        }

        /// <summary>
        /// Метод, возвращающий на одну страницу таблицы вперед 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage >= maxPage) currentPage = maxPage;
            else currentPage++;
            Refresh();
        }

        /// <summary>
        /// Метод, возвращающий на последнюю страницу таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            currentPage = maxPage;
            Refresh();
        }

        /// <summary>
        /// Метод для отображения недействительных вакансий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArchiv_Click(object sender, RoutedEventArgs e)
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
            .Where(p => p.Valid == false)
            .OrderByDescending(p => p.Date)
            .ToList();
        }

        /// <summary>
        /// Метод, возвращающий вакансии для людей с инвалидностью
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInv_Click(object sender, RoutedEventArgs e)
        {
            grVacancy.ItemsSource = windows.wAuth.center.Vacancy
                .Where(p => p.ForDisabled == true && p.Valid == true)
                .OrderByDescending(p => p.Date)
                .ToList();
        }
    }
}
