using CPS.Business;
using CPS.Common;
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
    /// Interaction logic for RequestEntry.xaml
    /// </summary>
    public partial class RequestEntry : UserControl
    {
        public RequestEntry()
        {
            InitializeComponent();
            BindComboBox();
            BindDataGrid();
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            cbTransactionCode.ItemsSource = AccountTypeDTO.GetLookups2();
            cbTransactionCode.DisplayMemberPath = "Value";
            cbTransactionCode.SelectedValuePath = "Key";

            var BearerOrOrderList = new List<LookupItem<string, string>>();
            BearerOrOrderList.Add(new LookupItem<string, string> { Key = "Bearer", Value = "Bearer" });
            BearerOrOrderList.Add(new LookupItem<string, string> { Key = "Order", Value = "Order" });
            cbBearerOrOrder.ItemsSource = BearerOrOrderList;
            cbBearerOrOrder.DisplayMemberPath = "Value";
            cbBearerOrOrder.SelectedValuePath = "Key";
                        
            var AtParList = new List<LookupItem<string, string>>();
            AtParList.Add(new LookupItem<string, string> { Key = "Y", Value = "Y" });
            AtParList.Add(new LookupItem<string, string> { Key = "N", Value = "N" });
            cbAtPar.ItemsSource = AtParList;
            cbAtPar.DisplayMemberPath = "Value";
            cbAtPar.SelectedValuePath = "Key";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            btnSave.Content = "Add";
            dgRequestEntry.SelectedIndex = -1;
            btnSave.Tag = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestDTO();
            if (btnSave.Tag != null)
            {
                request = (RequestDTO)btnSave.Tag;
            }
            else
            {
                request.RequestNo = Counter.NextValue(Counters.Request);
                request.IsManualEntry = true;
                request.IsPrinted = false;
            }

            if (cbBrach.SelectedValue != null)
                request.BranchId = (int)cbBrach.SelectedValue;
            else
            {
                MessageBox.Show("Please select branch", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            request.SerialNo = Convert.ToInt32(string.IsNullOrWhiteSpace(txtSerialNo.Text) ? "0" : txtSerialNo.Text);
            //request.MICRCode = txtMICRCode.Text;
            request.CityCode = Convert.ToInt32(string.IsNullOrWhiteSpace(txtCityCode.Text) ? "0" : txtCityCode.Text);
            if (string.IsNullOrWhiteSpace(txtMICRCode.Text))
            {
                MessageBox.Show("Please enter MICR code", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            request.BranchCode = Convert.ToInt32(txtMICRCode.Text.Substring(6, 3));
            request.BankCode = Convert.ToInt32(txtMICRCode.Text.Substring(3, 3));
            request.AccountNo = txtAccountNo.Text;
            request.AccountNoFull = txtAccountNoLong.Text;
            if (cbTransactionCode.SelectedValue != null)
                request.TransactionCode = (int)cbTransactionCode.SelectedValue;
            request.Name = txtCustomerName.Text;
            request.JointName1 = txtJointName1.Text;
            request.JointName2 = txtJointName2.Text;
            request.Signatory1 = txtSigningAuth.Text;
            request.Signatory2 = txtSigningAuth1.Text;
            request.Signatory3 = txtSigningAuth2.Text;
            request.Address1 = txtAddress1.Text;
            request.Address2 = txtAddress2.Text;
            request.Address3 = txtAddress3.Text;
            request.City = txtCity.Text;
            request.PostalCode = txtPinCode.Text;
            request.NoOfChequeBook = Convert.ToInt32(string.IsNullOrWhiteSpace(txtNoofBook.Text) ? "0" : txtNoofBook.Text);
            if (cbBookSize.SelectedValue != null)
                request.NoOfCheque = (int)cbBookSize.SelectedValue;
            request.ChequeFrom = Convert.ToInt32(string.IsNullOrWhiteSpace(txtChequeNoFrom.Text) ? "0" : txtChequeNoFrom.Text);
            request.ChequeTo = Convert.ToInt32(string.IsNullOrWhiteSpace(txtChequeNoTo.Text) ? "0" : txtChequeNoTo.Text);
            if (cbBearerOrOrder.SelectedValue != null)
                request.BearerOrder = (string)cbBearerOrOrder.SelectedValue;
            if (cbAtPar.SelectedValue != null)
                request.AtPar = (string)cbAtPar.SelectedValue;

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<RequestDTO>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(request, errors))
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

        private void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var repositoy = new PersistenceBase<RequestDTO>(context);
                var response = repositoy.FilterBy(f => f.IsPrinted == false).ToList();
                if (response != null)
                {
                    dgRequestEntry.ItemsSource = response;
                }
            }
        }

        private void dgRequestEntry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var request = (RequestDTO)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                btnSave.Tag = request;
                btnSave.Content = "Save";

                cbBrach.SelectedValue = request.BranchId;
                txtRequestNo.Text = request.RequestNo.ToString();
                txtSerialNo.Text = request.SerialNo.ToString();
                txtMICRCode.Text = request.MICRCode;
                txtCityCode.Text = request.CityCode.ToString();
                txtAccountNo.Text = request.AccountNo;
                txtAccountNoLong.Text = request.AccountNoFull;
                cbTransactionCode.SelectedValue = request.TransactionCode;
                txtCustomerName.Text = request.Name;
                txtJointName1.Text = request.JointName1;
                txtJointName2.Text = request.JointName2;
                txtSigningAuth.Text = request.Signatory1;
                txtSigningAuth1.Text = request.Signatory2;
                txtSigningAuth2.Text = request.Signatory3;
                txtAddress1.Text = request.Address1;
                txtAddress2.Text = request.Address2;
                txtAddress3.Text = request.Address3;
                txtCity.Text = request.City;
                txtPinCode.Text = request.PostalCode;
                txtNoofBook.Text = request.NoOfChequeBook.ToString();
                cbBookSize.SelectedValue = (enumCheckBookSize)request.NoOfCheque;
                txtChequeNoFrom.Text = request.ChequeFrom.ToString();
                txtChequeNoTo.Text = request.ChequeTo.ToString();
                cbBearerOrOrder.SelectedValue = request.BearerOrder;
                cbAtPar.SelectedValue = request.AtPar;
            }
        }
    }
}
