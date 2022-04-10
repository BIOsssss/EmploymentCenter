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
    /// Логика взаимодействия для wUsersAddRed.xaml
    /// </summary>
    public partial class wUsersAddRed : Window
    {
        public Users user;
        public Users user_main;
        public wUsersAddRed(Users user, Users user_main)
        {
            InitializeComponent();
            this.user = user;
            DataContext = user;
            cmbRole.ItemsSource = windows.wAuth.center.Roles.ToList();
            if(user_main.Roles.Name == "Администратор" && user.Role == 1)
            {
                btnInfo.Visibility = Visibility.Hidden;
            }
            else
            {
                btnInfo.Visibility = Visibility.Visible;
            }
            if ((user_main.Roles.Name == "Администратор") 
                && user.Role != 1)
            {
                btnPass.Visibility = Visibility.Visible;
            }
            else if(user_main.Roles.Name == "Инспектор"
                && (user.Role == 3|| user.Role == 9))
            {
                btnPass.Visibility = Visibility.Visible;
            }
            else
            {
                btnPass.Visibility = Visibility.Hidden;
            }

            this.user_main = user_main;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if(user.NameUser == null)
                {
                    error += "Введите имя пользователя\n";
                }
                else
                {
                    if (user.NameUser.Length < 6 || user.NameUser.Length > 30)
                    {
                        error += "Имя пользователя не должна быть меньше 6 символов и не больше 30 символов\n";
                    }
                }
                if(user.Password == null)
                {
                    error += "Введите пароль\n";
                }
                else
                {
                    if (user.Password.Length < 6 || user.NameUser.Length > 30)
                    {
                        error += "Пароль не должен быть меньше 6 символов и не больше 30 символов\n";
                    }
                }
                if(user.ID == 0)
                {
                    foreach (var item in wAuth.center.Users.ToList())
                    {
                        if (user.NameUser == item.NameUser)
                        {
                            error += "Пользователь с таким именем уже сушествует\n";
                        }
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
                    if (user.ID == 0)
                    {
                        wAuth.center.Users.Add(user);
                    }
                    wAuth.center.SaveChanges();
                    MessageBox.Show("Успешно сохранено", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Внимание", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(user.ID == 0)
                {
                    MessageBox.Show("Пользователь не зарегистрирован, поэтому подробная информация не доступна",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Roles.Name == "Инспектор")
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
                if (user.Roles.Name == "Соискатель")
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
                        windows.wApplicantsAddRed red = new windows.wApplicantsAddRed(applicant, user, user_main);
                        red.ShowDialog();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого пользователя нету информации о себе\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            applicant = new Applicants();
                            windows.wApplicantsAddRed add = new windows.wApplicantsAddRed(applicant, user, user_main);
                            add.ShowDialog();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (user.Roles.Name == "Менеджер организации")
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public char[] eng = new char[]
{
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x', 'y',
            'z'
};

        public char[] symbols = new char[]
{
            '-',
            ';',
            '+',
            '_',
            '=',
            '"',
            '[',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '?',
            '*',
            ')',
            '(',
            '!',
            ']'
};

        public char[] numbers = new char[]
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9'
        };

        Random ran = new Random();

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (user.NameUser != null)
                {
                    txtPass.Text = "";
                    char[] pass = new char[10];
                    pass[ran.Next(0, pass.Length)] = symbols[ran.Next(0, symbols.Length)];
                    for (int i = 0; i < 1; i++)
                    {
                        if (pass[i] == '\0')
                        {
                            pass[i] = numbers[ran.Next(0, numbers.Length)];
                        }
                    }
                    for (int i = 0; i < pass.Length; i++)
                    {
                        if (pass[i] == '\0')
                        {
                            pass[i] = char.ToUpper(eng[ran.Next(0, eng.Length)]);
                            break;
                        }
                    }
                    for (int i = 0; i < pass.Length; i++)
                    {
                        if (pass[i] == '\0')
                        {
                            pass[i] = eng[ran.Next(0, eng.Length)];
                        }
                    }
                    for (int i = 0; i < pass.Length; i++)
                    {
                        for (int j = i + 1; j < pass.Length; j++)
                        {
                            if (pass[i] == pass[j])
                            {
                                if (eng.Contains(pass[j]))
                                {
                                    pass[j] = eng[ran.Next(0, eng.Length)];
                                }
                                if (numbers.Contains(pass[j]))
                                {
                                    pass[j] = numbers[ran.Next(0, numbers.Length)];
                                }
                            }
                        }
                    }
                    for (int i = 0; i < pass.Length; i++)
                    {
                        txtPass.Text += pass[i];
                    }
                    user.Password = txtPass.Text;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach(char c in txtName.Text)
            {
                string s = c.ToString().ToLower();
                if(eng.Contains(s[0]) == false
                    && numbers.Contains(c) == false
                    && symbols.Contains(c) == false)
                {
                    MessageBox.Show("Имя пользователя должна содержать английские буквы," +
                        " цифры и символы", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    txtName.Text = user.NameUser;
                    return;
                }
            }
        }

        private void txtPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (char c in txtPass.Text)
            {
                string s = c.ToString().ToLower();
                if (eng.Contains(s[0]) == false
                    && numbers.Contains(c) == false
                    && symbols.Contains(c) == false)
                {
                    MessageBox.Show("Пароль должен содержать английские буквы," +
                        " цифры и символы", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    txtPass.Text = user.Password;
                    return;
                }
            }
        }
    }
}
