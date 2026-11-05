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
    public partial class RequestLayoutPreference : UserControl
    {
        RequestLayout objRequestLayout = new RequestLayout();

        public RequestLayoutPreference()
        {
            InitializeComponent();
            LoadRequestPreference();
        }

        private void LoadRequestPreference()
        {
            using (var context = new CPSDbContext())
            {
                objRequestLayout = context.RequestLayout.FirstOrDefault();
                if (objRequestLayout != null)
                {
                    chkBranchAddress1.IsChecked = objRequestLayout.branchAddress1Visble;
                    txtBranchAddress1X.Text = objRequestLayout.branchAddress1X.ToString();
                    txtBranchAddress1Y.Text = objRequestLayout.branchAddress1Y.ToString();

                    chkBranchAddress2.IsChecked = objRequestLayout.branchAddress2Visble;
                    txtBranchAddress2X.Text = objRequestLayout.branchAddress2X.ToString();
                    txtBranchAddress2Y.Text = objRequestLayout.branchAddress2Y.ToString();

                    chkChequeFrom1.IsChecked = objRequestLayout.chequeFrom1Visble;
                    txtChequeFrom1X.Text = objRequestLayout.chequeFrom1X.ToString();
                    txtChequeFrom1Y.Text = objRequestLayout.chequeFrom1Y.ToString();

                    chkChequeTo1.IsChecked = objRequestLayout.chequeTo1Visble;
                    txtChequeTo1X.Text = objRequestLayout.chequeTo1X.ToString();
                    txtChequeTo1Y.Text = objRequestLayout.chequeTo1Y.ToString();

                    chkChequeFrom2.IsChecked = objRequestLayout.chequeFrom2Visble;
                    txtChequeFrom2X.Text = objRequestLayout.chequeFrom2X.ToString();
                    txtChequeFrom2Y.Text = objRequestLayout.chequeFrom2Y.ToString();

                    chkChequeTo2.IsChecked = objRequestLayout.chequeTo2Visble;
                    txtChequeTo2X.Text = objRequestLayout.chequeTo2X.ToString();
                    txtChequeTo2Y.Text = objRequestLayout.chequeTo2Y.ToString();

                    chkNameAddress1.IsChecked = objRequestLayout.nameAddress1Visble;
                    txtNameAddress1X.Text = objRequestLayout.nameAddress1X.ToString();
                    txtNameAddress1Y.Text = objRequestLayout.nameAddress1Y.ToString();

                    chkNameAddress2.IsChecked = objRequestLayout.nameAddress2Visble;
                    txtNameAddress2X.Text = objRequestLayout.nameAddress2X.ToString();
                    txtNameAddress2Y.Text = objRequestLayout.nameAddress2Y.ToString();

                    chkAccountNo1.IsChecked = objRequestLayout.accountNo1Visble;
                    txtAccountNo1X.Text = objRequestLayout.accountNo1X.ToString();
                    txtAccountNo1Y.Text = objRequestLayout.accountNo1Y.ToString();

                    chkAccountNo2.IsChecked = objRequestLayout.accountNo2Visble;
                    txtAccountNo2X.Text = objRequestLayout.accountNo2X.ToString();
                    txtAccountNo2Y.Text = objRequestLayout.accountNo2Y.ToString();

                    chkBarcode1.IsChecked = objRequestLayout.barcode1Visble;
                    txtBarcode1X.Text = objRequestLayout.barcode1X.ToString();
                    txtBarcode1Y.Text = objRequestLayout.barcode1Y.ToString();

                    chkBarcode2.IsChecked = objRequestLayout.barcode2Visble;
                    txtBarcode2X.Text = objRequestLayout.barcode2X.ToString();
                    txtBarcode2Y.Text = objRequestLayout.barcode2Y.ToString();

                    chkAuditText1.IsChecked = objRequestLayout.audiText1Visble;
                    txtAuditText1X.Text = objRequestLayout.audiText1X.ToString();
                    txtAuditText1Y.Text = objRequestLayout.audiText1Y.ToString();

                    chkAuditText2.IsChecked = objRequestLayout.audiText2Visble;
                    txtAuditText2X.Text = objRequestLayout.audiText2X.ToString();
                    txtAuditText2Y.Text = objRequestLayout.audiText2Y.ToString();
                }
                else
                {
                    objRequestLayout = new RequestLayout { Id = 1 };
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var isValid = true;
            try
            {
                objRequestLayout.branchAddress1Visble = chkBranchAddress1.IsChecked ?? false;
                objRequestLayout.branchAddress1X = float.Parse(txtBranchAddress1X.Text);
                objRequestLayout.branchAddress1Y = float.Parse(txtBranchAddress1Y.Text);

                objRequestLayout.branchAddress2Visble = chkBranchAddress2.IsChecked ?? false;
                objRequestLayout.branchAddress2X = float.Parse(txtBranchAddress2X.Text);
                objRequestLayout.branchAddress2Y = float.Parse(txtBranchAddress2Y.Text);

                objRequestLayout.chequeFrom1Visble = chkChequeFrom1.IsChecked ?? false;
                objRequestLayout.chequeFrom1X = float.Parse(txtChequeFrom1X.Text);
                objRequestLayout.chequeFrom1Y = float.Parse(txtChequeFrom1Y.Text);

                objRequestLayout.chequeTo1Visble = chkChequeTo1.IsChecked ?? false;
                objRequestLayout.chequeTo1X = float.Parse(txtChequeTo1X.Text);
                objRequestLayout.chequeTo1Y = float.Parse(txtChequeTo1Y.Text);

                objRequestLayout.chequeFrom2Visble = chkChequeFrom2.IsChecked ?? false;
                objRequestLayout.chequeFrom2X = float.Parse(txtChequeFrom2X.Text);
                objRequestLayout.chequeFrom2Y = float.Parse(txtChequeFrom2Y.Text);

                objRequestLayout.chequeTo2Visble = chkChequeTo2.IsChecked ?? false;
                objRequestLayout.chequeTo2X = float.Parse(txtChequeTo2X.Text);
                objRequestLayout.chequeTo2Y = float.Parse(txtChequeTo2Y.Text);

                objRequestLayout.nameAddress1Visble = chkNameAddress1.IsChecked ?? false;
                objRequestLayout.nameAddress1X = float.Parse(txtNameAddress1X.Text);
                objRequestLayout.nameAddress1Y = float.Parse(txtNameAddress1Y.Text);

                objRequestLayout.nameAddress2Visble = chkNameAddress2.IsChecked ?? false;
                objRequestLayout.nameAddress2X = float.Parse(txtNameAddress2X.Text);
                objRequestLayout.nameAddress2Y = float.Parse(txtNameAddress2Y.Text);

                objRequestLayout.accountNo1Visble = chkAccountNo1.IsChecked ?? false;
                objRequestLayout.accountNo1X = float.Parse(txtAccountNo1X.Text);
                objRequestLayout.accountNo1Y = float.Parse(txtAccountNo1Y.Text);

                objRequestLayout.accountNo2Visble = chkAccountNo2.IsChecked ?? false;
                objRequestLayout.accountNo2X = float.Parse(txtAccountNo2X.Text);
                objRequestLayout.accountNo2Y = float.Parse(txtAccountNo2Y.Text);

                objRequestLayout.barcode1Visble = chkBarcode1.IsChecked ?? false;
                objRequestLayout.barcode1X = float.Parse(txtBarcode1X.Text);
                objRequestLayout.barcode1Y = float.Parse(txtBarcode1Y.Text);

                objRequestLayout.barcode2Visble = chkBarcode2.IsChecked ?? false;
                objRequestLayout.barcode2X = float.Parse(txtBarcode2X.Text);
                objRequestLayout.barcode2Y = float.Parse(txtBarcode2Y.Text);

                objRequestLayout.audiText1Visble = chkAuditText1.IsChecked ?? false;
                objRequestLayout.audiText1X = float.Parse(txtAuditText1X.Text);
                objRequestLayout.audiText1Y = float.Parse(txtAuditText1Y.Text);

                objRequestLayout.audiText2Visble = chkAuditText2.IsChecked ?? false;
                objRequestLayout.audiText2X = float.Parse(txtAuditText2X.Text);
                objRequestLayout.audiText2Y = float.Parse(txtAuditText2Y.Text);
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            if (isValid)
            {
                using (var context = new CPSDbContext())
                {
                    context.RequestLayout.AddOrUpdate(key => key.Id, objRequestLayout);
                    context.SaveChanges();
                    MessageBox.Show("Request layout Preferences save sucessfully.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Request layout coordinate values must be decimal.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
