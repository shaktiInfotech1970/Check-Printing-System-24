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

namespace CPS.Views.Master
{
    /// <summary>
    /// Interaction logic for BranchMaster.xaml
    /// </summary>
    public partial class BranchMaster : UserControl
    {
        //BusinessServiceBase<BranchMasterDTO> service = new BusinessServiceBase<BranchMasterDTO>();

        public BranchMaster()
        {
            InitializeComponent();
            BindDataGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var oBranchMaster = new BranchMasterDTO();

            if (btnSave.Tag != null)
            {
                oBranchMaster = (BranchMasterDTO)btnSave.Tag;
            }

            oBranchMaster.Code = txtCode.Text;
            oBranchMaster.Name = txtName.Text;
            oBranchMaster.MICR = txtMICR.Text;
            oBranchMaster.IFSC = txtIFSC.Text;
            oBranchMaster.ShortName = txtShortName.Text;
            oBranchMaster.City = txtCity.Text;
            oBranchMaster.PostalCode = txtPinCode.Text;
            oBranchMaster.Telephone1 = txtTelephone1.Text;
            oBranchMaster.Telephone2 = txtTelephone2.Text;
            oBranchMaster.Mobile = txtMobile.Text;
            oBranchMaster.Email = txtEmail.Text == string.Empty ? null : txtEmail.Text;
            oBranchMaster.Fax = txtFax.Text;
            oBranchMaster.AddressLine1 = txtAddress1.Text;
            oBranchMaster.AddressLine2 = txtAddress2.Text;
            oBranchMaster.AddressLine3 = txtAddress3.Text;
            oBranchMaster.ImportPath = txtImportPath.Text;
            oBranchMaster.ExportPath = txtExportPath.Text;

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BranchMasterDTO>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(oBranchMaster, errors))
                {
                    context.SaveChanges();
                    MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CPS.Common.Helper.ClearFormData(this);
                    BindDataGrid();
                    btnSave.Tag = null;
                    btnSave.Content = "Add";
                }
                else
                {
                    MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void btnBrowseImportPath_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog(txtImportPath);
        }

        private void btnBrowseExportPath_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog(txtExportPath);
        }

        private void OpenFolderDialog(TextBox obj)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result.ToString() == "OK")
                obj.Text = dialog.SelectedPath;
        }

        public void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BranchMasterDTO>(context);
                var response = repository.GetAll().ToList();
                if (response != null)
                {
                    dgBranchMaster.ItemsSource = response;
                }
            }
        }

        private void dgBranchMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var oBranchMaster = (BranchMasterDTO)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                btnSave.Tag = oBranchMaster;

                txtCode.Text = oBranchMaster.Code;
                txtName.Text = oBranchMaster.Name;
                txtIFSC.Text = oBranchMaster.IFSC;
                txtMICR.Text = oBranchMaster.MICR;
                txtShortName.Text = oBranchMaster.ShortName;
                txtCity.Text = oBranchMaster.City;
                txtPinCode.Text = oBranchMaster.PostalCode;
                txtTelephone1.Text = oBranchMaster.Telephone1;
                txtTelephone2.Text = oBranchMaster.Telephone2;
                txtMobile.Text = oBranchMaster.Mobile;
                txtEmail.Text = oBranchMaster.Email;
                txtFax.Text = oBranchMaster.Fax;
                txtAddress1.Text = oBranchMaster.AddressLine1;
                txtAddress2.Text = oBranchMaster.AddressLine2;
                txtAddress3.Text = oBranchMaster.AddressLine3;
                txtImportPath.Text = oBranchMaster.ImportPath;
                txtExportPath.Text = oBranchMaster.ExportPath;
                btnSave.Content = "Save";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dgBranchMaster.SelectedIndex = -1;
            btnSave.Tag = null;
            btnSave.Content = "Add";
        }

    }
}
