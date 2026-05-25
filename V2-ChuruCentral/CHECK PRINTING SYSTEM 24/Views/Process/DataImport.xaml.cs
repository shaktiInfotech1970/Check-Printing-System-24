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
    /// Interaction logic for DataImport.xaml
    /// </summary>
    public partial class DataImport : UserControl
    {
        // Create OpenFileDialog
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        public DataImport()
        {
            InitializeComponent();
            BindComboBox();
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Commma Separated File(*.csv)|*.csv|Normal Text File (*.txt)|*.txt|ASCII text (*.asc)|*.asc";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtFileName.Text = filename;
            }
        }

        private void btnShowColumns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var import = new ImportRequest();
                if (string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    MessageBox.Show("Please select valid file", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                import.Load(txtFileName.Text);
                foreach (var column in import.Columns())
                {
                    var dataGridTextColumn = new DataGridTextColumn { Header = column.Header, Binding = new Binding(column.PropertyName) };
                    dgImport.Columns.Add(dataGridTextColumn);
                }

                var selectedBranchId = (int?)cbBrach.SelectedValue;
                if (selectedBranchId.HasValue)
                {
                    using (var context = new CPSDbContext())
                    {
                        var repository = new PersistenceBase<BranchMasterDTO>(context);
                        var branch = repository.FindBy(f => f.Id == selectedBranchId.Value).FirstOrDefault();
                        if (branch != null)
                        {
                            var branchCode = 0;
                            branchCode = Convert.ToInt32(branch.Code);
                            var itemSource = import.Data.Where(o => o.BranchCode == branchCode).OrderBy(o => o.TransactionCode).ToList();
                            dgImport.ItemsSource = LoadChequeNumbers(itemSource);
                        }
                        else
                        {
                            MessageBox.Show("Branch not found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import error" + ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var totalRecords = 0;
                var importedRecord = 0;
                if (cbBrach.SelectedValue != null)
                {
                    var requestNo = Counter.NextValue(Counters.Request);
                    using (var context = new CPSDbContext())
                    {
                        var repository = new PersistenceBase<RequestDTO>(context);
                        var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                        var atRepository = new PersistenceBase<AccountTypeDTO>(context);

                        var olAccountTypes = atRepository.GetAll().Select(o => new LookupItem<int, int> { Key = o.Id, Value = o.Code }).ToList();
                        var olChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                                  where cbs.BranchId == (int)cbBrach.SelectedValue
                                                  select cbs).ToList();
                        var maxSeriesNo = repository.GetAll().Any() ? repository.GetAll().Max(o => o.SerialNo) : 0;
                        maxSeriesNo++;
                        foreach (RequestDTO request in dgImport.ItemsSource)
                        {
                            totalRecords++;

                            //request.Id = ObjectId.GenerateNewId();
                            request.SerialNo = maxSeriesNo;
                            request.RequestNo = requestNo;
                            request.BranchId = (int)cbBrach.SelectedValue;
                            request.IsManualEntry = false;
                            request.IsPrinted = false;

                            //if valid cheque series found then save the record
                            if (LoadChequeNumbers(olChequeBookSeries, olAccountTypes, request))
                            {
                                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                                if (repository.SaveOrUpdate(request, errors))
                                {
                                    context.SaveChanges();
                                    importedRecord++;
                                    maxSeriesNo++;
                                }
                                else //Rollback the updated last cheque number (To disallow using cheque series for the records which gives error)
                                {
                                    var series = olChequeBookSeries.Where(o => o.AccountTypeId == olAccountTypes.Where(x => x.Value == request.TransactionCode).FirstOrDefault().Key).FirstOrDefault();
                                    series.LastChequeNumber = request.ChequeFrom - 1;
                                    series.AvailableCheques = series.EndChequeNumber - series.LastChequeNumber;
                                }
                            }
                        }
                    }

                    dgImport.ItemsSource = null;
                    CPS.Common.Helper.ClearFormData(this);

                    var message = string.Format("{0} record(s) imported out of {1} record(s) !", importedRecord, totalRecords);
                    MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error" + ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cbBrach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBranchId = (int?)cbBrach.SelectedValue;
            if (selectedBranchId.HasValue)
            {
                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<BranchMasterDTO>(context);
                    var branch = repository.FindBy(f => f.Id == selectedBranchId.Value).FirstOrDefault();
                    if (branch != null)
                    {
                        dlg.InitialDirectory = branch.ImportPath;
                    }
                }
            }
        }

        private List<RequestDTO> LoadChequeNumbers(List<RequestDTO> data)
        {
            var updatedData = new List<RequestDTO>();
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            if (branchId > 0)
            {
                using (var context = new CPSDbContext())
                {
                    var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                    var atRepository = new PersistenceBase<AccountTypeDTO>(context);
                    var oTransactionCodes = atRepository.GetAll().Select(o => new { Id = o.Id, Code = o.Code }).ToList();
                    var oChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                             where cbs.BranchId == branchId
                                             select cbs).ToList();
                    foreach (var record in data)
                    {
                        var series = oChequeBookSeries.Where(o => o.AccountTypeId == oTransactionCodes.Where(x => x.Code == record.TransactionCode).FirstOrDefault().Id).FirstOrDefault();
                        if (series != null)
                        {
                            if (series.LastChequeNumber == 0)
                            {
                                record.ChequeFrom = series.StartChequeNumber;
                                record.ChequeTo = series.StartChequeNumber - 1 + (record.NoOfChequeBook * record.NoOfCheque);
                            }
                            else
                            {
                                record.ChequeFrom = series.LastChequeNumber + 1;
                                record.ChequeTo = series.LastChequeNumber + (record.NoOfChequeBook * record.NoOfCheque);
                            }
                            series.LastChequeNumber = record.ChequeTo;
                            updatedData.Add(record);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            return updatedData;
        }

        private bool LoadChequeNumbers(List<ChequeBookSeriesDTO> olChequeBookSeries, List<LookupItem<int, int>> olAccountTypes, RequestDTO oRequest)
        {
            var flag = false;
            var series = olChequeBookSeries.Where(o => o.AccountTypeId == olAccountTypes.Where(x => x.Value == oRequest.TransactionCode).FirstOrDefault().Key).FirstOrDefault();
            if (series != null)
            {
                if (series.LastChequeNumber == 0)
                {
                    oRequest.ChequeFrom = series.StartChequeNumber;
                    oRequest.ChequeTo = series.StartChequeNumber - 1 + (oRequest.NoOfChequeBook * oRequest.NoOfCheque);
                }
                else
                {
                    oRequest.ChequeFrom = series.LastChequeNumber + 1;
                    oRequest.ChequeTo = series.LastChequeNumber + (oRequest.NoOfChequeBook * oRequest.NoOfCheque);
                }
                if (oRequest.ChequeTo > series.EndChequeNumber)
                {
                    MessageBox.Show(string.Format("Chequebook Series Over. Cheque for {0} will not be printed", oRequest.Name), "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    series.LastChequeNumber = oRequest.ChequeTo;
                    series.AvailableCheques = series.EndChequeNumber - series.LastChequeNumber;
                    return true;
                }
            }
            return flag;
        }
    }
}
