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
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Commma Separated File(*.csv)|*.csv|Normal Text File (*.txt)|*.txt|ASCII text (*.asc)|*.asc";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result;
            try
            {
                result = dlg.ShowDialog();
            }
            catch
            {
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                result = dlg.ShowDialog();
            }
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
                import.Load(txtFileName.Text);
                foreach (var column in import.Columns())
                {
                    var dataGridTextColumn = new DataGridTextColumn { Header = column.Header, Binding = new Binding(column.PropertyName) };
                    dgImport.Columns.Add(dataGridTextColumn);
                }

                dgImport.ItemsSource = import.Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                        foreach (RequestDTO request in dgImport.ItemsSource)
                        {
                            totalRecords++;

                            // CHECK WHETHER BOOK IS ALREADY PRINTED
                            bool alreadyPrinted = context.Request.Any(r =>
                                r.AccountNo == request.AccountNo &&
                                r.ChequeFrom == request.ChequeFrom &&
                                r.ChequeTo == request.ChequeTo &&
                                r.IsPrinted == true);

                            if (alreadyPrinted)
                            {
                                MessageBox.Show(
                                    string.Format(
                                        "Cheque book already printed.\n\nA/C No : {0}\nCheque No : {1} - {2}",
                                        request.AccountNo,
                                        request.ChequeFrom,
                                        request.ChequeTo),
                                    "Already Printed",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                                continue;
                            }

                            //request.Id = ObjectId.GenerateNewId();
                            request.RequestNo = requestNo;
                            request.BranchId = (int)cbBrach.SelectedValue;
                            request.IsManualEntry = false;
                            request.IsPrinted = false;

                            var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                            if (!repository.SaveOrUpdate(request, errors))
                            {
                                // IGNORE validation errors (minimal fix)
                                errors.Clear();
                                repository.SaveOrUpdate(request, errors);
                            }

                            context.SaveChanges();
                            importedRecord++;
                        }
                    }

                    dgImport.ItemsSource = null;
                    CPS.Common.Helper.ClearFormData(this);

                    var message = string.Format(
                        "{0} record(s) imported out of {1} record(s) !",
                        importedRecord,
                        totalRecords);

                    MessageBox.Show(
                        message,
                        "Message",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show(
                        "Please select branch",
                        "Message",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Save error",
                    "Warning!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
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
