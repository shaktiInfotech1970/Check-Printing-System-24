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
    /// Interaction logic for BankMaster.xaml
    /// </summary>
    public partial class BankMaster : UserControl
    {
        public BankMaster()
        {
            InitializeComponent();
            LoadDetail();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var oBank = new BankMasterDTO();

            if (btnSave.Tag != null)
            {
                oBank = (BankMasterDTO)btnSave.Tag;
            }

            //oBank.Code = txtCode.Text;
            oBank.Name = txtName.Text;
            oBank.AddressLine1 = txtAddress1.Text;
            oBank.AddressLine2 = txtAddress2.Text;
            oBank.AddressLine3 = txtAddress3.Text;
            oBank.City = txtCity.Text;
            oBank.PostalCode = txtPinCode.Text;
            oBank.State = txtState.Text;
            oBank.Country = txtCountry.Text;
            oBank.Phone = txtPhone.Text;
            oBank.Mobile = txtMobile.Text;
            oBank.Email = txtEmail.Text == string.Empty ? null : txtEmail.Text;
            oBank.Fax = txtFax.Text;
            oBank.WebAddress = txtWebAddress.Text;
            using (var context = new CPSDbContext())
            {
                var bankRepository = new PersistenceBase<BankMasterDTO>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (bankRepository.SaveOrUpdate(oBank, errors))
                {
                    context.SaveChanges();
                    MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void LoadDetail()
        {
            using (var context = new CPSDbContext())
            {
                var bankRepository = new PersistenceBase<BankMasterDTO>(context);
                var bankMaster = bankRepository.GetAll().FirstOrDefault();
                if (bankMaster != null)
                {
                    txtCode.Text = bankMaster.Code;
                    txtName.Text = bankMaster.Name;
                    txtAddress1.Text = bankMaster.AddressLine1;
                    txtAddress2.Text = bankMaster.AddressLine2;
                    txtAddress3.Text = bankMaster.AddressLine3;
                    txtCity.Text = bankMaster.City;
                    txtPinCode.Text = bankMaster.PostalCode;
                    txtState.Text = bankMaster.State;
                    txtCountry.Text = bankMaster.Country;
                    txtPhone.Text = bankMaster.Phone;
                    txtMobile.Text = bankMaster.Mobile;
                    txtEmail.Text = bankMaster.Email;
                    txtFax.Text = bankMaster.Fax;
                    txtWebAddress.Text = bankMaster.WebAddress;
                    btnSave.Tag = bankMaster;
                }
            }
        }
    }
}
