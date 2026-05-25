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
            cbTransactionCode.SelectedIndex = 0;

            var BearerOrOrderList = new List<LookupItem<string, string>>();
            BearerOrOrderList.Add(new LookupItem<string, string> { Key = "Bearer", Value = "Bearer" });
            BearerOrOrderList.Add(new LookupItem<string, string> { Key = "Order", Value = "Order" });
            cbBearerOrOrder.ItemsSource = BearerOrOrderList;
            cbBearerOrOrder.DisplayMemberPath = "Value";
            cbBearerOrOrder.SelectedValuePath = "Key";
            cbBearerOrOrder.SelectedIndex = 0;

            var BookSizeList = new List<LookupItem<int, string>>();
            BookSizeList.Add(new LookupItem<int, string> { Key = 15, Value = "15" });
            BookSizeList.Add(new LookupItem<int, string> { Key = 30, Value = "30" });
            BookSizeList.Add(new LookupItem<int, string> { Key = 45, Value = "45" });
            BookSizeList.Add(new LookupItem<int, string> { Key = 60, Value = "60" });

            cbBookSize.ItemsSource = BookSizeList;
            cbBookSize.DisplayMemberPath = "Value";
            cbBookSize.SelectedValuePath = "Key";
            cbBookSize.SelectedIndex = 0;

            var NoOfBookList = new List<LookupItem<int, string>>();
            for (int i = 1; i <= 10; i++)
            {
                NoOfBookList.Add(new LookupItem<int, string> { Key = i, Value = i.ToString() });
            }
            cbNoOfBook.ItemsSource = NoOfBookList;
            cbNoOfBook.DisplayMemberPath = "Value";
            cbNoOfBook.SelectedValuePath = "Key";
            cbNoOfBook.SelectedIndex = 0;

            var AtParList = new List<LookupItem<string, string>>();
            AtParList.Add(new LookupItem<string, string> { Key = "Y", Value = "Y" });
            AtParList.Add(new LookupItem<string, string> { Key = "N", Value = "N" });
            cbAtPar.ItemsSource = AtParList;
            cbAtPar.DisplayMemberPath = "Value";
            cbAtPar.SelectedValuePath = "Key";
            cbAtPar.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            btnSave.Content = "Add";
            dgRequestEntry.SelectedIndex = -1;
            btnSave.Tag = null;
            BindComboBox();
            SetAccessibility(true);
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
                request.SerialNo = request.RequestNo;
                request.IsManualEntry = true;
                request.IsPrinted = false;
                request.BranchCode = Convert.ToInt32(txtMICRCode.Text.Substring(6, 3));
                request.BankCode = Convert.ToInt32(txtMICRCode.Text.Substring(3, 3));
                //request.SerialNo = Convert.ToInt32(string.IsNullOrWhiteSpace(txtSerialNo.Text) ? "0" : txtSerialNo.Text);
                //request.MICRCode = txtMICRCode.Text;
                request.CityCode = Convert.ToInt32(string.IsNullOrWhiteSpace(txtCityCode.Text) ? "0" : txtCityCode.Text);
                request.NoOfChequeBook = Convert.ToInt32(string.IsNullOrWhiteSpace(cbNoOfBook.SelectedValue.ToString()) ? "0" : cbNoOfBook.SelectedValue);
                if (cbBookSize.SelectedValue != null)
                    request.NoOfCheque = (int)cbBookSize.SelectedValue;
                request.ChequeFrom = Convert.ToInt32(string.IsNullOrWhiteSpace(txtChequeNoFrom.Text) ? "0" : txtChequeNoFrom.Text);
                request.ChequeTo = Convert.ToInt32(string.IsNullOrWhiteSpace(txtChequeNoTo.Text) ? "0" : txtChequeNoTo.Text);
            }

            if (cbBrach.SelectedValue != null)
                request.BranchId = (int)cbBrach.SelectedValue;
            else
            {
                MessageBox.Show("Please select branch", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMICRCode.Text))
            {
                MessageBox.Show("Please enter MICR code", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //request.AccountNo = txtAccountNo.Text;
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
                    if (btnSave.Tag != null)
                    {
                        context.SaveChanges();
                        MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        CPS.Common.Helper.ClearFormData(this);
                        BindComboBox();
                        BindDataGrid();
                        btnSave.Tag = null;
                        btnSave.Content = "Add";
                    }
                    else
                    {
                        var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                        var atRepository = new PersistenceBase<AccountTypeDTO>(context);

                        var oChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                                 join at in atRepository.GetAll() on cbs.AccountTypeId equals at.Id
                                                 where cbs.BranchId == request.BranchId && at.Code == request.TransactionCode
                                                 select cbs).FirstOrDefault();
                        oChequeBookSeries.LastChequeNumber = Convert.ToInt32(txtChequeNoTo.Text);
                        oChequeBookSeries.AvailableCheques = oChequeBookSeries.EndChequeNumber - oChequeBookSeries.LastChequeNumber;
                        if (oChequeBookSeries.AvailableCheques >= 0)
                        {
                            if (cbsRepository.SaveOrUpdate(oChequeBookSeries, errors))
                            {
                                context.SaveChanges();
                                MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                CPS.Common.Helper.ClearFormData(this);
                                BindComboBox();
                                BindDataGrid();
                                btnSave.Tag = null;
                                btnSave.Content = "Add";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your cheque series for this branch and transaction code is completely used. Please contact your System Administrator!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
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
                //txtAccountNo.Text = request.AccountNo;
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
                cbNoOfBook.SelectedValue = request.NoOfChequeBook.ToString();
                cbBookSize.SelectedValue = request.NoOfCheque;
                txtChequeNoFrom.Text = request.ChequeFrom.ToString();
                txtChequeNoTo.Text = request.ChequeTo.ToString();
                cbBearerOrOrder.SelectedValue = request.BearerOrder;
                cbAtPar.SelectedValue = request.AtPar;

                SetAccessibility(false);
            }
        }

        private void SetAccessibility(bool flag)
        {
            cbBrach.IsEnabled = flag;
            cbTransactionCode.IsEnabled = flag;
            cbNoOfBook.IsEnabled = flag;
            cbBookSize.IsEnabled = flag;
        }

        private void cbBookSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChequeNumbers();
        }

        private void cbNoOfBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChequeNumbers();
        }

        private void cbTransactionCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChequeNumbers();
        }

        private void cbBrach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            if (branchId > 0)
            {
                using (var context = new CPSDbContext())
                {
                    var branchRepository = new PersistenceBase<BranchMasterDTO>(context);

                    var oBranch = (from b in branchRepository.GetAll()
                                   where b.Id == branchId
                                   select b).FirstOrDefault();

                    txtMICRCode.Text = oBranch.MICR;
                    txtCity.Text = oBranch.City;
                    txtCityCode.Text = txtMICRCode.Text.Substring(0, 3);
                }
                LoadChequeNumbers();
            }
        }

        private void LoadChequeNumbers()
        {
            var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
            if (branchId > 0)
            {
                var transactionCode = Convert.ToInt32(cbTransactionCode.SelectedValue);
                var bookSize = Convert.ToInt32(cbBookSize.SelectedValue);
                var noOfBook = Convert.ToInt32(cbNoOfBook.SelectedValue);
                using (var context = new CPSDbContext())
                {
                    var cbsRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                    var atRepository = new PersistenceBase<AccountTypeDTO>(context);
                    var oChequeBookSeries = (from cbs in cbsRepository.GetAll()
                                             join at in atRepository.GetAll() on cbs.AccountTypeId equals at.Id
                                             where cbs.BranchId == branchId && at.Code == transactionCode
                                             select cbs).FirstOrDefault();

                    if (oChequeBookSeries.LastChequeNumber == 0)
                    {
                        txtChequeNoFrom.Text = oChequeBookSeries.StartChequeNumber.ToString();
                        txtChequeNoTo.Text = (oChequeBookSeries.StartChequeNumber - 1 + (bookSize * noOfBook)).ToString();
                    }
                    else
                    {
                        txtChequeNoFrom.Text = (oChequeBookSeries.LastChequeNumber + 1).ToString();
                        txtChequeNoTo.Text = (oChequeBookSeries.LastChequeNumber + (bookSize * noOfBook)).ToString();
                    }
                }
            }
        }
    }
}
