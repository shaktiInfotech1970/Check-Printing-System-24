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
    /// Interaction logic for ReprintChequeSinglePage.xaml
    /// </summary>
    public partial class ReprintedChequeSinglePage : UserControl
    {
        public ReprintedChequeSinglePage()
        {
            InitializeComponent();
            BindComboBox();
            btnPrint.IsEnabled = false;
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            cbAccountType.ItemsSource = AccountTypeDTO.GetLookups2();
            cbAccountType.DisplayMemberPath = "Value";
            cbAccountType.SelectedValuePath = "Key";
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
                var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
                var transactionfromDate = string.IsNullOrWhiteSpace(dtPrintDateFrom.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateFrom.Text);
                var transactiontoDate = string.IsNullOrWhiteSpace(dtPrintDateTo.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateTo.Text);

                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                             && DbFunctions.TruncateTime(ph.CreatedOn) >= transactionfromDate.Date && DbFunctions.TruncateTime(ph.CreatedOn) <= transactiontoDate.Date
                             && ph.PrintType == PrintType.RePrintCheque
                             select new { Request = r, AccountType = at, Branch = b, PrintHistory = ph, NoOfCheque = ph.ChequeNoTo - ph.ChequeNoFrom + 1 });

                var response = query.Where(o => o.NoOfCheque > 0).ToList();
                dgReprintedChequeSinglePage.ItemsSource = response;
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
            Paragraph title = new Paragraph(string.Format("Re-Printed Cheque Single Page:- From:{0}  - To:{1}", (dtPrintDateFrom.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy"), (dtPrintDateTo.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy")), new Font(Font.FontFamily.HELVETICA, 15));
            title.Alignment = Element.ALIGN_CENTER;

            ReportPDF report = new ReportPDF(dgReprintedChequeSinglePage, new float[] { 40, 90, 180, 50, 30, 55, 50, 30, 110, 50, 95 });
            report.Generate("ReprintedChequeSinglePage", title);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV file (*.csv)|*.csv";
            dlg.FileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            if (dlg.ShowDialog() == true)
            {
                ReportCSV report = new ReportCSV(dgReprintedChequeSinglePage);
                report.Generate(dlg.FileName);
            }
        }
    }
}