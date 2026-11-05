using CPS.Business;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPS.Views.Process
{
    /// <summary>
    /// Interaction logic for PrintChequeBook.xaml
    /// </summary>
    public partial class PrintChequeBook : UserControl
    {
        public PrintChequeBook()
        {
            InitializeComponent();
            BindComboBox();
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            cbTransactionCode.ItemsSource = AccountTypeDTO.GetLookups2();
            cbTransactionCode.DisplayMemberPath = "Value";
            cbTransactionCode.SelectedValuePath = "Key";           
        }

        private void btnShowColumn_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CPSDbContext())
            {
                var requestRepository = new PersistenceBase<RequestDTO>(context);
                var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                var accountTypeRepository = new PersistenceBase<AccountTypeDTO>(context);

                var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
                var transactionCode = cbTransactionCode.SelectedValue == null ? 0 : (int)cbTransactionCode.SelectedValue;
                var bookSize = cbBookSize.SelectedValue == null ? 0 : (int)cbBookSize.SelectedValue;

                var query = (from r in requestRepository.FilterBy(w => w.IsPrinted == false)
                             join b in branchRepository.GetAll() on r.BranchId equals b.Id
                             join at in accountTypeRepository.GetAll() on r.TransactionCode equals at.Code
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                             && (transactionCode == 0 || (transactionCode != 0 && r.TransactionCode == transactionCode))
                             && (bookSize == 0 || (bookSize != 0 && r.NoOfCheque == bookSize))
                             select new PrintRequest { Request = r, AccountType = at, Branch = b });

                dgRequestEntry.ItemsSource = query.ToList();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            if (dgRequestEntry.ItemsSource != null)
            {
                var requests = dgRequestEntry.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
                if (requests.Count() <= 0)
                {
                    MessageBox.Show("Please select requests to print", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var isPrinted = PrintJob.Print(requests);
                    btnShowColumn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    if (isPrinted)
                    {
                        MessageBox.Show("Cheque books have been printed.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Print job is not created", "Warning!", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
            btnSave.IsEnabled = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dgRequestEntry.SelectedIndex = -1;
            dgRequestEntry.ItemsSource = null;
        }

        private void ToggleDGCheckbox(object sender, RoutedEventArgs e)
        {
            Common.Helper.ToggleDGCheckbox(sender, dgRequestEntry);
        }
    }
}
