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
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        public Search()
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

        private void btnShowColumns_Click(object sender, RoutedEventArgs e)
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
            string customerName = string.IsNullOrWhiteSpace(dtCustomerName.Text) ? "" : dtCustomerName.Text;
            string accountNo = string.IsNullOrWhiteSpace(dtAccountNo.Text) ? "" : dtAccountNo.Text;

            if (branchId == 0 && accountType == 0 && string.IsNullOrWhiteSpace(customerName) && string.IsNullOrWhiteSpace(accountNo))
            {
                MessageBox.Show("Please use any of the criteria for search!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                var repositoryAccountType = new PersistenceBase<AccountTypeDTO>(context);
                var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);
                var query = (from r in repositoryRequest.GetAll()
                             join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                             join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                             join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId into tmpPh
                             from ph in tmpPh.DefaultIfEmpty()
                             where
                                ((branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                             && (customerName.Length <= 0 || r.Name.Contains(customerName))
                             && (accountNo.Length <= 0 || r.AccountNoFull.Contains(accountNo))
                             )                             
                             select new { Request = r, AccountType = at, Branch = b, PrintHistory = ph });
                var response = query.ToList();
                dgDaywiseChequePrint.ItemsSource = response;
                if (response.Count == 0)
                {
                    MessageBox.Show("No records found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
