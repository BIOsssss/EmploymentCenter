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

namespace Центр_занятости.pages
{
    /// <summary>
    /// Логика взаимодействия для pRef.xaml
    /// </summary>
    public partial class pZayav : Page
    {
        public Users user;

        public int currentPage = 1;
        public int maxPage;
        public int count = 12;

        public pZayav(Users user)
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
            }
            if(user.Roles.Name == "Инспектор")
            {
                btnDel.Visibility = Visibility.Hidden;
            }
        }

        private List<ApplicationOfUnemployed> Update()
        {
            if (user.Roles.Name == "Соискатель")
            {
                var items = windows.wAuth.center.Applicants
                    .Where(p => p.ID_User == user.ID).ToList();
                Applicants applicant = new Applicants();
                foreach (var item in items)
                {
                    applicant = item;
                }
                grZayav.ItemsSource = windows.wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.ID_Applicant == applicant.ID).ToList();
                return windows.wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.ID_Applicant == applicant.ID).ToList();
            }
            else
            {
                grZayav.ItemsSource = windows.wAuth.center.ApplicationOfUnemployed.ToList();
                return windows.wAuth.center.ApplicationOfUnemployed.ToList();
            }
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var apl = grZayav.SelectedItem as ApplicationOfUnemployed;
                if (apl != null)
                {
                    windows.wApplicationAddRed red = new windows.wApplicationAddRed(apl, user);
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
            ApplicationOfUnemployed apl = new ApplicationOfUnemployed();
            windows.wApplicationAddRed add = new windows.wApplicationAddRed(apl, user);
            add.ShowDialog();
            Update();
            Refresh();

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = grZayav.SelectedItem as ApplicationOfUnemployed;
                if (item != null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить данные?", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        windows.wAuth.center.ApplicationOfUnemployed.Remove(item);
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
                return;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.DataContext as ApplicationOfUnemployed;
            string s = Environment.CurrentDirectory;
            s = s.Remove(s.Length - 10, 10);
            string path = $@"{s}//docs//application.dot";
            Word.Application app = new Word.Application();
            Word.Document doc = null;
            try
            {
                path = System.IO.Path.GetFullPath(path);
                doc = app.Documents.Add(path);

                Dictionary<string, string> docs = new Dictionary<string, string>
                {
                    ["<FIO>"] = $"{item.Applicants.FIO}",
                    ["<Adress>"] = $"{item.Applicants.RegistrationAddress.Adres}",
                    ["<Series>"] = $"{item.Applicants.Passport1.Series}",
                    ["<Number>"] = $"{item.Applicants.Passport1.Number}",
                    ["<Issued>"] = $"{item.Applicants.Passport1.Issued} " +
                    $"{item.Applicants.Passport1.IssDate.Date.ToShortDateString()}",
                    ["<Phone>"] = $"{item.Applicants.Phone}",
                    ["<Email>"] = $"{item.Applicants.Email}",
                    ["<Date>"] = $"{item.DateApplicant.ToLongDateString()}",
                    ["<AbbFIO>"] = $"{item.Applicants.AbbFIO}"
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
            grZayav.ItemsSource = listPage;
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

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var app = btn.DataContext as ApplicationOfUnemployed;
            windows.wApplicationAddRed info = new windows.wApplicationAddRed(app, user);
            info.ShowDialog();
        }
    }
}
