using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for PendingChequePrintRDLC.xaml
    /// </summary>
    public partial class PendingChequePrintRDLC : UserControl
    {
        public PendingChequePrintRDLC()
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

            var data = GetData(branchId, accountType);

            _reportviewer.LocalReport.ReportEmbeddedResource = "CPS.Views.Reports.Layout.PendingChequePrint.rdlc";
            _reportviewer.LocalReport.EnableExternalImages = true;

            var param = new ReportParameter[2];
            param[0] = new ReportParameter("BankName", string.Format("{0}", BankMasterDTO.GetBankName()));
            param[1] = new ReportParameter("Title", string.Format("Pending Cheque Print Report"));
            _reportviewer.LocalReport.SetParameters(param);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "PendingChequePrintReportDataSet";
            reportDataSource.Value = data;
            _reportviewer.LocalReport.DataSources.Clear();
            _reportviewer.LocalReport.DataSources.Add(reportDataSource);

            _reportviewer.RefreshReport();

        }
        
        private List<PendingChequePrint> GetData(int branchId, int accountType)
        {
            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                var repositoryAccountType = new PersistenceBase<AccountTypeDTO>(context);

                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                             && r.IsPrinted == false
                             orderby r.CreatedOn descending
                             select new PendingChequePrint
                             {
                                 SerialNo = r.SerialNo,
                                 AccountNoFull = r.AccountNoFull,
                                 Name = r.Name,
                                 BranchId = r.BranchId,
                                 BranchCode = r.BranchCode,
                                 BranchName = b.Name,
                                 TransactionCode = r.TransactionCode,
                                 ChequeNoFrom = r.ChequeFrom,
                                 ChequeNoTo = r.ChequeTo,
                                 NoOfCheque = r.NoOfCheque,
                                 NoOfChequeBook = r.NoOfChequeBook
                             });

                var response = query.ToList();
                if (response.Count == 0)
                {
                    return new List<PendingChequePrint>();
                }
                return response;
            }
        }
    }
    public class PendingChequePrint
    {
        public long SerialNo { get; set; }
        public string AccountNoFull { get; set; }
        public string Name { get; set; }
        public int BranchCode { get; set; }
        public int TransactionCode { get; set; }
        public int ChequeNoFrom { get; set; }
        public int ChequeNoTo { get; set; }
        public int NoOfCheque { get; set; }
        public string BranchName { get; set; }
        public int NoOfChequeBook { get; set; }
        public int BranchId { get; set; }

    }
}
