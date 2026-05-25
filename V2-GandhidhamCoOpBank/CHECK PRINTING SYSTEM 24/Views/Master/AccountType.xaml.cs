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
    /// Interaction logic for AccountType.xaml
    /// </summary>
    public partial class AccountType : UserControl
    {
        public AccountType()
        {
            InitializeComponent();
            BindDataGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var oAccountType = new AccountTypeDTO();

            if (string.IsNullOrWhiteSpace(txtAccountCode.Text))
            {
                MessageBox.Show("Account Code is required.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var code = Convert.ToInt32(txtAccountCode.Text);
                if (btnSave.Tag != null)
                {
                    oAccountType = (AccountTypeDTO)btnSave.Tag;
                }

                oAccountType.Code = code;
                oAccountType.Name = txtAccountType.Text;

                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<AccountTypeDTO>(context);
                    var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (repository.SaveOrUpdate(oAccountType, errors))
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
        }

        private void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<AccountTypeDTO>(context);
                var response = repository.GetAll().ToList();
                if (response != null)
                {
                    dgAccountType.ItemsSource = response;
                }
            }
        }

        private void dgAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var oAccountType = (AccountTypeDTO)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                btnSave.Tag = oAccountType;
                txtAccountCode.Text = oAccountType.Code.ToString();
                txtAccountType.Text = oAccountType.Name;
                btnSave.Content = "Save";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dgAccountType.SelectedIndex = -1;
            btnSave.Tag = null;
            btnSave.Content = "Add";
        }
    }
}
