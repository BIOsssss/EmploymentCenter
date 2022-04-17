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
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.IO;

namespace Центр_занятости.pages
{
    /// <summary>
    /// Логика взаимодействия для pRef.xaml
    /// </summary>
    public partial class pRef : Page
    {
        public Users user;

        public int currentPage = 1;
        public int maxPage;
        public int count = 13;

        public pRef(Users user)
        {
            InitializeComponent();
            this.user = user;
            Update();
            Refresh();
            if (user.Roles.Name == "Соискатель")
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDel.Visibility = Visibility.Hidden;
                btnRed.Visibility = Visibility.Hidden;
                btnArch.Visibility = Visibility.Hidden;
                btnClear.Visibility = Visibility.Hidden;
            }
            if (user.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private List<ReferralToWork> Update()
        {
            if(user.Roles.Name == "Соискатель")
            {
                var items = windows.wAuth.center.Applicants
                    .Where(p => p.ID_User == user.ID).ToList();
                Applicants applicant = new Applicants();
                foreach (var item in items)
                {
                    applicant = item;
                }
                grRef.ItemsSource = windows.wAuth.center.ReferralToWork
                    .Where(p => p.ID_Applicant == applicant.ID).ToList();
                return windows.wAuth.center.ReferralToWork
                    .Where(p => p.ID_Applicant == applicant.ID).ToList();
            }
            else
            {
                if (click)
                {
                    grRef.ItemsSource = windows.wAuth.center.ReferralToWork
                    .Where(p => p.Hired == false).ToList();
                    return windows.wAuth.center.ReferralToWork
                    .Where(p => p.Hired == false).ToList();
                }
                else
                {
                    grRef.ItemsSource = windows.wAuth.center.ReferralToWork
                        .Where(p => p.Hired == true).ToList();
                    return windows.wAuth.center.ReferralToWork
                        .Where(p => p.Hired == true).ToList();
                }
            }
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reff = grRef.SelectedItem as ReferralToWork;
                if (reff != null)
                {
                    windows.wRefAddRed red = new windows.wRefAddRed(reff, user);
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
            ReferralToWork reff = new ReferralToWork();
            windows.wRefAddRed add = new windows.wRefAddRed(reff, user);
            add.ShowDialog();
            Update();
            Refresh();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grRef.SelectedItem as ReferralToWork;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.ReferralToWork.Remove(item);
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.DataContext as ReferralToWork;
            string s = Environment.CurrentDirectory;
            if (s.Contains("EmploymentCenter"))
            {

            }
            if (s.Contains("Debug"))
            {
                s = s.Remove(s.Length - 10, 10);
            }
            string path = $@"{s}\docs\reference.dot";
            Word.Application app = new Word.Application();
            Word.Document doc = null;
            try
            {
                path = System.IO.Path.GetFullPath(path);
                doc = app.Documents.Add(path);

                Dictionary<string, string> docs = new Dictionary<string, string>
                {
                    ["<FullNameCenter>"] = $"{item.Workers.Centers.FullName}",
                    ["<FullNameOrg>"] = $"{item.Organization.FullNameOrg}",
                    ["<Address>"] = $"{item.Organization.LegalAddress}",
                    ["<PhoneOrg>"] = $"{item.Organization.Phone}",
                    ["<ManagerOrg>"] = $"{item.Organization.ManagerOrg.FIO}",
                    ["<FIO>"] = $"{item.Applicants.FIO}",
                    ["<Spec>"] = $"{item.Vacancy.Header}",
                    ["<PhoneCenter>"] = $"{item.Workers.Centers.Phone}",
                    ["<DateStart>"] = $"{item.DateStart.ToShortDateString()}",
                    ["<DateFinish>"] = $"{item.DateFinish.ToShortDateString()}",
                    ["<Gender>"] = $"{item.Applicants.Gen}",
                    ["<NameCenter>"] = $"{item.Workers.Centers.Name}",
                    ["<Worker>"] = $"{item.Workers.FIO}",
                    ["<Gen>"] = $"{item.Applicants.Ge}"

                };

                foreach (var it in docs)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = it.Key;
                    find.Replacement.Text = it.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: Type.Missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: Type.Missing,
                        Replace: replace
                        );
                    ;
                }
                app.Visible = true;
                doc.Activate();
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message, "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //doc.Close();
                //app.Quit();
                //if (doc != null)
                //{
                //    Marshal.ReleaseComObject(doc);
                //}
                //if (app != null)
                //{
                //    Marshal.ReleaseComObject(app);
                //}
                //doc = null;
                //app = null;
                //GC.Collect();
            }
        }

        private void Refresh()
        {
            maxPage = (int)Math.Ceiling(Update().Count * 1.0 / count);
            var listPage = Update().Skip((currentPage - 1) * count).Take(count).ToList();
            txtCount.Text = currentPage.ToString();
            txtTotal.Text = maxPage.ToString();
            grRef.ItemsSource = listPage;
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

        public bool click = false;

        private void btnArch_Click(object sender, RoutedEventArgs e)
        {
            click = true;
            Update();
            Refresh();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            click = false;
            Update();
            Refresh();
        }
    }
}
