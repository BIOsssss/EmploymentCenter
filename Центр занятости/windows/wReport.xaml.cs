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
using Excel = Microsoft.Office.Interop.Excel;

namespace Центр_занятости.windows
{
    /// <summary>
    /// Логика взаимодействия для wReport.xaml
    /// </summary>
    public partial class wReport : Window
    {
        public wReport()
        {
            InitializeComponent();
            cmbCenter.ItemsSource = wAuth.center.Centers.ToList();
        }

        Dictionary<int, string> month = new Dictionary<int, string>()
        {
            [1] = "ЯНВАРЬ",
            [2] = "ФЕВРАЛЬ",
            [3] = "МАРТ",
            [4] = "АПРЕЛЬ",
            [5] = "МАЙ",
            [6] = "ИЮНЬ",
            [7] = "ИЮЛЬ",
            [8] = "АВГУСТ",
            [9] = "СЕНТЯБРЬ",
            [10] = "ОКТЯБРЬ",
            [11] = "НОЯБРЬ",
            [12] = "ДЕКАБРЬ"
        };



        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(cmbCenter.SelectedIndex == -1
                || dtStart.SelectedDate == null
                || dtFinish.SelectedDate == null)
            {
                MessageBox.Show("Выбирете центр занятости, дату начала и дату завершения формирования отчета",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                Centers center = cmbCenter.SelectedItem as Centers;
                //Excel
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workBook;
                workBook = excelApp.Workbooks.Add();

                //создание листов
                excelApp.SheetsInNewWorkbook = 2;

                Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.Worksheets.get_Item(2);
                worksheet1.Name = "Трудоустройство";

                Excel.Worksheet worksheet3 = (Excel.Worksheet)excelApp.Worksheets.get_Item(1);
                worksheet3.Name = "Соискатели";

                //Трудоустройство

                //Центр занятости
                int index1 = 1;
                Excel.Range czn = worksheet1.Range[worksheet1.Cells[index1, index1], worksheet1.Cells[index1 + 1, index1]];
                czn.Merge();
                czn.Font.Bold = true;
                czn.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                czn.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                worksheet1.Cells[index1, index1] = "Центр занятости";
                czn.EntireColumn.AutoFit();
                czn.EntireRow.AutoFit();

                index1 += 2;

                //заполнение ЦЗН
                worksheet1.Cells[index1, 1] = center.Name;
                Excel.Range czn_R = worksheet1.Range[worksheet1.Cells[1, 1], worksheet1.Cells[index1, 1]];
                czn_R.EntireColumn.AutoFit();
                czn_R.EntireRow.AutoFit();
                czn_R.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //месяца
                index1 = 2;

                for (int i = dtStart.SelectedDate.Value.Month; i <= dtFinish.SelectedDate.Value.Month; i++)
                {
                    Excel.Range m = worksheet1.Range[worksheet1.Cells[1, index1], worksheet1.Cells[1, index1 + 2]];
                    m.Merge();
                    m.Font.Bold = true;
                    worksheet1.Cells[1, index1] = month[i];
                    m.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    m.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    m.EntireColumn.AutoFit();
                    m.EntireRow.AutoFit();

                    int j = 2;
                    worksheet1.Cells[j, index1] = "Кол-во обращений";
                    j++;
                    var countObr = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.DateApplicant.Month >= i
                    && p.DateApplicant.Month <= i
                    && p.Workers.ID_Center == center.ID)
                    .ToList();
                    int obr = countObr.Count;
                    worksheet1.Cells[j, index1] = obr;

                    j = 2;
                    worksheet1.Cells[j, index1 + 1] = "Трудоустроились";
                    j++;
                    var countWork = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.DateApplicant.Month >= i
                    && p.DateApplicant.Month <= i
                    && p.Workers.ID_Center == center.ID
                    && p.StatusUnemployed.Name == "Трудоустроен").ToList();
                    int work = countWork.Count;
                    worksheet1.Cells[j, index1 + 1] = work;

                    j = 2;
                    worksheet1.Cells[j, index1 + 2] = "Отказано в пособии";
                    j++;
                    var countNotSt = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.DateApplicant.Month >= i
                    && p.DateApplicant.Month <= i
                    && p.Workers.ID_Center == center.ID
                    && p.StatusUnemployed.Name == "Отказано в пособии").ToList();
                    int notStipend = countNotSt.Count;
                    worksheet1.Cells[j, index1 + 2] = notStipend;

                    Excel.Range range = worksheet1.Range[worksheet1.Cells[1, index1], worksheet1.Cells[j, index1 + 2]];
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.EntireColumn.AutoFit();
                    range.EntireRow.AutoFit();
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    index1 += 3;
                }

                //Соискатели

                var man = wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID
                    && p.Applicants.Genders.Name == "Мужской").ToList();
                var fem = wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID
                    && p.Applicants.Genders.Name == "Женский").ToList();

                int sum = 0,
                    avg = 0;
                float male = 0,
                    female = 0;
                    ;
                foreach (var item in wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Workers.ID_Center == center.ID
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate))
                {
                    TimeSpan ts = DateTime.Today - item.Applicants.Birthday;
                    sum += ts.Days / 365;
                }
                if(wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Workers.ID_Center == center.ID
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).Count() == 0)
                {

                }
                else
                {
                    avg = sum / wAuth.center.ApplicationOfUnemployed
    .Where(p => p.Workers.ID_Center == center.ID
    && dtStart.SelectedDate <= p.DateApplicant
    && p.DateApplicant <= dtFinish.SelectedDate).Count();

                    male = (float)man.Count * 100 / wAuth.center.ApplicationOfUnemployed
                        .Where(p => p.Workers.ID_Center == center.ID
                        && dtStart.SelectedDate <= p.DateApplicant
                        && p.DateApplicant <= dtFinish.SelectedDate).Count();
                    female = (float)fem.Count * 100 / wAuth.center.ApplicationOfUnemployed
                        .Where(p => p.Workers.ID_Center == center.ID
                        && dtStart.SelectedDate <= p.DateApplicant
                        && p.DateApplicant <= dtFinish.SelectedDate).Count();
                }

                worksheet3.Cells[1, 1] = $"Период отчета: с {dtStart.SelectedDate.Value.ToLongDateString()} " +
                    $"по {dtFinish.SelectedDate.Value.ToLongDateString()}";

                worksheet3.Cells[3, 1] = $"Мужчин = {male:f} %";
                worksheet3.Cells[4, 1] = $"Женщин = {female:f} %";
                worksheet3.Cells[5, 1] = $"Средний возраст соискателей = {avg}";

                //диаграмма
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(5, 100, 250, 300);
                Excel.Chart chart = chartObject.Chart;

                var sr = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Среднее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID).ToList();
                var srp = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Среднее профессиональное"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID).ToList();
                var nv = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Неоконченное высшее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID).ToList();
                var v = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Высшее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Workers.ID_Center == center.ID).ToList();

                worksheet3.Cells[40, 1] = "Среднее";
                worksheet3.Cells[41, 1] = "Среднее профессиональное";
                worksheet3.Cells[42, 1] = "Неоконченное высшее";
                worksheet3.Cells[43, 1] = "Высшее";
                worksheet3.Cells[40, 2] = sr.Count;
                worksheet3.Cells[41, 2] = srp.Count;
                worksheet3.Cells[42, 2] = nv.Count;
                worksheet3.Cells[43, 2] = v.Count;

                Excel.Range rng = worksheet3.Range[worksheet3.Cells[40, 1], worksheet3.Cells[43, 2]];
                chart.SetSourceData(rng);
                chart.ChartType = Excel.XlChartType.xlPie;
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Уровень образования соискателей";
                chart.HasLegend = true;

                Dictionary<string, int> topVac = new Dictionary<string, int>();
                foreach (var item in wAuth.center.Professions.ToList())
                {
                    int count = 0;
                    foreach (var it in wAuth.center.Vacancy.ToList())
                    {
                        if (it.Specialization == item.ID
                            && dtStart.SelectedDate <= it.Date
                        && it.Date <= dtFinish.SelectedDate) count++;
                    }
                    topVac.Add(item.Name, count);
                }

                int n = 40;

                foreach (var item in topVac.OrderByDescending(p => p.Value))
                {
                    if (n > 44)
                    {
                        break;
                    }
                    else
                    {
                        worksheet3.Cells[n, 3] = item.Key;
                        worksheet3.Cells[n, 4] = item.Value;
                        n++;
                    }
                }

                worksheet3.Cells[39, 4] = "Кол-во ваканский";

                Excel.ChartObjects chart1Objects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chart1Object = chart1Objects.Add(255, 100, 250, 300);
                Excel.Chart chart1 = chart1Object.Chart;

                Excel.Range rngV = worksheet3.Range[worksheet3.Cells[39, 3], worksheet3.Cells[44, 4]];
                chart1.SetSourceData(rngV);
                chart1.ChartType = Excel.XlChartType.xlColumnClustered;
                chart1.HasTitle = true;
                chart1.ChartTitle.Text = "ТОП 5 профессий по вакансиям";
                chart1.HasLegend = true;

                Dictionary<string, int> topApp = new Dictionary<string, int>();
                foreach (var item in wAuth.center.Professions.ToList())
                {
                    int count = 0;
                    foreach (var it in wAuth.center.ApplicationOfUnemployed.ToList())
                    {
                        if (it.Applicants.Profession == item.ID
                            && dtStart.SelectedDate <= it.DateApplicant
                        && it.DateApplicant <= dtFinish.SelectedDate) count++;
                    }
                    topApp.Add(item.Name, count);
                }

                n = 40;

                foreach (var item in topApp.OrderByDescending(p => p.Value))
                {
                    if (n > 44)
                    {
                        break;
                    }
                    else
                    {
                        worksheet3.Cells[n, 6] = item.Key;
                        worksheet3.Cells[n, 7] = item.Value;
                        n++;
                    }
                }

                worksheet3.Cells[39, 7] = "Кол-во соискателей";

                Excel.ChartObjects chart2Objects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chart2Object = chart2Objects.Add(500, 100, 250, 300);
                Excel.Chart chart2 = chart2Object.Chart;

                Excel.Range rngA = worksheet3.Range[worksheet3.Cells[39, 6], worksheet3.Cells[44, 7]];
                chart2.SetSourceData(rngA);
                chart2.ChartType = Excel.XlChartType.xl3DBarStacked;
                chart2.HasTitle = true;
                chart2.ChartTitle.Text = "ТОП 5 профессий среди соискателей";
                chart2.HasLegend = true;

                //включаемся
                excelApp.WindowState = Excel.XlWindowState.xlMaximized;
                excelApp.Visible = true;
                excelApp.UserControl = true;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtStart.SelectedDate = new DateTime(DateTime.Now.Year,1,1);
            dtFinish.SelectedDate = DateTime.Now;

            if (cmbCenter.SelectedIndex == -1)
            {
                btnOK.IsEnabled = false;
            }
            else
            {
                btnOK.IsEnabled = true;
            }
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            if(dtStart.SelectedDate == null || 
                dtFinish.SelectedDate == null)
            {
                MessageBox.Show("Выбирете дату начала и дату завершения формирования отчета",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //Excel
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workBook;
                workBook = excelApp.Workbooks.Add();

                //создание листов
                excelApp.SheetsInNewWorkbook = 2;

                Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.Worksheets.get_Item(2);
                worksheet1.Name = "Трудоустройство";

                Excel.Worksheet worksheet3 = (Excel.Worksheet)excelApp.Worksheets.get_Item(1);
                worksheet3.Name = "Соискатели";

                //Трудоустройство

                //Центр занятости
                int index1 = 1;
                Excel.Range czn = worksheet1.Range[worksheet1.Cells[index1, index1], worksheet1.Cells[index1 + 1, index1]];
                czn.Merge();
                czn.Font.Bold = true;
                czn.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                czn.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                worksheet1.Cells[index1, index1] = "Центр занятости";
                czn.EntireColumn.AutoFit();
                czn.EntireRow.AutoFit();

                index1 += 2;

                //заполнение ЦЗН
                foreach (var item in wAuth.center.Centers.ToList())
                {
                    worksheet1.Cells[index1, 1] = item.Name;
                    index1++;
                }
                Excel.Range czn_R = worksheet1.Range[worksheet1.Cells[1, 1], worksheet1.Cells[index1 - 1, 1]];
                czn_R.EntireColumn.AutoFit();
                czn_R.EntireRow.AutoFit();
                czn_R.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //месяца
                index1 = 2;

                int k = 0;

                for (int i = dtStart.SelectedDate.Value.Month; i <= dtFinish.SelectedDate.Value.Month; i++)
                {
                    Excel.Range m = worksheet1.Range[worksheet1.Cells[1, index1], worksheet1.Cells[1, index1 + 2]];
                    m.Merge();
                    m.Font.Bold = true;
                    worksheet1.Cells[1, index1] = month[i];
                    m.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    m.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    m.EntireColumn.AutoFit();
                    m.EntireRow.AutoFit();

                    int j = 2;
                    worksheet1.Cells[j, index1] = "Кол-во обращений";
                    foreach (var item in wAuth.center.Centers)
                    {
                        j++;
                        var countObr = wAuth.center.ApplicationOfUnemployed
                        .Where(p => p.DateApplicant.Month >= i
                        && p.DateApplicant.Month <= i
                        && p.Workers.ID_Center == item.ID)
                        .ToList();
                        int obr = countObr.Count;
                        worksheet1.Cells[j, index1] = obr;
                    }
                    Excel.Range c = worksheet1.Range[worksheet1.Cells[3, index1], worksheet1.Cells[j, index1]];
                    string adres = c.get_Address(1, 1, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                    string[] str = adres.Split('$');
                    adres = "";
                    foreach (string s in str)
                    {
                        adres += s;
                    }
                    worksheet1.Cells[j + 1, index1].Formula = $"=SUM({adres})";
                    worksheet1.Cells[j + 1, index1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    j = 2;
                    worksheet1.Cells[j, index1 + 1] = "Трудоустроились";
                    foreach (var item in wAuth.center.Centers)
                    {
                        j++;
                        var countWork = wAuth.center.ApplicationOfUnemployed
                        .Where(p => p.DateApplicant.Month >= i
                        && p.DateApplicant.Month <= i
                        && p.Workers.ID_Center == item.ID
                        && p.StatusUnemployed.Name == "Трудоустроен").ToList();
                        int work = countWork.Count;
                        worksheet1.Cells[j, index1 + 1] = work;
                    }
                    Excel.Range w = worksheet1.Range[worksheet1.Cells[3, index1 + 1], worksheet1.Cells[j, index1 + 1]];
                    adres = w.get_Address(1, 1, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                    str = adres.Split('$');
                    adres = "";
                    foreach (string s in str)
                    {
                        adres += s;
                    }
                    worksheet1.Cells[j + 1, index1 + 1].Formula = $"=SUM({adres})";
                    worksheet1.Cells[j + 1, index1 + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    j = 2;
                    worksheet1.Cells[j, index1 + 2] = "Отказано в пособии";
                    foreach (var item in wAuth.center.Centers)
                    {
                        j++;
                        var countNotSt = wAuth.center.ApplicationOfUnemployed
                            .Where(p => p.DateApplicant.Month >= i
                            && p.DateApplicant.Month <= i
                            && p.Workers.ID_Center == item.ID
                            && p.StatusUnemployed.Name == "Отказано в пособии").ToList();
                        int notStipend = countNotSt.Count;
                        worksheet1.Cells[j, index1 + 2] = notStipend;
                    }
                    Excel.Range o = worksheet1.Range[worksheet1.Cells[3, index1 + 2], worksheet1.Cells[j, index1 + 2]];
                    adres = o.get_Address(1, 1, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                    str = adres.Split('$');
                    adres = "";
                    foreach (string s in str)
                    {
                        adres += s;
                    }
                    worksheet1.Cells[j + 1, index1 + 2].Formula = $"=SUM({adres})";
                    worksheet1.Cells[j + 1, index1 + 2].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    Excel.Range range = worksheet1.Range[worksheet1.Cells[1, index1], worksheet1.Cells[j, index1 + 2]];
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.EntireColumn.AutoFit();
                    range.EntireRow.AutoFit();
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    index1 += 3;

                    worksheet1.Cells[j + 1, 1] = "ИТОГО:";
                    worksheet1.Cells[j + 1, 1].Font.Bold = true;
                    worksheet1.Cells[j + 1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    worksheet1.Cells[j + 1, 1].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    worksheet1.Cells[j + 1, 1].EntireColumn.AutoFit();
                    worksheet1.Cells[j + 1, 1].EntireRow.AutoFit();
                    k = j;
                }

                //Соискатели

                var man = wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Applicants.Genders.Name == "Мужской").ToList();
                var fem = wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate
                    && p.Applicants.Genders.Name == "Женский").ToList();

                int sum = 0;
                foreach (var item in wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).ToList())
                {
                    TimeSpan ts = DateTime.Today - item.Applicants.Birthday;
                    sum += ts.Days / 365;
                }
                int avg = sum / wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).Count();

                float male = (float)man.Count * 100 / wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).Count();
                float female = (float)fem.Count * 100 / wAuth.center.ApplicationOfUnemployed
                    .Where(p => dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).Count();

                worksheet3.Cells[1, 1] = $"Период отчета: с {dtStart.SelectedDate.Value.ToLongDateString()} " +
                    $"по {dtFinish.SelectedDate.Value.ToLongDateString()}";
                
                worksheet3.Cells[3, 1] = $"Мужчин = {male:f} %";
                worksheet3.Cells[4, 1] = $"Женщин = {female:f} %";
                worksheet3.Cells[5, 1] = $"Средний возраст соискателей = {avg}";

                //диаграмма
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(5, 100, 250, 300);
                Excel.Chart chart = chartObject.Chart;

                var sr = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Среднее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).ToList();
                var srp = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Среднее профессиональное"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).ToList();
                var nv = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Неоконченное высшее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).ToList();
                var v = wAuth.center.ApplicationOfUnemployed
                    .Where(p => p.Applicants.Educations.Name == "Высшее"
                    && dtStart.SelectedDate <= p.DateApplicant
                    && p.DateApplicant <= dtFinish.SelectedDate).ToList();

                worksheet3.Cells[40, 1] = "Среднее";
                worksheet3.Cells[41, 1] = "Среднее профессиональное";
                worksheet3.Cells[42, 1] = "Неоконченное высшее";
                worksheet3.Cells[43, 1] = "Высшее";
                worksheet3.Cells[40, 2] = sr.Count;
                worksheet3.Cells[41, 2] = srp.Count;
                worksheet3.Cells[42, 2] = nv.Count;
                worksheet3.Cells[43, 2] = v.Count;

                Excel.Range rng = worksheet3.Range[worksheet3.Cells[40, 1], worksheet3.Cells[43, 2]];
                chart.SetSourceData(rng);
                chart.ChartType = Excel.XlChartType.xlPie;
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Уровень образования соискателей";
                chart.HasLegend = true;

                Dictionary<string, int> topVac = new Dictionary<string, int>();
                foreach(var item in wAuth.center.Professions.ToList())
                {
                    int count = 0;
                    foreach(var it in wAuth.center.Vacancy.ToList())
                    {
                        if (it.Specialization == item.ID
                            && dtStart.SelectedDate <= it.Date
                        && it.Date <= dtFinish.SelectedDate) count++;
                    }
                    topVac.Add(item.Name, count);
                }

                int n = 40;

                foreach (var item in topVac.OrderByDescending(p => p.Value))
                {
                    if(n > 44)
                    {
                        break;
                    }
                    else
                    {
                        worksheet3.Cells[n, 3] = item.Key;
                        worksheet3.Cells[n, 4] = item.Value;
                        n++;
                    }
                }

                worksheet3.Cells[39, 4] = "Кол-во ваканский";

                Excel.ChartObjects chart1Objects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chart1Object = chart1Objects.Add(255, 100, 250, 300);
                Excel.Chart chart1 = chart1Object.Chart;

                Excel.Range rngV = worksheet3.Range[worksheet3.Cells[39, 3], worksheet3.Cells[44, 4]];
                chart1.SetSourceData(rngV);
                chart1.ChartType = Excel.XlChartType.xlColumnClustered;
                chart1.HasTitle = true;
                chart1.ChartTitle.Text = "ТОП 5 профессий по вакансиям";
                chart1.HasLegend = true;

                Dictionary<string, int> topApp = new Dictionary<string, int>();
                foreach (var item in wAuth.center.Professions.ToList())
                {
                    int count = 0;
                    foreach (var it in wAuth.center.ApplicationOfUnemployed.ToList())
                    {
                        if (it.Applicants.Profession == item.ID
                            && dtStart.SelectedDate <= it.DateApplicant
                        && it.DateApplicant <= dtFinish.SelectedDate) count++;
                    }
                    topApp.Add(item.Name, count);
                }

                n = 40;

                foreach (var item in topApp.OrderByDescending(p => p.Value))
                {
                    if (n > 44)
                    {
                        break;
                    }
                    else
                    {
                        worksheet3.Cells[n, 6] = item.Key;
                        worksheet3.Cells[n, 7] = item.Value;
                        n++;
                    }
                }

                worksheet3.Cells[39, 7] = "Кол-во соискателей";

                Excel.ChartObjects chart2Objects = (Excel.ChartObjects)worksheet3.ChartObjects();
                Excel.ChartObject chart2Object = chart2Objects.Add(500, 100, 250, 300);
                Excel.Chart chart2 = chart2Object.Chart;

                Excel.Range rngA = worksheet3.Range[worksheet3.Cells[39, 6], worksheet3.Cells[44, 7]];
                chart2.SetSourceData(rngA);
                chart2.ChartType = Excel.XlChartType.xl3DBarStacked;
                chart2.HasTitle = true;
                chart2.ChartTitle.Text = "ТОП 5 профессий среди соискателей";
                chart2.HasLegend = true;

                //включаемся
                excelApp.WindowState = Excel.XlWindowState.xlMaximized;
                excelApp.Visible = true;
                excelApp.UserControl = true;
            }
        }

        private void cmbCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCenter.SelectedIndex == -1)
            {
                btnOK.IsEnabled = false;
            }
            else
            {
                btnOK.IsEnabled = true;
            }
        }
    }
}
