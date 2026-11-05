using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace CPS.Views.Process
{
    /// <summary>
    /// Interaction logic for RemoveRequestData.xaml
    /// </summary>
    public partial class RemoveRequestData : UserControl
    {
        public RemoveRequestData()
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
        public int? ToIntNullable(string s, int? defaultValue = null) => int.TryParse(s, out var v) ? v : defaultValue;


        private void btnRemoveRequest_Click(object sender, RoutedEventArgs e)
        {


            if (dgRemoveRequestData.ItemsSource != null)
            {
                var requests = dgRemoveRequestData.ItemsSource.Cast<PrintRequestAndHistory>().Where(w => w.Request.IsSelected);
                if (requests.Count() <= 0)
                {
                    MessageBox.Show("Please select request(s) to remove", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to remove record(s)?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)  // error is here
                    {
                        RemoveRecord(requests);
                        Clear();
                        MessageBox.Show("Selected record(s) removed successfully.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("There is no record to remove.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnShowColumns_Click(object sender, RoutedEventArgs e)
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
            string customerName = string.IsNullOrWhiteSpace(dtCustomerName.Text) ? "" : dtCustomerName.Text;
            string accountNo = string.IsNullOrWhiteSpace(dtAccountNo.Text) ? "" : dtAccountNo.Text;
            int? printJobNo = ToIntNullable(dtJobNo.Text, null);

            System.DateTime? printedDate = string.IsNullOrWhiteSpace(dtPrintedDate.Text) ? null : (DateTime?)System.Convert.ToDateTime(dtPrintedDate.Text);
            System.DateTime? requestedDate = string.IsNullOrWhiteSpace(dtImportedDate.Text) ? null : (DateTime?)System.Convert.ToDateTime(dtImportedDate.Text);

            if (branchId == 0 && accountType == 0 && string.IsNullOrWhiteSpace(customerName) && string.IsNullOrWhiteSpace(accountNo) && (!printJobNo.HasValue)
                && (!requestedDate.HasValue)
                && (!printedDate.HasValue))
            {
                MessageBox.Show("Please use any of the criteria to search request data!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                             && ((!printJobNo.HasValue) || (printJobNo.HasValue && r.PrintJobNo == printJobNo.Value))
                             && ((!requestedDate.HasValue) || (DbFunctions.TruncateTime(r.CreatedOn) == requestedDate))
                             && ((!printedDate.HasValue) || (DbFunctions.TruncateTime(ph.CreatedOn) == printedDate))
                             )
                             select new PrintRequestAndHistory { Request = r, AccountType = at, Branch = b, PrintHistory = ph });

                var response = query.ToList();
                dgRemoveRequestData.ItemsSource = response;
                if (response.Count == 0)
                {
                    MessageBox.Show("No records found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        private void ToggleDGCheckbox(object sender, RoutedEventArgs e)
        {
            var printRequestAndHistory = dgRemoveRequestData.Items.OfType<PrintRequestAndHistory>();
            var IsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (var item in printRequestAndHistory)
            {
                item.Request.IsSelected = IsChecked;
            }
            dgRemoveRequestData.ItemsSource = printRequestAndHistory;
            dgRemoveRequestData.Items.Refresh();
        }
        private void Clear()
        {
            dgRemoveRequestData.SelectedIndex = -1;
            dgRemoveRequestData.ItemsSource = null;
        }
        private void RemoveRecord(System.Collections.Generic.IEnumerable<PrintRequestAndHistory> list)
        {
            using (var context = new CPSDbContext())
            {
                // Remove Request
                var requestDataRepository = new PersistenceBase<RequestDTO>(context);
                var printHistoryRepository = new PersistenceBase<PrintHistoryDTO>(context);
                foreach (var item in list.Where(w => w.Request.IsSelected = true))
                {
                    var existingRequestData = requestDataRepository.FindBy(f => f.Id == item.Request.Id).FirstOrDefault();
                    var existingPrintHistory = item.PrintHistory == null ? null : printHistoryRepository.FindBy(f => f.Id == item.PrintHistory.Id).FirstOrDefault();
                    if (existingRequestData != null)
                    {
                        requestDataRepository.SoftDelete(existingRequestData);
                    }
                    if (existingPrintHistory != null)
                    {
                        printHistoryRepository.SoftDelete(existingPrintHistory);
                    }
                }
                context.SaveChanges();
            }

        }
    }
}
