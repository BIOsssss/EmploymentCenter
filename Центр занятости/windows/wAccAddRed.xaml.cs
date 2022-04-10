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
    /// Логика взаимодействия для wAccAddRed.xaml
    /// </summary>
    public partial class wAccAddRed : Window
    {
        public PaymentAccount account;
        public wAccAddRed(PaymentAccount account, Applicants app, Users user)
        {
            InitializeComponent();
            this.account = account;
            DataContext = account;
            if(user.Roles.Name == "Инспектор" ||
                user.Roles.Name == "Соискатель")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                if (account.NameBank == null)
                {
                    error += "Введите наименование банка\n";
                }
                if (account.BIK_Bank == null)
                {
                    error += "Введите БИК банка\n";
                }
                if (account.INN_Bank == null)
                {
                    error += "Введите ИНН банка\n";
                }
                if (account.CoresspodentAcc == null)
                {
                    error += "Введите корреспондентский счет\n";
                }
                if (account.AccountCitizen == null)
                {
                    error += "Введите счет соискателя\n";
                }
                if (error != "")
                {
                    MessageBox.Show(error, "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (account.ID == 0)
                    {
                        wAuth.center.PaymentAccount.Add(account);
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (account.ID == 0)
            {
                MessageBox.Show("Платежный счет не добавлен в базу данных, поэтому удаление невозможно",
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
                        wAuth.center.PaymentAccount.Remove(account);
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

        private void txtBIK_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(txtBIK.Text, out int value))
            {

            }
            else
            {
                MessageBox.Show("БИК банка состоит из цифр от 0 до 9",
                   "Внимание" , MessageBoxButton.OK, MessageBoxImage.Error);
                txtBIK.Text = "";
                return;
            }
        }

        private void txtINN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(float.TryParse(txtINN.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("ИНН банка состоит из цифр от 0 до 9",
   "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtINN.Text = "";
                return;
            }
        }

        private void txtCor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(float.TryParse(txtCor.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("Корреспондентский счет состоит из цифр от 0 до 9",
   "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCor.Text = "";
                return;
            }
        }

        private void txtAcc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(float.TryParse(txtAcc.Text, out float value))
            {

            }
            else
            {
                MessageBox.Show("Счет соискателя состоит из цифр от 0 до 9",
   "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAcc.Text = "";
                return;
            }
        }
    }
}
