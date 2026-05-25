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
    /// Interaction logic for DayWiseChequePrintRDLC.xaml
    /// </summary>
    public partial class DaywiseChequePrintRDLC : UserControl
    {
        public DaywiseChequePrintRDLC()
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
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            RenderReport();
        }
        private void RenderReport()
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
            var transactionDate = string.IsNullOrWhiteSpace(dtTransactionDate.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtTransactionDate.Text);

            var data = GetData(branchId, accountType, transactionDate);

            _reportviewer.LocalReport.ReportEmbeddedResource = "CPS.Views.Reports.Layout.DaywiseChequePrint.rdlc";
            _reportviewer.LocalReport.EnableExternalImages = true;

            var param = new ReportParameter[2];            
            param[0] = new ReportParameter("BankName", string.Format("{0}", BankMasterDTO.GetBankName()));            
            param[1] = new ReportParameter("Title", string.Format("Daywise Cheque Print:- Transaction Date: {0}", (dtTransactionDate.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy")));
            _reportviewer.LocalReport.SetParameters(param);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DaywiseChequePrintReportDataSet";
            reportDataSource.Value = data;
            _reportviewer.LocalReport.DataSources.Clear();
            _reportviewer.LocalReport.DataSources.Add(reportDataSource);

            _reportviewer.RefreshReport();

        }
        private List<DaywiseChequePrintReport> GetData(int branchId, int accountType, DateTime transactionDate)
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
                             && DbFunctions.TruncateTime(ph.CreatedOn) == transactionDate.Date
                             && ph.PrintType == PrintType.ChequeBook
                             select new DaywiseChequePrintReport
                             {
                                 SerialNo = r.SerialNo,
                                 AccountNoFull = r.AccountNoFull,
                                 Name = r.Name,
                                 BranchCode = r.BranchCode,
                                 BranchId = r.BranchId,
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
                    return new List<DaywiseChequePrintReport>();
                }
                return response;
            }

        }
    }
    public class DaywiseChequePrintReport
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
