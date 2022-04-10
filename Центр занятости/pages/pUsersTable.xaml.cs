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
    /// Логика взаимодействия для pUsersTable.xaml
    /// </summary>
    public partial class pUsersTable : Page
    {

        public int currentPage = 1;
        public int maxPage;
        public int count = 13;

        public string role;
        public Users user_main;
        public pUsersTable(string role, Users user_main)
        {
            InitializeComponent();
            this.role = role;
            Update();
            Refresh();
            this.user_main = user_main;
            if (user_main.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
            if(role != "Соискатель")
            {
                btnArchiv.Visibility = Visibility.Hidden;
                btnClear.Visibility = Visibility.Hidden;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Users user = new Users();
            var id = windows.wAuth.center.Roles.Where(x => x.Name == role);
            foreach(var item in id)
            {
                user.Role = item.ID;
            }
            windows.wUsersAddRed add = new windows.wUsersAddRed(user, user_main);
            add.ShowDialog();
            Update();
            Refresh();
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var user = btn.DataContext as Users;
            windows.wUsersAddRed red = new windows.wUsersAddRed(user, user_main);
            red.ShowDialog();
            Update();
            Refresh();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grUsers.SelectedItem as Users;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание", 
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.Users.Remove(item);
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

        private List<Users> Update()
        {
            if(role != "Соискатель")
            {
                var items = windows.wAuth.center.Users.Where(x => x.Roles.Name == role).ToList();
                grUsers.ItemsSource = items;
                return items;
            }
            else
            {
                var items = windows.wAuth.center.Users
                .Where(x => x.Roles.Name == role)
                .ToList();
                List<Applicants> applicants = new List<Applicants>();
                foreach (var item in items)
                {
                    var it = windows.wAuth.center.Applicants
                        .Where(p => p.Registered == true && p.ID_User == item.ID).ToList();
                    foreach (var i in it)
                    {
                        applicants.Add(i);
                    }
                }
                List<Users> users = new List<Users>();
                foreach (var applicant in applicants)
                {
                    var it = windows.wAuth.center.Users.Where(p => p.ID == applicant.ID_User);
                    foreach (var i in it)
                    {
                        users.Add(i);
                    }
                }
                grUsers.ItemsSource = users;
                return users;
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var user = btn.DataContext as Users;
                if (role == "Инспектор")
                {
                    bool res = false;
                    Workers worker = null;
                    foreach (var item in windows.wAuth.center.Workers)
                    {
                        if (user.ID == item.ID_User)
                        {
                            res = true;
                            worker = item;
                        }
                    }
                    if (res)
                    {
                        windows.wWorkersAddRed red = new windows.wWorkersAddRed(worker, user, user_main);
                        red.ShowDialog();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого пользователя нету информации о себе\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            worker = new Workers();
                            windows.wWorkersAddRed add = new windows.wWorkersAddRed(worker, user, user_main);
                            add.ShowDialog();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (role == "Соискатель")
                {
                    bool res = false;
                    Applicants applicant = null;
                    foreach (var item in windows.wAuth.center.Applicants)
                    {
                        if (user.ID == item.ID_User)
                        {
                            res = true;
                            applicant = item;
                        }
                    }
                    if (res)
                    {
                        windows.wApplicantsAddRed red = new windows.wApplicantsAddRed(applicant,user, user_main);
                        red.ShowDialog();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого пользователя нету информации о себе\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            applicant = new Applicants();
                            windows.wApplicantsAddRed add = new windows.wApplicantsAddRed(applicant,user, user_main);
                            add.ShowDialog();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (role == "Менеджер организации")
                {
                    bool res = false;
                    ManagerOrg manager = null;
                    foreach (var item in windows.wAuth.center.ManagerOrg)
                    {
                        if (user.ID == item.ID_User)
                        {
                            res = true;
                            manager = item;
                        }
                    }
                    if (res)
                    {
                        windows.wManagersAddRed red = new windows.wManagersAddRed(manager, user, user_main);
                        red.ShowDialog();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого пользователя нету информации о себе\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            manager = new ManagerOrg();
                            windows.wManagersAddRed add = new windows.wManagersAddRed(manager, user, user_main);
                            add.ShowDialog();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Refresh()
        {
            maxPage = (int)Math.Ceiling(Update().Count * 1.0 / count);
            var listPage = Update().Skip((currentPage - 1) * count).Take(count).ToList();
            txtCount.Text = currentPage.ToString();
            txtTotal.Text = maxPage.ToString();
            grUsers.ItemsSource = listPage;
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

        private void btnArchiv_Click(object sender, RoutedEventArgs e)
        {
            if(role == "Соискатель")
            {
                var items = windows.wAuth.center.Users
                    .Where(x => x.Roles.Name == role)
                    .ToList();
                List<Applicants> applicants = new List<Applicants>();
                foreach (var item in items)
                {
                    var it = windows.wAuth.center.Applicants
                        .Where(p => p.Registered == false && p.ID_User == item.ID).ToList();
                    foreach(var i in it)
                    {
                        applicants.Add(i);
                    }
                }
                List<Users> users = new List<Users>();
                foreach(var applicant in applicants)
                {
                    var it = windows.wAuth.center.Users.Where(p => p.ID == applicant.ID_User);
                    foreach(var i in it)
                    {
                        users.Add(i);
                    }
                }
                var nul = windows.wAuth.center.Users
                .Where(x => x.Roles.Name == role)
                .ToList();
                foreach(var item in nul)
                {
                    if(item.Applicants.Count == 0) users.Add(item);
                }
                grUsers.ItemsSource = users;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Update();
            Refresh();
        }
    }
}
