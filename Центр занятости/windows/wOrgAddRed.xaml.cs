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
using Microsoft.Win32;
using System.IO;
using System.Drawing;

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wOrgAddRed.xaml
    /// </summary>
    public partial class wOrgAddRed : Window
    {
        public Organization org;
        public ManagerOrg manager;
        public wOrgAddRed(Organization org, ManagerOrg manager, Users user)
        {
            InitializeComponent();
            this.org = org;
            this.manager = manager;
            DataContext = org;
            this.org.ID_Manager = manager.ID;
            cmbType.ItemsSource = wAuth.center.TypeOrganization.ToList();

            if(user.Roles.Name == "Соискатель")
            {
                txtOGRN.IsReadOnly = true;
                txtAbb.IsReadOnly = true;
                txtAddress.IsReadOnly = false;
                txtFull.IsReadOnly = true;
                txtINN.IsReadOnly = true;
                txtKPP.IsReadOnly = true;
                txtPhone.IsReadOnly = true;
                cmbType.IsEnabled = false;
                dateReg.IsEnabled = false;
                btnClear.Visibility = Visibility.Hidden;
                btnDel.Visibility = Visibility.Hidden;
                btnImg.Visibility = Visibility.Hidden;
                bntOK.Visibility = Visibility.Hidden;
            }
            if(user.Roles.Name == "Менеджер организации")
            {
                if(user.ID != manager.ID_User)
                {
                    txtOGRN.IsReadOnly = true;
                    txtAbb.IsReadOnly = true;
                    txtAddress.IsReadOnly = false;
                    txtFull.IsReadOnly = true;
                    txtINN.IsReadOnly = true;
                    txtKPP.IsReadOnly = true;
                    txtPhone.IsReadOnly = true;
                    cmbType.IsEnabled = false;
                    dateReg.IsEnabled = false;
                    btnClear.Visibility = Visibility.Hidden;
                    btnDel.Visibility = Visibility.Hidden;
                    btnImg.Visibility = Visibility.Hidden;
                    bntOK.Visibility = Visibility.Hidden;
                }
                else
                {

                }
            }
            if(user.Roles.Name != "Администратор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
            try
            {
                if (org.Image == null)
                {

                }
                else
                {
                    string file = org.Image;
                    string path = $@"{Environment.CurrentDirectory}{file}";
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists)
                    {
                        img.Source = new BitmapImage(new Uri(path));
                    }
                    btnImg.Visibility = Visibility.Hidden;

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
            try
            {
                if (org.ID == 0)
                {
                    dateReg.SelectedDate = DateTime.Now;
                }
                if (org.Image == null)
                {

                }
                else
                {
                    string file = org.Image;
                    string path = $@"{Environment.CurrentDirectory}{file}";
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists)
                    {
                        img.Source = new BitmapImage(new Uri(path));
                    }
                    btnImg.Visibility = Visibility.Hidden;
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
            if (org.ID == 0)
            {
                MessageBox.Show("Организация не зарегистрирована, поэтому удаление невозможно",
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
                        wAuth.center.Organization.Remove(org);
                        wAuth.center.SaveChanges();
                        MessageBox.Show("Успешно удалено", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
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
                if(cmbType.SelectedIndex == -1)
                {
                    error += "Выберите тип организации\n";
                }
                if (org.AbbreviatedName == null)
                {
                    error += "Введите аббревиатуру организации\n";
                }
                if (org.FullNameOrg == null)
                {
                    error += "Введите полное наименование организации\n";
                }
                if (org.DateRegistration.Date >= DateTime.Now.Date)
                {
                    error += "Организация должна быть создана ранее текущего дня\n";
                    dateReg.SelectedDate = DateTime.Now;
                }
                if (org.INN == null)
                {
                    error += "Ввдите ИНН организации\n";
                }
                if (org.KPP == null && org.TypeOrg == 1)
                {
                    error += "Введите КПП организации\n";
                }
                if(org.TypeOrg == 2 && org.KPP == null && org.OGRN == null)
                {
                    error += "Введите ОГРН организации\n";
                }
                if (org.LegalAddress == null)
                {
                    error += "Введите юр. адрес организации\n";
                }
                if (org.Phone == null)
                {
                    error += "Введите контактный номер телефона\n";
                }
                else
                {
                    if ((org.Phone[0] == '+' && org.Phone[1] == '7') ||
                        org.Phone[0] == '8')
                    {
                        foreach (char c in org.Phone)
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
                if (org.ID == 0)
                {
                    foreach (var x in wAuth.center.Organization)
                    {
                        if (x.TypeOrg == org.TypeOrg &&
                            (x.AbbreviatedName == org.AbbreviatedName ||
                            x.FullNameOrg == org.FullNameOrg))
                        {
                            error += "Такая организация уже существует, введите другую\n";
                        }
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
                    if (org.ID == 0)
                    {
                        wAuth.center.Organization.Add(org);
                    }
                    wAuth.center.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbType.SelectedIndex == 0)
            {
                txtINN.MaxLength = 10;
                txtOGRN.MaxLength = 13;
            }
            if (cmbType.SelectedIndex == 1)
            {
                txtINN.MaxLength = 12;
                txtOGRN.MaxLength = 15;
                txtAbb.Text = $"ИП {manager.FIO}";
                txtFull.Text = $"Индивидуальный предприниматель {manager.FIO}";
                org.AbbreviatedName = txtAbb.Text;
                org.FullNameOrg = txtFull.Text;
            }
        }

        private void txtINN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtINN.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("Введите 10 (либо 12) цифр ИНН",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtINN.Text = "";
                return;
            }
        }

        private void txtKPP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtKPP.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("Введите 9 цифр КПП",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtKPP.Text = "";
                return;
            }
        }

        private void btnImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = "Изображение (*.jpg,*.png)|*.jpg;*.png|Все файлы (*.*)|*.*"
                };
                if (dialog.ShowDialog() != true) { return; }
                string file = dialog.FileName;
                img.Source = new BitmapImage(new Uri(file));
                string[] f = file.Split('\\');
                string fl = f[f.Length - 1];
                string fImg = $@"\image\{fl}";
                string path = $@"{Environment.CurrentDirectory}{fImg}";
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                {
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists == false)
                    {
                        fileInfo.CopyTo(path);
                    }
                    else
                    {
                        btnImg.Visibility = Visibility.Hidden;
                        org.Image = fImg;
                        return;
                    }
                    btnImg.Visibility = Visibility.Hidden;
                    org.Image = fImg;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnImg_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string file = files[0].ToString();
                img.Source = new BitmapImage(new Uri(file));
                string[] f = file.Split('\\');
                string fl = f[f.Length - 1];
                string fImg = $@"\image\{fl}";
                string path = $@"{Environment.CurrentDirectory}{fImg}";
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                {
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists == false)
                    {
                        fileInfo.CopyTo(path);
                    }
                    else
                    {
                        btnImg.Visibility = Visibility.Hidden;
                        org.Image = fImg;
                        return;
                    }
                    btnImg.Visibility = Visibility.Hidden;
                    org.Image = fImg;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            img.Source = null;
            org.Image = null;
            btnImg.Visibility = Visibility.Visible;
        }

        private void txtKPP_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(org.TypeOrg == 1)
            {
                if(txtINN.Text != null && txtKPP.Text == "")
                {
                    txtKPP.Text = $"{txtINN.Text.Substring(0, 5)}1001";
                    org.KPP = txtKPP.Text;
                }
            }
        }

        private void txtFull_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(org.TypeOrg == 1)
            {
                if(txtFull.Text == "" && txtAbb.Text != null)
                {
                    string[] mas = txtAbb.Text.Split('\"');
                    if (txtAbb.Text.Contains("МУП"))
                    {
                        txtFull.Text += $"Муниципальное унитарное предприятие ";
                    }
                    if (txtAbb.Text.Contains("ИФНС"))
                    {
                        txtFull.Text += $"Межрайонная Инспекция Федеральной налоговой службы ";
                    }
                    if (txtAbb.Text.Contains("УФПС"))
                    {
                        string v = mas[0].Remove(0, 5);
                        if (v.Contains("АО"))
                        {
                            mas[0] = v.Substring(0, v.Length - 4);
                        }
                        else { }
                        txtFull.Text += $"Управление федеральной почтовой связи {mas[0]} ";
                    }
                    if (txtAbb.Text.Contains("ООО"))
                    {
                        string v = mas[0];
                        if (v.Contains("НПО"))
                        {
                            txtFull.Text += $"Общество с ограниченной ответственностью ";
                        }
                        else
                        {
                            txtFull.Text += $"Общество с ограниченной ответственностью \"{mas[1]}\"";
                        }
                    }
                    if (txtAbb.Text.Contains("ОАО"))
                    {
                        txtFull.Text += $"Открытое акционерное общество \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("АО"))
                    {
                        txtFull.Text += $"Акционерное общество \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ФУ"))
                    {
                        string v = mas[0].Remove(0, 3);
                        txtFull.Text += $"Финансовое управление {mas[0]} ";
                    }
                    if (txtAbb.Text.Contains("НПО"))
                    {
                        txtFull.Text += $"Научно-производственное объединение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МБУ"))
                    {
                        txtFull.Text += $"Муниципальное бюджетное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ФГКУ"))
                    {
                        txtFull.Text += $"Федеральное государственное казенное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ГБОУ"))
                    {
                        txtFull.Text += $"Государственное бюджетное общеобразовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МКОУ"))
                    {
                        txtFull.Text += $"Муниципальное казённое общеобразовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МКУ"))
                    {
                        txtFull.Text += $"Муниципальное казённое учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ПАО"))
                    {
                        txtFull.Text += $"Публичное акционерное общество \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МКДОУ"))
                    {
                        txtFull.Text += $"Муниципальное казенное дошкольное образование учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МОУ"))
                    {
                        txtFull.Text += $"Муниципальное общеобразовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ГАУЗ"))
                    {
                        txtFull.Text += $"Государственое автономное учреждение здравоохранения \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МАОУ"))
                    {
                        txtFull.Text += $"Муниципальное автономное общеобразовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МАДОУ"))
                    {
                        txtFull.Text += $"Муниципальное автономное дошкольное образовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ОБУ"))
                    {
                        txtFull.Text += $"Областное бюджетное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МБДОУ"))
                    {
                        txtFull.Text += $"Муниципальное бюджетное дошкольное образовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("МБОУ"))
                    {
                        txtFull.Text += $"Муниципальное бюджетное общеобразовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ЗАО"))
                    {
                        txtFull.Text += $"Закрытое акционерное общество \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ЧФСУ"))
                    {
                        txtFull.Text += $"Частное фикзультурно-спортивное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ГБПОУ"))
                    {
                        txtFull.Text += $"Государственное бюджетное профессиональное образовательное учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ОГКУ"))
                    {
                        txtFull.Text += $"Областное государственное казённое учреждение \"{mas[1]}\"";
                    }
                    if (txtAbb.Text.Contains("ФГУП"))
                    {
                        txtFull.Text += $"Федеральное государственное унитарное предприятие \"{mas[1]}\"";
                    }
                    org.FullNameOrg = txtFull.Text;
                }
            }
        }
    }
}
