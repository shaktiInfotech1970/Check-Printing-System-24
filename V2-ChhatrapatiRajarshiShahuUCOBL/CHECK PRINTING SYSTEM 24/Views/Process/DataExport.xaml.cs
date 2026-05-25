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
            var result = BranchMasterDTO.GetLookups();
            result.Insert(0, new LookupItem<int, string> { Key = 0, Value = "Select All" });
            cbBrach.ItemsSource = result;
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
        }

        private void btnShowColumn_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CPSDbContext())
            {
                var requestRepository = new PersistenceBase<RequestDTO>(context);
                var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                var accountTypeRepository = new PersistenceBase<AccountTypeDTO>(context);
                var printHistoryRepository = new PersistenceBase<PrintHistoryDTO>(context);

                var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;

                var printHistoryQuery = printHistoryRepository.GetAll();
                if (dpTransactionDate.SelectedDate.HasValue)
                {
                    var startDate = dpTransactionDate.SelectedDate.Value.Date;
                    var endDate = startDate.AddDays(1).AddMilliseconds(-1);
                    printHistoryQuery = printHistoryQuery.Where(w => w.CreatedOn >= startDate && w.CreatedOn <= endDate);
                }

                var query = (from p in printHistoryQuery
                             join r in requestRepository.GetAll() on p.RequestId equals r.Id
                             join b in branchRepository.GetAll() on r.BranchId equals b.Id
                             join at in accountTypeRepository.GetAll() on r.TransactionCode equals at.Code
                             where (branchId == 0 || (branchId != 0 && r.BranchId == branchId)) && r.IsPrinted == true
                             select new PrintRequest { Request = r, AccountType = at, Branch = b });

                dg.ItemsSource = query.Distinct().ToList();
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dg.ItemsSource != null)
            {
                var requests = dg.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
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
            var requests = dg.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dlg.FileName))
            {
                foreach (var request in requests)
                {
                    file.WriteLine(string.Format("{0:00000000}~{1:000}~{2:00}~{3:000000000000000} ~{4:000000}~{5:000000}~{6:00}~{7:000}~{8:0}~{9:0}~{10}"
                        , Convert.ToInt32(request.Request.additional_f3.Trim())
                        , request.Request.BranchCode
                        , request.Request.TransactionCode
                        , request.Request.AccountNoFull
                        , request.Request.ChequeFrom
                        , request.Request.ChequeTo
                        , request.Request.NoOfChequeBook
                        , request.Request.NoOfCheque
                        , string.IsNullOrWhiteSpace(request.Request.BearerOrder) ? "B" : request.Request.BearerOrder.Trim().ToUpper().Substring(0, 1)
                        , request.Request.AtPar.ToUpper()
                        , request.Request.additional_f1
                        ));
                }
            }

            MessageBox.Show("File exported successfully", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dg.SelectedIndex = -1;
            dg.ItemsSource = null;
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
            Common.Helper.ToggleDGCheckbox(sender, dg);
        }
    }
}
