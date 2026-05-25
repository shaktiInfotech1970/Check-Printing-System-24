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

                    chkCustom1.IsChecked = objChequeLayout.custom1Visible;
                    txtCustom1X.Text = objChequeLayout.custom1X.ToString();
                    txtCustom1Y.Text = objChequeLayout.custom1Y.ToString();
                    txtCustom1Text.Text = objChequeLayout.custom1Text==null?string.Empty: objChequeLayout.custom1Text.ToString();

                    chkCustom2.IsChecked = objChequeLayout.custom2Visible;
                    txtCustom2X.Text = objChequeLayout.custom2X.ToString();
                    txtCustom2Y.Text = objChequeLayout.custom2Y.ToString();
                    txtCustom2Text.Text = objChequeLayout.custom2Text == null ? string.Empty : objChequeLayout.custom2Text.ToString();

                    chkCustom3.IsChecked = objChequeLayout.custom3Visible;
                    txtCustom3X.Text = objChequeLayout.custom3X.ToString();
                    txtCustom3Y.Text = objChequeLayout.custom3Y.ToString();
                    txtCustom3Text.Text = objChequeLayout.custom3Text == null ? string.Empty : objChequeLayout.custom3Text.ToString();

                    chkCustom4.IsChecked = objChequeLayout.custom4Visible;
                    txtCustom4X.Text = objChequeLayout.custom4X.ToString();
                    txtCustom4Y.Text = objChequeLayout.custom4Y.ToString();
                    txtCustom4Text.Text = objChequeLayout.custom4Text == null ? string.Empty : objChequeLayout.custom4Text.ToString();


                    chkCustom5.IsChecked = objChequeLayout.custom5Visible;
                    txtCustom5X.Text = objChequeLayout.custom5X.ToString();
                    txtCustom5Y.Text = objChequeLayout.custom5Y.ToString();
                    txtCustom5Text.Text = objChequeLayout.custom5Text == null ? string.Empty : objChequeLayout.custom5Text.ToString();
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

                objChequeLayout.custom1Visible = chkCustom1.IsChecked ?? false;
                objChequeLayout.custom1X = float.Parse(txtCustom1X.Text);
                objChequeLayout.custom1Y = float.Parse(txtCustom1Y.Text);
                objChequeLayout.custom1Text = txtCustom1Text.Text.Trim();

                objChequeLayout.custom2Visible = chkCustom2.IsChecked ?? false;
                objChequeLayout.custom2X = float.Parse(txtCustom2X.Text);
                objChequeLayout.custom2Y = float.Parse(txtCustom2Y.Text);
                objChequeLayout.custom2Text = txtCustom2Text.Text.Trim();

                objChequeLayout.custom3Visible = chkCustom3.IsChecked ?? false;
                objChequeLayout.custom3X = float.Parse(txtCustom3X.Text);
                objChequeLayout.custom3Y = float.Parse(txtCustom3Y.Text);
                objChequeLayout.custom3Text = txtCustom3Text.Text.Trim();

                objChequeLayout.custom4Visible = chkCustom4.IsChecked ?? false;
                objChequeLayout.custom4X = float.Parse(txtCustom4X.Text);
                objChequeLayout.custom4Y = float.Parse(txtCustom4Y.Text);
                objChequeLayout.custom4Text = txtCustom4Text.Text.Trim();

                objChequeLayout.custom5Visible = chkCustom5.IsChecked ?? false;
                objChequeLayout.custom5X = float.Parse(txtCustom5X.Text);
                objChequeLayout.custom5Y = float.Parse(txtCustom5Y.Text);
                objChequeLayout.custom5Text = txtCustom5Text.Text.Trim();



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
