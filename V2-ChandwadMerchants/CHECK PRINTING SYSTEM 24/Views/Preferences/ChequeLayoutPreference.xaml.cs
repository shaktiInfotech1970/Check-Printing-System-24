using CPS.Business;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using System.Data.Entity.Migrations;

namespace CPS.Views.Preferences
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class ChequeLayoutPreference : UserControl
    {
        ChequeLayout objChequeLayout = new ChequeLayout();

        public ChequeLayoutPreference()
        {
            InitializeComponent();
            LoadRequestPreference();
        }

        private void LoadRequestPreference()
        {
            using (var context = new CPSDbContext())
            {
                objChequeLayout = context.ChequeLayout.FirstOrDefault();
                if (objChequeLayout != null)
                {
                    chkBranchAddress.IsChecked = objChequeLayout.branchAddressVisble;
                    txtBranchAddressX.Text = objChequeLayout.branchAddressX.ToString();
                    txtBranchAddressY.Text = objChequeLayout.branchAddressY.ToString();

                    chkIFSC.IsChecked = objChequeLayout.ifscVisble;
                    txtIFSCX.Text = objChequeLayout.ifscX.ToString();
                    txtIFSCY.Text = objChequeLayout.ifscY.ToString();

                    chkOrderOrBearer.IsChecked = objChequeLayout.orderOrBarerVisble;
                    txtOrderOrBearerX.Text = objChequeLayout.orderOrBarerX.ToString();
                    txtOrderOrBearerY.Text = objChequeLayout.orderOrBarerY.ToString();

                    chkAccountNo.IsChecked = objChequeLayout.accountNoVisble;
                    txtAccountNoX.Text = objChequeLayout.accountNoX.ToString();
                    txtAccountNoY.Text = objChequeLayout.accountNoY.ToString();

                    chkStamp.IsChecked = objChequeLayout.stampVisble;
                    txtStampX.Text = objChequeLayout.stampX.ToString();
                    txtStampY.Text = objChequeLayout.stampY.ToString();

                    chkMICR.IsChecked = objChequeLayout.micrVisble;
                    txtMICRX.Text = objChequeLayout.micrX.ToString();
                    txtMICRY.Text = objChequeLayout.micrY.ToString();

                    chkBarcode.IsChecked = objChequeLayout.barcodeVisble;
                    txtBarcodeX.Text = objChequeLayout.barcodeX.ToString();
                    txtBarcodeY.Text = objChequeLayout.barcodeY.ToString();

                    chkAuditText.IsChecked = objChequeLayout.audiTextVisble;
                    txtAuditTextX.Text = objChequeLayout.audiTextX.ToString();
                    txtAuditTextY.Text = objChequeLayout.audiTextY.ToString();

                    chkAccountPayee.IsChecked = objChequeLayout.accountPayeeVisble;
                    txtAccountPayeeX.Text = objChequeLayout.accountPayeeX.ToString();
                    txtAccountPayeeY.Text = objChequeLayout.accountPayeeY.ToString();
                }
                else
                {
                    objChequeLayout = new ChequeLayout { Id = 1 };
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var isValid = true;
            try
            {
                objChequeLayout.branchAddressVisble = chkBranchAddress.IsChecked ?? false;
                objChequeLayout.branchAddressX = float.Parse(txtBranchAddressX.Text);
                objChequeLayout.branchAddressY = float.Parse(txtBranchAddressY.Text);

                objChequeLayout.ifscVisble = chkIFSC.IsChecked ?? false;
                objChequeLayout.ifscX = float.Parse(txtIFSCX.Text);
                objChequeLayout.ifscY = float.Parse(txtIFSCY.Text);

                objChequeLayout.orderOrBarerVisble = chkOrderOrBearer.IsChecked ?? false;
                objChequeLayout.orderOrBarerX = float.Parse(txtOrderOrBearerX.Text);
                objChequeLayout.orderOrBarerY = float.Parse(txtOrderOrBearerY.Text);

                objChequeLayout.accountNoVisble = chkAccountNo.IsChecked ?? false;
                objChequeLayout.accountNoX = float.Parse(txtAccountNoX.Text);
                objChequeLayout.accountNoY = float.Parse(txtAccountNoY.Text);

                objChequeLayout.stampVisble = chkStamp.IsChecked ?? false;
                objChequeLayout.stampX = float.Parse(txtStampX.Text);
                objChequeLayout.stampY = float.Parse(txtStampY.Text);

                objChequeLayout.micrVisble = chkMICR.IsChecked ?? false;
                objChequeLayout.micrX = float.Parse(txtMICRX.Text);
                objChequeLayout.micrY = float.Parse(txtMICRY.Text);

                objChequeLayout.barcodeVisble = chkBarcode.IsChecked ?? false;
                objChequeLayout.barcodeX = float.Parse(txtBarcodeX.Text);
                objChequeLayout.barcodeY = float.Parse(txtBarcodeY.Text);

                objChequeLayout.audiTextVisble = chkAuditText.IsChecked ?? false;
                objChequeLayout.audiTextX = float.Parse(txtAuditTextX.Text);
                objChequeLayout.audiTextY = float.Parse(txtAuditTextY.Text);

                objChequeLayout.accountPayeeVisble = chkAccountPayee.IsChecked ?? false;
                objChequeLayout.accountPayeeX = float.Parse(txtAccountPayeeX.Text);
                objChequeLayout.accountPayeeY = float.Parse(txtAccountPayeeY.Text);
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            if (isValid)
            {
                using (var context = new CPSDbContext())
                {
                    context.ChequeLayout.AddOrUpdate(key => key.Id, objChequeLayout);
                    context.SaveChanges();
                    MessageBox.Show("Cheque layout Preferences save sucessfully.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Cheque layout values are not valid.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
