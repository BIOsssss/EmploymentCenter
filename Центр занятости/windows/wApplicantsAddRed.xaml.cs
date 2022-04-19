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
    /// Логика взаимодействия для wApplicantsAddRed.xaml
    /// </summary>
    public partial class wApplicantsAddRed : Window
    {
        public Applicants applicant;
        public Users user_main;
        public wApplicantsAddRed(Applicants applicant, Users user, Users user_main)
        {
            InitializeComponent();
            this.applicant = applicant;
            DataContext = applicant;
            this.applicant.ID_User = user.ID;
            cmbGender.ItemsSource = wAuth.center.Genders.ToList();
            cmbEducation.ItemsSource = wAuth.center.Educations.ToList();
            cmbCitez.ItemsSource = wAuth.center.Countries.ToList();
            cmbProf.ItemsSource = wAuth.center.Professions.ToList();
            this.user_main = user_main;
            if (user_main.Roles.Name == "Соискатель" ||
                user_main.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddPas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (applicant.ID == 0)
                {
                    MessageBox.Show("Соискатель не зарегистрирован, поэтому добавление паспорта невозможно",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool res = false;
                    Passport pass = null;
                    foreach (var passport in windows.wAuth.center.Passport)
                    {
                        if (passport.ID == applicant.Passport)
                        {
                            res = true;
                            pass = passport;
                        }
                    }
                    if (res)
                    {
                        wPassAddRed red = new wPassAddRed(pass, applicant, user_main);
                        red.ShowDialog();
                        wAuth.center.SaveChanges();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого соискателя нету информации о паспорте\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            pass = new Passport();
                            wPassAddRed add = new wPassAddRed(pass, applicant, user_main);
                            add.ShowDialog();
                            applicant.Passport = add.passport.ID;
                            wAuth.center.SaveChanges();
                            txtPassp.Text = $"{applicant.Passport1.Series} " +
                                $"{applicant.Passport1.Number}";
                            tlPass.Content = txtPassp.Text;
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

        private void btnAddWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (applicant.ID == 0)
                {
                    MessageBox.Show("Соискатель не зарегистрирован, поэтому добавление места работы невозможно",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool res = false;
                    ExpWorkUnemployed work = null;
                    foreach (var works in windows.wAuth.center.ExpWorkUnemployed)
                    {
                        if (works.ID_Applicant == applicant.ID)
                        {
                            res = true;
                            work = works;
                            break;
                        }
                    }
                    if (res)
                    {
                        wWorkTable red = new wWorkTable(applicant, user_main);
                        red.ShowDialog();
                        txtWork.Text = $"{applicant.Opyt}";
                        tlWork.Content = txtWork.Text;
                    }
                    else
                    {
                        if (MessageBox.Show("У этого соискателя нету информации об опыте работы\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            work = new ExpWorkUnemployed();
                            wWorkAddRed add = new wWorkAddRed(applicant, work);
                            add.ShowDialog();
                            wAuth.center.SaveChanges();
                            txtWork.Text = $"{applicant.Opyt}";
                            tlWork.Content = txtWork.Text;
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

        private void btnAddAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (applicant.ID == 0)
                {
                    MessageBox.Show("Соискатель не зарегистрирован, поэтому добавление платежного счета невозможно",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool res = false;
                    PaymentAccount acc = null;
                    foreach (var account in windows.wAuth.center.PaymentAccount)
                    {
                        if (account.ID == applicant.PaymentAcc)
                        {
                            res = true;
                            acc = account;
                        }
                    }
                    if (res)
                    {
                        wAccAddRed red = new wAccAddRed(acc, applicant, user_main);
                        red.ShowDialog();
                        wAuth.center.SaveChanges();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого соискателя нету информации о платежном счете\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            acc = new PaymentAccount();
                            wAccAddRed add = new wAccAddRed(acc, applicant, user_main);
                            add.ShowDialog();
                            applicant.PaymentAcc = add.account.ID;
                            wAuth.center.SaveChanges();
                            txtAcc.Text = $"{applicant.Acc}";
                            tlAcc.Content = txtAcc.Text;
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (applicant.ID == 0)
            {
                MessageBox.Show("Соискатель не зарегистрирован, поэтому удаление невозможно",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var adrress = wAuth.center.RegistrationAddress.Where(p => p.ID == applicant.Address).ToList();
                        var pass = wAuth.center.Passport.Where(p => p.ID == applicant.Passport).ToList();
                        var opyt = wAuth.center.ExpWorkUnemployed.Where(p => p.ID_Applicant == applicant.ID).ToList();
                        var payment = wAuth.center.PaymentAccount.Where(p => p.ID == applicant.PaymentAcc).ToList();
                        var zayv = wAuth.center.ApplicationOfUnemployed.Where(p => p.ID_Applicant == applicant.ID).ToList();
                        var work = wAuth.center.ReferralToWork.Where(p => p.ID_Applicant == applicant.ID).ToList();
                        var stip = wAuth.center.Stipend.Where(p => p.ID_Applicants == applicant.ID).ToList();

                        string error = "Невозможно удалить информацию о соискателе, поскольку есть связанная с ним информация:\n";
                        if(adrress.Count > 0)
                        {
                            error += "Адрес проживания\n";
                        }
                        if (pass.Count > 0)
                        {
                            error += "Паспортные данные\n";
                        }
                        if (opyt.Count > 0)
                        {
                            error += "Опыт работы\n";
                        }
                        if (payment.Count > 0)
                        {
                            error += "Рассчетный счет\n";
                        }
                        if (zayv.Count > 0)
                        {
                            error += "Заявление по безработице\n";
                        }
                        if (work.Count > 0)
                        {
                            error += "Направление на работу\n";
                        }
                        if (stip.Count > 0)
                        {
                            error += "Назначенные пособии\n";
                        }
                        if(error != "")
                        {
                            MessageBox.Show(error, "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            wAuth.center.Applicants.Remove(applicant);
                            wAuth.center.SaveChanges();
                            MessageBox.Show("Успешно удалено", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        public char[] snils = new char[]
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
            '9',
            '-',
            ' '
        };

        public char[] phone = new char[]
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
            '9',
            '+'
        };

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                TimeSpan? dr = DateTime.Today - dateBirthday.SelectedDate;
                int age = dr.Value.Days / 365;
                if(applicant.LastName == null)
                {
                    error += "Введите фамилию\n";
                }
                if (applicant.FirstName == null)
                {
                    error += "Введите фамилию\n";
                }
                if(cmbGender.SelectedIndex == -1)
                {
                    error += "Выберите пол\n";
                }
                if(age < 18)
                {
                    error += "Соискатель должен быть совершеннолетним\n";
                    dateBirthday.SelectedDate = DateTime.Now;
                }
                if(cmbEducation.SelectedIndex == -1)
                {
                    error += "Выберите образование\n";
                }
                if(applicant.INN == null)
                {
                    error += "Введите ИНН\n";
                }
                if(applicant.SNILS == null)
                {
                    error += "Введите СНИЛС\n";
                }
                else
                {
                    if(applicant.SNILS.Contains('-') &&
                        applicant.SNILS.Contains(' '))
                    {
                        foreach(char s in applicant.SNILS)
                        {
                            if (snils.Contains(s))
                            {

                            }
                            else
                            {
                                error += "Формат СНИЛС: XXX-XXX-XXX XX\n";
                                txtSNILS.Text = "";
                            }
                        }
                    }
                    else
                    {
                        error += "Формат СНИЛС: XXX-XXX-XXX XX\n";
                        txtSNILS.Text = "";
                    }
                }
                if(applicant.Phone == null)
                {
                    error += "Введите номер телефона\n";
                }
                else
                {
                    if ((applicant.Phone[0] == '+' && applicant.Phone[1] == '7') ||
                        applicant.Phone[0] == '8')
                    {
                        foreach (char c in applicant.Phone)
                        {
                            if (phone.Contains(c))
                            {

                            }
                            else
                            {
                                error += "Формат номера телефона: " +
                                    " +7XXXXXXXXXX " +
                                    " либо 8XXXXXXXXXX\n";
                                txtPhone.Text = "";
                            }
                        }
                    }
                    else
                    {
                        error += "Формат номера телефона: " +
                            " +7XXXXXXXXXX " +
                            " либо 8XXXXXXXXXX\n";
                        txtPhone.Text = "";
                    }
                }
                if(applicant.Email == null)
                {
                    error += "Введите адрес электронной почты\n";
                }
                else
                {
                    if(applicant.Email.Contains('@'))
                    {
                        if (applicant.Email.Contains('.'))
                        {

                        }
                        else
                        {
                            error += "Адрес электронной почты имеет следующий вид: " +
                            "nameuser2022@mail.ru\n";
                            txtEmail.Text = "";
                        }
                    }
                    else
                    {
                        error += "Адрес электронной почты имеет следующий вид: " +
                        "nameuser2022@mail.ru\n";
                        txtEmail.Text = "";
                    }
                }
                if(cmbCitez.SelectedIndex == -1)
                {
                    error += "Выберите гражданство\n";
                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(applicant.ID == 0)
                    {
                        wAuth.center.Applicants.Add(applicant);
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

        private void txtINN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(float.TryParse(txtINN.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("Введите 10 цифр ИНН",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtINN.Text = "";
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(applicant.ID == 0)
            {
                dateBirthday.SelectedDate = DateTime.Now;
            }
            if(user_main.Roles.Name == "Соискатель")
            {
                cbReg.IsEnabled = false;
            }
            try
            {
                txtAddress.Text = $"{applicant.Adres}";
                tlAdres.Content = txtAddress.Text;

                txtPassp.Text = $"{applicant.Pasp}";
                tlPass.Content = txtPassp.Text;

                txtWork.Text = $"{applicant.Opyt}";
                tlWork.Content = txtWork.Text;

                txtAcc.Text = $"{applicant.Acc}";
                tlAcc.Content = txtAcc.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (applicant.ID == 0)
                {
                    MessageBox.Show("Соискатель не зарегистрирован, поэтому добавление адреса проживания невозможно",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool res = false;
                    RegistrationAddress adres = null;
                    foreach (var address in windows.wAuth.center.RegistrationAddress)
                    {
                        if (address.ID == applicant.Address)
                        {
                            res = true;
                            adres = address;
                        }
                    }
                    if (res)
                    {
                        wAddressAddRed red = new wAddressAddRed(adres, applicant, user_main);
                        red.ShowDialog();
                        wAuth.center.SaveChanges();
                    }
                    else
                    {
                        if (MessageBox.Show("У этого соискателя нету информации об адресе проживания\n" +
                            "Хотите создать?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            adres = new RegistrationAddress();
                            wAddressAddRed add = new wAddressAddRed(adres, applicant, user_main);
                            add.ShowDialog();
                            applicant.Address = add.address.ID;
                            wAuth.center.SaveChanges();
                            txtAddress.Text = add.address.Localities.Name + ", " + add.address.Street + ", " +
                                add.address.Flat;
                            tlAdres.Content = txtAddress.Text;
                            
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

        private void txtSNILS_TextChanged(object sender, TextChangedEventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.Text))
            {
                if (txtSNILS.Text.Length >= 0 && txtSNILS.Text.Length < 11)
                {
                    SNILSCheck();
                }
                else if (txtSNILS.Text.Length == 11)
                {
                    if (txtSNILS.Text.Contains("-"))
                    {
                        SNILSCheck();
                    }
                    else
                    {
                        txtSNILS.Text = SNILSFormat(txtSNILS.Text);
                        applicant.SNILS = txtSNILS.Text;
                    }
                }
                else if(txtSNILS.Text.Length > 11 && txtSNILS.Text.Length < txtSNILS.MaxLength)
                {
                    SNILSCheck();
                }
                else if (txtSNILS.Text.Length == txtSNILS.MaxLength)
                {

                }
                else
                {
                    MessageBox.Show("Формат СНИЛС: XXX-XXX-XXX XX, где X - цифры от 0 до 9", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtSNILS.Text = "";
                    return;
                }
            }
            else
            {
                SNILSCheck();
            }
        }

        private string SNILSFormat(string SNILS)
        {
            string a = SNILS.Substring(0, 3);
            string b = SNILS.Substring(3, 3);
            string c = SNILS.Substring(6, 3);
            string d = SNILS.Substring(9, 2);
            return $"{a}-{b}-{c} {d}";
        }

        private void SNILSCheck()
        {
            if (txtSNILS.Text.Length == 3)
            {
                int sn;
                if (int.TryParse(txtSNILS.Text, out sn))
                {
                    txtSNILS.Text += "-";
                    txtSNILS.SelectionStart = txtSNILS.Text.Length;
                    txtSNILS.Focus();
                }
                else
                {
                    MessageBox.Show("Формат СНИЛС: XXX-XXX-XXX XX, где X - цифры от 0 до 9", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtSNILS.Text = "";
                    return;
                }
            }
            if (txtSNILS.Text.Length == 7)
            {
                int sn;
                if (int.TryParse(txtSNILS.Text.Substring(4, 3), out sn))
                {
                    txtSNILS.Text += "-";
                    txtSNILS.SelectionStart = txtSNILS.Text.Length;
                    txtSNILS.Focus();
                }
                else
                {
                    MessageBox.Show("Формат СНИЛС: XXX-XXX-XXX XX, где X - цифры от 0 до 9", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtSNILS.Text = "";
                    return;
                }
            }
            if (txtSNILS.Text.Length == 11)
            {
                int sn;
                if (int.TryParse(txtSNILS.Text.Substring(8, 3), out sn))
                {
                    txtSNILS.Text += " ";
                    txtSNILS.SelectionStart = txtSNILS.Text.Length;
                    txtSNILS.Focus();
                }
                else
                {
                    MessageBox.Show("Формат СНИЛС: XXX-XXX-XXX XX, где X - цифры от 0 до 9", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtSNILS.Text = "";
                    return;
                }
            }
        }

        MediaPlayer mp = new MediaPlayer();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mp.Close();
            if(user_main.Roles.Name == "Администратор")
            {

            }
            else
            {
                if (applicant.Address == null || applicant.RegistrationAddress == null
                || applicant.Passport == null || applicant.Passport1 == null)
                {
                    MessageBox.Show("Введите адрес проживания и паспортные данные",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                else
                {

                }
            }
        }

        private void cmbProf_DropDownOpened(object sender, EventArgs e)
        {
            string s = Environment.CurrentDirectory;
            if (s.Contains("Debug"))
            {
                s = s.Remove(s.Length - 10, 10);
            }
            if (s.Contains("EmploymentCenter"))
            {

            }
            mp.Open(new Uri($@"{s}" + @"\" + "music" + @"\" + "elevator.mp3"));
            mp.Volume = 0.15;
            mp.Play();
        }

        private void cmbProf_DropDownClosed(object sender, EventArgs e)
        {
            mp.Close();
        }
    }
}
