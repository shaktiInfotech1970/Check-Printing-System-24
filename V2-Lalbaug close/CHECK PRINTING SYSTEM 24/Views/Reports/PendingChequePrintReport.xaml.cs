using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for PendingChequePrintReport.xaml
    /// </summary>
    public partial class PendingChequePrintReport : UserControl
    {
        public PendingChequePrintReport()
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

                var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
                var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;

                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                             && r.IsPrinted == false
                             orderby r.CreatedOn descending
                             select new { Request = r, AccountType = at, Branch = b });

                var response = query.ToList();
                dgPendingChequePrintReport.ItemsSource = response;

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
            Paragraph title = new Paragraph("Pending Cheque Print Report", new Font(Font.FontFamily.HELVETICA, 15));
            title.Alignment = Element.ALIGN_CENTER;

            ReportPDF report = new ReportPDF(dgPendingChequePrintReport, new float[] { 50, 120, 250, 60, 40, 65, 60, 55, 70 });
            report.Generate("PendingChequePrint", title);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV file (*.csv)|*.csv";
            dlg.FileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            if (dlg.ShowDialog() == true)
            {
                ReportCSV report = new ReportCSV(dgPendingChequePrintReport);
                report.Generate(dlg.FileName);
            }
        }
    }
}
