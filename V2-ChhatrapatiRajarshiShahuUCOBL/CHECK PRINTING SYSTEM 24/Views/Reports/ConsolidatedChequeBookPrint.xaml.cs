using CPS.Business;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for ConsolidatedChequeBookPrint.xaml
    /// </summary>
    public partial class ConsolidatedChequeBookPrint : UserControl
    {
        public ConsolidatedChequeBookPrint()
        {
            InitializeComponent();
            BindComboBox();
        }

        private void BindComboBox()
        {

            var source = BranchMasterDTO.GetLookups();
            source.Insert(0, new LookupItem<int, string> { Key = 0, Value = "ALL" });
            cbBrach.ItemsSource = source;
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            var source2 = AccountTypeDTO.GetLookups2();
            source2.Insert(0, new LookupItem<int, string> { Key = 0, Value = "ALL" });
            cbBrach.ItemsSource = source;
            cbAccountType.ItemsSource = source2;
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

            //var reportViewer = new ReportViewer();
            _reportviewer.LocalReport.ReportEmbeddedResource = "CPS.Views.Reports.Layout.ConsolidateChequeBookPrintReport.rdlc";
            _reportviewer.LocalReport.EnableExternalImages = true;

            var param = new ReportParameter[3];
            param[0] = new ReportParameter("From", transactionfromDate.ToShortDateString());
            param[1] = new ReportParameter("To", transactiontoDate.ToShortDateString());
            param[2] = new ReportParameter("BankName", "SHRI CHHATRAPATI RAJARSHI SHAHU URBAN CO-OP BANK LTD");
            _reportviewer.LocalReport.SetParameters(param);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ConsolidateChequeBookPrintReportDataSet";
            reportDataSource.Value = data;
            _reportviewer.LocalReport.DataSources.Clear();
            _reportviewer.LocalReport.DataSources.Add(reportDataSource);

            _reportviewer.RefreshReport();

        }
        private List<ConsolidateChequeBookPrintReport> GetData(int branchId, int accountType, DateTime transactionfromDate, DateTime transactiontoDate)
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
                             select new ConsolidateChequeBookPrintReport
                             {
                                 RequestId = r.Id,
                                 SrNo = r.SerialNo,
                                 AcctNo = r.AccountNoFull,
                                 AcctType = r.TransactionCode,
                                 AcctTypeName = at.Name,
                                 BookSize = r.NoOfCheque,
                                 BranchCode = b.Code,
                                 BranchName = b.Name,
                                 MICR = b.MICR,
                                 CheqNoFrom = r.ChequeFrom,
                                 CheqNoTo = r.ChequeTo,
                                 CustomerName = r.Name,
                                 NoOfBook = r.NoOfChequeBook,
                                 PrintDate = ph.CreatedOn,
                                 UserName = ph.CreatedBy,
                                 PrintJobNo = r.PrintJobNo ?? 0
                             });

                var response = query.ToList().Distinct(new DistinctConsolidateChequeBookPrintReportComparer()).OrderBy(o => o.BranchName).ToList();
                if (response.Count == 0)
                {
                    return new List<ConsolidateChequeBookPrintReport>();
                }
                return response;
            }
        }
    }

    public class ConsolidateChequeBookPrintReport
    {
        public int RequestId { get; set; }
        public long SrNo { get; set; }

        public string BranchCode { get; set; }
        public string MICR { get; set; }
        public int AcctType { get; set; }
        public string AcctNo { get; set; }
        public int CheqNoFrom { get; set; }
        public int CheqNoTo { get; set; }
        public int NoOfBook { get; set; }
        public int BookSize { get; set; }
        public string CustomerName { get; set; }
        public DateTime PrintDate { get; set; }
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public string AcctTypeName { get; set; }
        public int PrintJobNo { get; set; }
    }

    class DistinctConsolidateChequeBookPrintReportComparer : IEqualityComparer<ConsolidateChequeBookPrintReport>
    {

        public bool Equals(ConsolidateChequeBookPrintReport x, ConsolidateChequeBookPrintReport y)
        {
            return x.RequestId == y.RequestId;
        }

        public int GetHashCode(ConsolidateChequeBookPrintReport obj)
        {
            return obj.RequestId.GetHashCode();
        }
    }
}