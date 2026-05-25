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

                dgRequestEntry.ItemsSource = query.ToList();
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
                    dlg.DefaultExt = ".csv";
                    dlg.Filter = "CSV (Comma Delimited)|*.csv";
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
                    file.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                        , request.Request.SerialNo
                        , request.Request.CityCode
                        , request.Request.BankCode
                        , request.Request.BranchCode
                        , request.Request.AccountNo
                        , request.Request.AccountNoFull
                        , request.Request.NoOfCheque
                        , request.Request.NoOfChequeBook
                        , request.Request.BearerOrder
                        , request.Request.AtPar
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
    }
}
