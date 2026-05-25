using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for PrintedChequePrintFileRDLC.xaml
    /// </summary>
    public partial class PrintedChequePrintFileRDLC : UserControl
    {
        public PrintedChequePrintFileRDLC()
        {
            InitializeComponent();
            BindComboBox();            
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
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            RenderReport();
        }
        private void RenderReport()
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
            var transactionfromDate = string.IsNullOrWhiteSpace(dtPrintDateFrom.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateFrom.Text);
            var transactiontoDate = string.IsNullOrWhiteSpace(dtPrintDateTo.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtPrintDateTo.Text);

            var data = GetData(branchId, accountType, transactionfromDate, transactiontoDate);

            _reportviewer.LocalReport.ReportEmbeddedResource = "CPS.Views.Reports.Layout.PrintedChequePrintFile.rdlc";
            _reportviewer.LocalReport.EnableExternalImages = true;

            var param = new ReportParameter[2];
            param[0] = new ReportParameter("BankName", string.Format("{0}", BankMasterDTO.GetBankName()));
            param[1] = new ReportParameter("Title", string.Format("Printed Cheque (Print File):- From:{0}  - To:{1}", (dtPrintDateFrom.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy"), (dtPrintDateTo.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy")));
            _reportviewer.LocalReport.SetParameters(param);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "PrintedChequePrintFileReportDataSet";
            reportDataSource.Value = data;
            _reportviewer.LocalReport.DataSources.Clear();
            _reportviewer.LocalReport.DataSources.Add(reportDataSource);

            _reportviewer.RefreshReport();
        }
        private List<PrintedChequePrintFileReport> GetData(int branchId, int accountType, DateTime transactionfromDate, DateTime transactiontoDate)
        {
            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                var repositoryAccountType = new PersistenceBase<AccountTypeDTO>(context);
                var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);

                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                             && DbFunctions.TruncateTime(ph.CreatedOn) >= transactionfromDate.Date && DbFunctions.TruncateTime(ph.CreatedOn) <= transactiontoDate.Date
                             && ph.PrintType == PrintType.ChequeBook
                             select new PrintedChequePrintFileReport
                             {
                                 SerialNo = r.SerialNo,
                                 AccountNoFull = r.AccountNoFull,
                                 Name = r.Name,
                                 BranchId = r.BranchId,
                                 BranchCode = r.BranchCode,
                                 BranchName = b.Name,
                                 TransactionCode = r.TransactionCode,
                                 ChequeNoFrom = ph.ChequeNoFrom,
                                 ChequeNoTo = ph.ChequeNoTo,
                                 NoOfCheque = r.NoOfCheque,
                                 CreatedOn = ph.CreatedOn,
                                 CreatedBy = ph.CreatedBy,
                                 NoOfChequeBook = 1
                             });

                var response = query.ToList();
                if (response.Count == 0)
                {
                    return new List<PrintedChequePrintFileReport>();

                }
                return response;
            }
        }
    }
    public class PrintedChequePrintFileReport
    {
        public long SerialNo { get; set; }
        public string AccountNoFull { get; set; }
        public string Name { get; set; }
        public int BranchCode { get; set; }
        public int TransactionCode { get; set; }
        public int ChequeNoFrom { get; set; }
        public int ChequeNoTo { get; set; }
        public int NoOfCheque { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Sign { get; set; }
        public string BranchName { get; set; }
        public int NoOfChequeBook { get; set; }
        public int BranchId { get; set; }

    }

}
