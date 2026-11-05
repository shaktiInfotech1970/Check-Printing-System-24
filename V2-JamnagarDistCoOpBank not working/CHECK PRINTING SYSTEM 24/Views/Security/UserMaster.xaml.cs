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

namespace CPS.Views.Security
{
    /// <summary>
    /// Interaction logic for UserMaster.xaml
    /// </summary>
    public partial class UserMaster : UserControl
    {
        public UserMaster()
        {
            InitializeComponent();
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<UserMasterDTO>(context);
                var response = repository.GetAll().Where(o => o.UserId.ToLower() != "superadmin" && o.UserId.ToLower() != "admin").ToList();
                if (response != null)
                {
                    dgUserMaster.ItemsSource = response;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var oUser = new UserMasterDTO();

            if (btnSave.Tag != null)
            {
                oUser = (UserMasterDTO)btnSave.Tag;
            }

            oUser.Name = txtName.Text;
            oUser.UserId = txtUserId.Text;
            oUser.Password = txtPassword.Password;
            oUser.IsLocked = IsLocked.IsChecked ?? false;
            if (oUser.IsLocked)
            {
                oUser.LockDate = dpLockDate.SelectedDate ?? DateTime.Now;
                oUser.LockReason = txtLockReason.Text;
            }
            else
            {
                oUser.LockDate = null;
                oUser.LockReason = string.Empty;
            }

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<UserMasterDTO>(context);

                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(oUser, errors))
                {
                    context.SaveChanges();

                    if (btnSave.Tag == null)
                    {
                        var repositoryPermission = new PersistenceBase<PermissionDTO>(context);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.AccountType, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.BankMaster, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.BranchMaster, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.ChequeBookSeries, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.DataExport, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.DataImport, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.PrintChequeBook, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.RePrintChequeBook, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.RePrintRequest, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.RePrintSinglePage, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.RequestDataEntry, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.DaywiseChequePrint, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.PendingChequeRequest, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.PrintedCheque, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.PrintedChequePrintFile, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.PrintedChequeSeries, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.ReprintedCheque, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.ReprintedChequeSinglePage, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.TotalPrintCheque, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.TotalReprintCheque, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.Permission, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.UserMaster, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.Preferences, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.DatabaseBackup, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.RequestLayoutPreference, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.ChequeLayoutPreference, Permission = (int)Common.Permission.None }, errors);
                        repositoryPermission.SaveOrUpdate(new PermissionDTO { UserId = oUser.Id, Page = Common.Page.SearchReport, Permission = (int)Common.Permission.None }, errors);
                        context.SaveChanges();
                    }

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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            btnSave.Content = "Add";
            btnSave.Tag = null;
            dgUserMaster.SelectedIndex = -1;
        }

        private void dgUserMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var oUserMaster = (UserMasterDTO)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                btnSave.Tag = oUserMaster;
                txtName.Text = oUserMaster.Name;
                txtUserId.Text = oUserMaster.UserId;
                txtPassword.Password = oUserMaster.Password;
                IsLocked.IsChecked = oUserMaster.IsLocked;
                dpLockDate.SelectedDate = oUserMaster.LockDate;
                txtLockReason.Text = oUserMaster.LockReason;
                btnSave.Content = "Save";
            }
        }
    }
}
