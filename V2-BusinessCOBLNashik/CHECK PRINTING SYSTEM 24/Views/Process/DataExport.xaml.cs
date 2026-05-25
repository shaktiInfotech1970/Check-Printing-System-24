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
    public partial class DataExport : UserControl
    {
        // Create OpenFileDialog
        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

        public DataExport()
        {
            InitializeComponent();
            BindComboBox();
            dlg.FileOk += dlg_FileOk;
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
        }

        private void btnShowColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dpTransactionDateTo.SelectedDate.HasValue && (!dpTransactionDateFrom.SelectedDate.HasValue || dpTransactionDateTo.SelectedDate.Value < dpTransactionDateFrom.SelectedDate.Value))
            {
                dgRequestEntry.ItemsSource = null;
                MessageBox.Show("To date must be greater than From date.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new CPSDbContext())
            {
                var requestRepository = new PersistenceBase<RequestDTO>(context);
                var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                var accountTypeRepository = new PersistenceBase<AccountTypeDTO>(context);
                var printHistoryRepository = new PersistenceBase<PrintHistoryDTO>(context);

                var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;

                var printHistoryQuery = printHistoryRepository.GetAll();
                if (dpTransactionDateFrom.SelectedDate.HasValue)
                {
                    var startDate = dpTransactionDateFrom.SelectedDate.Value.Date;
                    var endDate = startDate.AddDays(1).AddMilliseconds(-1);
                    if (dpTransactionDateTo.SelectedDate.HasValue && dpTransactionDateTo.SelectedDate.Value.Date > dpTransactionDateFrom.SelectedDate.Value.Date) endDate = dpTransactionDateTo.SelectedDate.Value.Date;

                    printHistoryQuery = printHistoryQuery.Where(w => w.CreatedOn >= startDate && w.CreatedOn <= endDate);
                }

                var query = (from p in printHistoryQuery
                             join r in requestRepository.GetAll() on p.RequestId equals r.Id
                             join b in branchRepository.GetAll() on r.BranchId equals b.Id
                             join at in accountTypeRepository.GetAll() on r.TransactionCode equals at.Code
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId)) && r.IsPrinted == true
                             select new PrintRequest { Request = r, AccountType = at, Branch = b });

                dgRequestEntry.ItemsSource = query.Distinct().ToList();
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequestEntry.ItemsSource != null)
            {
                var requests = dgRequestEntry.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
                if (requests.Count() <= 0)
                {
                    MessageBox.Show("Nothing to export", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Set filter for file extension and default file extension
                    dlg.DefaultExt = ".txt";
                    dlg.Filter = "Normal Text File (*.txt)|*.txt";
                    dlg.FileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    dlg.ShowDialog();
                }
            }
        }

        void dlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var requests = dgRequestEntry.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dlg.FileName))
            {
                foreach (var request in requests)
                {
                    file.WriteLine(string.Format("{0:000000}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10:00000000}{11:000000}"
                        , request.Request.BranchCode
                        , request.Request.additional_f6
                        , request.Request.additional_f7.PadLeft(22 - ( 6 + request.Request.additional_f6.Length), ' '), request.Request.additional_f1
                        , request.Request.additional_f3.PadLeft(8, ' ')
                        , request.Request.NoOfCheque.ToString().PadLeft(4, ' ')
                        , request.Request.NoOfChequeBook.ToString().PadLeft(8, ' ')
                        , request.Request.ChequeFrom.ToString().PadLeft(12, ' ')
                        , request.Request.TransactionCode.ToString().PadLeft(4, ' ')
                        , request.Request.TransactionCode.ToString().PadLeft(4, ' ')
                        , request.Request.additional_f1.PadLeft(14, ' '), request.Request.BranchCode
                        ));
                }
            }

            MessageBox.Show("File exported successfully", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dgRequestEntry.SelectedIndex = -1;
            dgRequestEntry.ItemsSource = null;
        }

        private void cbBrach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBranchId = (int?)cbBrach.SelectedValue;
            if (selectedBranchId.HasValue)
            {
                using (var context = new CPSDbContext())
                {
                    var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                    var branch = branchRepository.FilterBy(f => f.Id == selectedBranchId.Value).FirstOrDefault();
                    if (branch != null)
                    {
                        dlg.InitialDirectory = branch.ExportPath;
                    }
                }
            }
        }

        private void ToggleDGCheckbox(object sender, RoutedEventArgs e)
        {
            Common.Helper.ToggleDGCheckbox(sender, dgRequestEntry);
        }
    }
}
