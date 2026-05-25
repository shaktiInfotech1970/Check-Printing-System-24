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
            dlg.Filter = "ASCII text (*.asc)|*.asc|Normal Text File (*.txt)|*.txt|Commma Separated File(*.csv)|*.csv|Excel Files (*.xls)|*.xls;*.xlsx";

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
                dgImport.ItemsSource = null;
                dgImport.Columns.Clear();
                dgImport.Items.Refresh();

                var import = new ImportRequest();
                import.Load(txtFileName.Text);
                foreach (var column in import.Columns())
                {
                    var dataGridTextColumn = new DataGridTextColumn { Header = column.Header, Binding = new Binding(column.PropertyName) };
                    dgImport.Columns.Add(dataGridTextColumn);
                }

                dgImport.ItemsSource = import.Data;
            }
            catch (Exception)
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

                        #region check if records found in the file belongs to the selected branch. If yes, updated the branchid else show message.
                        foreach (RequestDTO request in dgImport.ItemsSource)
                        {
                            var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                            var requestBranchCode = request.BranchCode.ToString("000").Trim();
                            var branch = repositoryBranch.FindBy(f => (branchId == 0 && f.Code == requestBranchCode) || (branchId != 0 && f.Id == branchId)).FirstOrDefault();
                            if (branch != null && Convert.ToInt32(branch.Code) == request.BranchCode)
                            {
                                request.BranchId = branch.Id;
                            }
                            else
                            {
                                MessageBox.Show("File you are trying to import has records from different branch.", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                return;
                            }
                        }
                        #endregion

                        foreach (RequestDTO request in dgImport.ItemsSource)
                        {
                            totalRecords++;

                            //request.Id = ObjectId.GenerateNewId();
                            request.RequestNo = requestNo;
                            //request.BranchId = (int)cbBrach.SelectedValue;
                            request.IsManualEntry = false;
                            request.IsPrinted = false;

                            var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                            if (repository.SaveOrUpdate(request, errors))
                            {
                                context.SaveChanges();
                                importedRecord++;
                            }
                            else
                            {
                                MessageBox.Show(errors[0].ErrorMessage, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                        }
                    }

                    dgImport.ItemsSource = null;
                    dgImport.Columns.Clear();
                    dgImport.Items.Refresh();
                    CPS.Common.Helper.ClearFormData(this);

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
    }
}
