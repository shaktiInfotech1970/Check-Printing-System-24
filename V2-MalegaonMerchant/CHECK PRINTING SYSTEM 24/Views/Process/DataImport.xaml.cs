using CPS.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CPS.Views.Process
{
    /// <summary>
    /// Interaction logic for DataImport.xaml
    /// </summary>
    public partial class DataImport : UserControl
    {
        // Create OpenFileDialog
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        public static string CheckSum = string.Empty;
        public DataImport()
        {
            InitializeComponent();
            BindComboBox();
        }

        private void BindComboBox()
        {
            var result = BranchMasterDTO.GetLookups();
            result.Insert(0, new LookupItem<int, string> { Key = 0, Value = "Select All" });
            cbBrach.ItemsSource = result;
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Excel Files (*.xls)|*.xls;*.xlsx|ASCII text (*.asc)|*.asc|Normal Text File (*.txt)|*.txt|Commma Separated File(*.csv)|*.csv";

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
                if (string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    MessageBox.Show("Please select valid file", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                var selectedBranchId = (int?)cbBrach.SelectedValue;
                if (selectedBranchId.HasValue)
                {

                    CheckSum = this.GetChecksum(txtFileName.Text);
                    if (!string.IsNullOrEmpty(CheckSum))
                    {
                        if (this.IsUniqueCheckSum(CheckSum))
                        {

                            var import = new ImportRequest();
                            import.Load(txtFileName.Text);

                            dgImport.ItemsSource = null;
                            dgImport.Columns.Clear();
                            dgImport.Items.Refresh();

                            foreach (var column in import.Columns())
                            {
                                var dataGridTextColumn = new DataGridTextColumn { Header = column.Header, Binding = new Binding(column.PropertyName) };
                                dgImport.Columns.Add(dataGridTextColumn);
                            }

                            using (var context = new CPSDbContext())
                            {
                                if (selectedBranchId == 0)
                                {
                                    var itemSource = import.Data.OrderBy(o => o.TransactionCode).ToList();
                                    dgImport.ItemsSource = LoadRequiredFields(itemSource);
                                }
                                else
                                {
                                    var repository = new PersistenceBase<BranchMasterDTO>(context);
                                    var branch = repository.FindBy(f => f.Id == selectedBranchId.Value).FirstOrDefault();
                                    if (branch != null)
                                    {
                                        var branchCode = 0;
                                        branchCode = Convert.ToInt32(branch.Code);
                                        var itemSource = import.Data.Where(o => o.BranchCode == branchCode).OrderBy(o => o.TransactionCode).ToList();
                                        dgImport.ItemsSource = LoadRequiredFields(itemSource);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Branch not found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    }
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("File already imported.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please browse valid file.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import error", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    var branchId = (int)cbBrach.SelectedValue;
                    var requestNo = Counter.NextValue(Counters.Request);
                    using (var context = new CPSDbContext())
                    {
                        var repository = new PersistenceBase<RequestDTO>(context);
                        var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                        var atRepository = new PersistenceBase<AccountTypeDTO>(context);
                        var brRepository = new PersistenceBase<BranchMasterDTO>(context);

                        var olAccountTypes = atRepository.GetAll().Select(o => new LookupItem<int, int> { Key = o.Id, Value = o.Code }).ToList();
                        var olChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                                  select cbs).ToList();

                        var maxSeriesNo = repository.GetAll().Any() ? repository.GetAll().Max(o => o.SerialNo) : 0;
                        foreach (RequestDTO request in dgImport.ItemsSource)
                        {
                            totalRecords++;
                            request.RequestNo = requestNo;
                            request.SerialNo = ++maxSeriesNo;
                            //request.BranchId = branchId;
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
                                }
                                else
                                {
                                    var series = olChequeBookSeries.Where(o => o.BranchId == request.BranchId && o.AccountTypeId == olAccountTypes.Where(x => x.Value == request.TransactionCode).FirstOrDefault().Key).FirstOrDefault();
                                    series.LastChequeNumber = request.ChequeFrom - 1;
                                    series.AvailableCheques = series.EndChequeNumber - series.LastChequeNumber;
                                    MessageBox.Show(errors[0].ErrorMessage, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                            }
                        }
                    }

                    dgImport.ItemsSource = null;
                    dgImport.Columns.Clear();
                    dgImport.Items.Refresh();
                    CPS.Common.Helper.ClearFormData(this);

                    if (importedRecord > 0 && !string.IsNullOrEmpty(CheckSum))
                    {
                        this.AddCheckSumInHistory(CheckSum);
                    }
                    var message = string.Format("{0} record(s) imported out of {1} record(s) !", importedRecord, totalRecords);
                    MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Save error", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private List<RequestDTO> LoadRequiredFields(List<RequestDTO> data)
        {
            var updatedData = new List<RequestDTO>();
            using (var context = new CPSDbContext())
            {
                var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                var atRepository = new PersistenceBase<AccountTypeDTO>(context);
                var brRepository = new PersistenceBase<BranchMasterDTO>(context);
                var oTransactionCodes = atRepository.GetAll().Select(o => new { Id = o.Id, Code = o.Code }).ToList();
                var oChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                         join br in brRepository.GetAll() on cbs.BranchId equals br.Id
                                         select new { Series = cbs, Branch = br }).ToList();
                foreach (var record in data)
                {
                    record.AccountNo = record.AccountNoFull?.Substring(record.AccountNoFull.Length - Math.Min(record.AccountNoFull.Length, 6))?.PadLeft(6, '0');
                    var series = oChequeBookSeries.Where(o => o.Branch.Code == record.BranchCode.ToString() && o.Series.AccountTypeId == oTransactionCodes.Where(x => x.Code == record.TransactionCode).FirstOrDefault().Id).FirstOrDefault();
                    if (series != null && series.Series != null)
                    {
                        if (series.Series.LastChequeNumber == 0)
                        {
                            record.ChequeFrom = series.Series.StartChequeNumber;
                            record.ChequeTo = series.Series.StartChequeNumber - 1 + (record.NoOfChequeBook * record.NoOfCheque);
                        }
                        else
                        {
                            record.ChequeFrom = series.Series.LastChequeNumber + 1;
                            record.ChequeTo = series.Series.LastChequeNumber + (record.NoOfChequeBook * record.NoOfCheque);
                        }
                        series.Series.LastChequeNumber = record.ChequeTo;
                        record.BranchId = series.Series.BranchId;
                        updatedData.Add(record);
                    }
                }
            }
            return updatedData;
        }

        private bool LoadChequeNumbers(List<ChequeBookSeriesDTO> olChequeBookSeries, List<LookupItem<int, int>> olAccountTypes, RequestDTO oRequest)
        {
            var flag = false;
            var series = olChequeBookSeries.Where(o => o.BranchId == oRequest.BranchId && o.AccountTypeId == olAccountTypes.Where(x => x.Value == oRequest.TransactionCode).FirstOrDefault().Key).FirstOrDefault();
            if (series != null)
            {
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

        private string GetChecksum(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
            }
            return string.Empty;
        }
        private bool IsUniqueCheckSum(string CheckSum)
        {
            using (var context = new CPSDbContext())
            {
                DateTime checkDate = DateTime.Now.AddDays(-30);
                var repository = new PersistenceBase<DataImportFileHistoryDTO>(context);
                return !(repository.GetAll().Where(o => o.CheckSum.ToLower() == CheckSum.ToLower() && o.CreatedOn > checkDate).Any());
            }
        }
        private void AddCheckSumInHistory(string CheckSum)
        {
            try
            {
                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<DataImportFileHistoryDTO>(context);
                    var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    DataImportFileHistoryDTO DataImportFileHistory = new DataImportFileHistoryDTO();
                    DataImportFileHistory.CheckSum = CheckSum;
                    if (repository.SaveOrUpdate(DataImportFileHistory, errors))
                    {
                        context.SaveChanges();
                    }
                }
            }
            finally
            {
            }
        }
    }
}
