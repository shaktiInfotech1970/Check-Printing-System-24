using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for PrintedChequeSeries.xaml
    /// </summary>
    public partial class PrintedChequeSeries : UserControl
    {
        public PrintedChequeSeries()
        {
            InitializeComponent();
            BindComboBox();
            btnPrint.IsEnabled = false;
        }

        private void BindComboBox()
        {
            var source = BranchMasterDTO.GetLookups();
            source.Insert(0, new LookupItem<int, string> { Key = 0, Value = "ALL" });
            cbBrach.ItemsSource = source;
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
        }

        private void btnShowColumns_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                var repositoryAccountType = new PersistenceBase<AccountTypeDTO>(context);
                var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);

                var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
                var transactionfromDate = string.IsNullOrWhiteSpace(dtPrintDateFrom.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateFrom.Text);
                var transactiontoDate = string.IsNullOrWhiteSpace(dtPrintDateTo.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateTo.Text);

                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && DbFunctions.TruncateTime(ph.CreatedOn) >= transactionfromDate.Date && DbFunctions.TruncateTime(ph.CreatedOn) <= transactiontoDate.Date
                             select new { r, b, at, ph });

                var groupedQuery = (from o in query
                                    group o by new { o.r.BranchId, o.b.Name, o.r.TransactionCode, AccountType = o.at.Name } into g
                                    select new
                                    {
                                        BranchName = g.Key.Name,
                                        TransactionCode = g.Key.TransactionCode,
                                        StartChequeNo = g.Min(min => min.r.ChequeFrom),
                                        EndChequeNo = g.Max(max => max.r.ChequeTo)
                                    });

                var response = groupedQuery.ToList();
                dgPrintedChequeSeries.ItemsSource = response;
                btnPrint.IsEnabled = true;
                btnExportCsv.IsEnabled = true;
                if (response.Count == 0)
                {
                    btnPrint.IsEnabled = false;
                    btnExportCsv.IsEnabled = false;
                    MessageBox.Show("No records found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Paragraph title = new Paragraph(string.Format("Printed Cheque Series:- From:{0}  - To:{1}", (dtPrintDateFrom.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy"), (dtPrintDateTo.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy")), new Font(Font.FontFamily.HELVETICA, 15));
            title.Alignment = Element.ALIGN_CENTER;

            ReportPDF report = new ReportPDF(dgPrintedChequeSeries, new float[] { 200, 75, 75, 75 });
            report.Generate("PrintedChequeSeries", title);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV file (*.csv)|*.csv";
            dlg.FileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            if (dlg.ShowDialog() == true)
            {
                ReportCSV report = new ReportCSV(dgPrintedChequeSeries);
                report.Generate(dlg.FileName);
            }
        }
    }
}