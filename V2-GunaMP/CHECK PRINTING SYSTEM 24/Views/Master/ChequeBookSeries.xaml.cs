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
    /// Interaction logic for ChequeBookSeries.xaml
    /// </summary>
    public partial class ChequeBookSeries : UserControl
    {
        public ChequeBookSeries()
        {
            InitializeComponent();
            LoadData();
            BindComboBox();
            BindDataGrid();
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            cbAccountType.ItemsSource = AccountTypeDTO.GetLookups();
            cbAccountType.DisplayMemberPath = "Value";
            cbAccountType.SelectedValuePath = "Key";
        }

        private void LoadData()
        {
            txtStartChequeNumber.Text = "1";
            txtEndChequeNumber.Text = "999999";
            txtLastChequeNumber.Text = "0";
            txtAvailableCheques.Text = "999999";
        }

        private void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var chequeBookSeriesRepository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                var accountTypeRepository = new PersistenceBase<AccountTypeDTO>(context);

                var data = (from cs in chequeBookSeriesRepository.GetAll()
                            join br in branchRepository.GetAll() on cs.BranchId equals br.Id
                            join at in accountTypeRepository.GetAll() on cs.AccountTypeId equals at.Id
                            select new
                            {
                                Id = cs.Id,
                                CreatedBy = cs.CreatedBy,
                                CreatedOn = cs.CreatedOn,
                                UpdatedBy = cs.UpdatedBy,
                                UpdatedOn = cs.UpdatedOn,
                                IsDeleted = cs.IsDeleted,
                                BranchId = cs.BranchId,
                                BranchName = br.Name,
                                AccountTypeId = cs.AccountTypeId,
                                AccountType = at.Name,
                                AvailableCheques = cs.AvailableCheques,
                                StartChequeNumber = cs.StartChequeNumber,
                                EndChequeNumber = cs.EndChequeNumber,
                                LastChequeNumber = cs.LastChequeNumber
                            }).ToList().Select(s => new ChequeBookSeriesDTO {
                                Id = s.Id,
                                CreatedBy = s.CreatedBy,
                                CreatedOn = s.CreatedOn,
                                UpdatedBy = s.UpdatedBy,
                                UpdatedOn = s.UpdatedOn,
                                IsDeleted = s.IsDeleted,
                                BranchId = s.BranchId,
                                BranchName = s.BranchName,
                                AccountTypeId = s.AccountTypeId,
                                AccountType = s.AccountType,
                                AvailableCheques = s.AvailableCheques,
                                StartChequeNumber = s.StartChequeNumber,
                                EndChequeNumber = s.EndChequeNumber,
                                LastChequeNumber = s.LastChequeNumber
                            });

                if (data != null)
                {
                    dgChequeBookSeries.ItemsSource = data;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var oChequeBookSeries = new ChequeBookSeriesDTO();

            if (cbBrach.SelectedValue == null)
            {
                MessageBox.Show("Please select branch.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (cbAccountType.SelectedValue == null)
            {
                MessageBox.Show("Please select account type.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtStartChequeNumber.Text))
            {
                MessageBox.Show("Please enter start cheque number.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtEndChequeNumber.Text))
            {
                MessageBox.Show("Please enter end cheque number.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (btnSave.Tag != null)
                {
                    oChequeBookSeries = (ChequeBookSeriesDTO)btnSave.Tag;
                }

                oChequeBookSeries.BranchId = (int)cbBrach.SelectedValue;
                oChequeBookSeries.AccountTypeId = (int)cbAccountType.SelectedValue;
                oChequeBookSeries.StartChequeNumber = Convert.ToInt32(txtStartChequeNumber.Text);
                oChequeBookSeries.EndChequeNumber = Convert.ToInt32(txtEndChequeNumber.Text);
                oChequeBookSeries.LastChequeNumber = Convert.ToInt32(txtLastChequeNumber.Text);
                oChequeBookSeries.AvailableCheques = Convert.ToInt32(txtEndChequeNumber.Text);

                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                    var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (repository.SaveOrUpdate(oChequeBookSeries, errors))
                    {
                        context.SaveChanges();
                        MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        CPS.Common.Helper.ClearFormData(this);
                        LoadData();
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

        private void dgChequeBookSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var oChequeBookSeriesDTO = (ChequeBookSeriesDTO)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                btnSave.Tag = oChequeBookSeriesDTO;
                txtStartChequeNumber.Text = oChequeBookSeriesDTO.StartChequeNumber.ToString();
                txtEndChequeNumber.Text = oChequeBookSeriesDTO.EndChequeNumber.ToString();
                txtLastChequeNumber.Text = oChequeBookSeriesDTO.LastChequeNumber.ToString();
                txtAvailableCheques.Text = oChequeBookSeriesDTO.AvailableCheques.ToString();

                cbBrach.SelectedValue = oChequeBookSeriesDTO.BranchId;
                cbAccountType.SelectedValue = oChequeBookSeriesDTO.AccountTypeId;
                btnSave.Content = "Save";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            btnSave.Content = "Add";
            LoadData();
            dgChequeBookSeries.SelectedIndex = -1;
            btnSave.Tag = null;
        }
    }
}
