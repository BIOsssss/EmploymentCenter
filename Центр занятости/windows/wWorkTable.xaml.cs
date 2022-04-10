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
    /// Логика взаимодействия для wWorkTable.xaml
    /// </summary>
    public partial class wWorkTable : Window
    {
        public int currentPage = 1;
        public int maxPage;
        public int count = 12;

        public Applicants app;
        public wWorkTable(Applicants app, Users user)
        {
            InitializeComponent();
            this.app = app;
            if(user.Roles.Name == "Инспектор" || 
                user.Roles.Name == "Соискатель")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
            try
            {
                Update();
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<ExpWorkUnemployed> Update()
        {
            var res = wAuth.center.ExpWorkUnemployed.Where(p => p.ID_Applicant == app.ID).ToList();
            grWorks.ItemsSource = res;
            return res;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grWorks.SelectedItem as ExpWorkUnemployed;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.ExpWorkUnemployed.Remove(item);
                        windows.wAuth.center.SaveChanges();
                        Update();
                        Refresh();
                        MessageBox.Show("Успешно удалено", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выбирете элемент(ы), прежде чем удалять", "Внимание",
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
            ExpWorkUnemployed work = new ExpWorkUnemployed();
            wWorkAddRed add = new wWorkAddRed(app, work);
            add.ShowDialog();
            Update();
            Refresh();
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var work = btn.DataContext as ExpWorkUnemployed;
            wWorkAddRed red = new wWorkAddRed(app,work);
            red.ShowDialog();
            Update();
            Refresh();
        }

        private void Refresh()
        {
            maxPage = (int)Math.Ceiling(Update().Count * 1.0 / count);
            var listPage = Update().Skip((currentPage - 1) * count).Take(count).ToList();
            txtCount.Text = currentPage.ToString();
            txtTotal.Text = maxPage.ToString();
            grWorks.ItemsSource = listPage;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Refresh();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage <= 1) currentPage = 1;
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

    }
}
