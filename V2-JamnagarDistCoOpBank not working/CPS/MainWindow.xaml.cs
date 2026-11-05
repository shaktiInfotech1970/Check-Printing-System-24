using CPS.Business;
using CPS.Views.Master;
using CPS.Views.Process;
using CPS.Views.Reports;
using CPS.Views.Security;
using log4net;
using System.Windows;

namespace CPS
{
    /// <summary>
    /// Interaction logic for mainWindow.xaml
    /// </summary>
    public partial class mainWindow : Window
    {
        public UserPermission LoggedInUserPermission;
        public mainWindow(UserPermission _LoggedInUserPermission)
        {
            LoggedInUserPermission = new UserPermission();
            LoggedInUserPermission = _LoggedInUserPermission;
            InitializeComponent();
            var mitem = MainMenu.Items;
            var count = mitem.Count;

            for (int i = MainMenu.Items.Count - 2; i >= 0; i--)
            {
                System.Windows.Controls.MenuItem mainItem =
                    (System.Windows.Controls.MenuItem)MainMenu.Items[i];
                if (mainItem.Items.Count > 0)
                {
                    for (int j = mainItem.Items.Count - 1; j >= 0; j--)
                    {
                        if (((System.Windows.Controls.MenuItem)mainItem.Items[j]).Name != "subMenuChangePassword")
                        {
                            if (!LoggedInUserPermission.HasAny((Common.Page)System.Enum.Parse(typeof(Common.Page), ((System.Windows.Controls.MenuItem)mainItem.Items[j]).Name.Replace("subMenu", ""), true)))
                            {
                                mainItem.Items.Remove(mainItem.Items[j]);
                            }
                        }
                    }

                    if (mainItem.Items.Count == 0)
                    {
                        MainMenu.Items.Remove(mainItem);
                    }
                }

            }
        }

        private void subMenuBankMaster_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Bank Master";
            ucContainer.Children.Add(new BankMaster());
        }

        private void subMenuBranchMaster_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Branch Master";
            ucContainer.Children.Add(new BranchMaster());
        }

        private void subMenuChequeBookSeries_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Cheque Book Series";
            ucContainer.Children.Add(new ChequeBookSeries());
        }

        private void subMenuAccountType_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Account Type";
            ucContainer.Children.Add(new AccountType());
        }

        private void subMenuPermission_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Permission";
            ucContainer.Children.Add(new Permissions());
        }

        private void subMenuUserMaster_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "User Master";
            ucContainer.Children.Add(new UserMaster());
        }

        private void subMenuChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Change Password";
            ucContainer.Children.Add(new ChangePassword());
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void subMenuDataImport_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Import";
            ucContainer.Children.Add(new DataImport());
        }

        private void subMenuRequestDataEntry_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Request Data Entry";
            ucContainer.Children.Add(new RequestEntry());
        }

        private void subMenuPrintChequeBook_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Print Cheque Book";
            ucContainer.Children.Add(new PrintChequeBook());
        }

        private void subMenuReprintChequeBook_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Re-Print Cheque Book";
            ucContainer.Children.Add(new RePrintChequeBook());
        }

        private void subMenuReprintRequest_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Re-Print Request";
            ucContainer.Children.Add(new RePrintRequest());
        }

        private void subMenuReprintSinglePage_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Re-Print Single Page";
            ucContainer.Children.Add(new RePrintSinglePage());
        }

        private void subMenuDataExport_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Data Export";
            ucContainer.Children.Add(new DataExport());
        }
        private void subMenuRemoveRequestData_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Remove Requested Data";
            ucContainer.Children.Add(new RemoveRequestData());
        }

        private void subMenuDaywiseChequePrint_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Daywise Cheque Print Report";
            ucContainer.Children.Add(new DaywiseChequePrintRDLC());
        }

        private void subMenuPrintedChequePrintFile_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Printed Cheque Report";
            ucContainer.Children.Add(new PrintedChequePrintFileRDLC());
        }

        private void subMenuReprintedCheque_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Re-Printed Cheque Report";
            ucContainer.Children.Add(new ReprintedChequeRDLC());
        }

        private void subMenuReprintedChequeSinglePage_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Re-Printed Cheque Report Single Page";
            ucContainer.Children.Add(new ReprintedChequeSinglePageRDLC());
        }

        private void subMenuPendingChequePrint_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Pending Cheque Print Report";
            ucContainer.Children.Add(new PendingChequePrintRDLC());
        }

        private void subMenuTotalPrintCheque_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Total Cheque Print Report";
           ucContainer.Children.Add(new TotalChequePrintRDLC());
         
        }

        private void subMenuTotalReprintCheque_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Total Re-Printed Cheque Report";
            ucContainer.Children.Add(new TotalReprintedChequeRDLC());
        }

        private void subMenuPrintedChequeSeries_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Cheque Series Report";
            ucContainer.Children.Add(new PrintedChequeSeriesRDLC());
        }

        private void subMenuPrintedCheque_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Printed Cheque Report";
            ucContainer.Children.Add(new PrintedChequeRDLC());
        }
        private void subMenuSearch_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Search Report";
            ucContainer.Children.Add(new Search());
        }
        private void subMenuPreferences_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Preferences";
            ucContainer.Children.Add(new CPS.Views.Preferences.Preferences());
        }

        private void subMenuDatabaseBackup_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Database Backup";
            ucContainer.Children.Add(new CPS.Views.Preferences.Backup());
        }

        private void subMenuRequestLayoutPreference_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Request Layout Preference";
            ucContainer.Children.Add(new CPS.Views.Preferences.RequestLayoutPreference());
        }

        private void subMenuChequeLayoutPreference_Click(object sender, RoutedEventArgs e)
        {
            ucContainer.Children.Clear();
            SubTitle.Text = "Cheque Layout Preference";
            ucContainer.Children.Add(new CPS.Views.Preferences.ChequeLayoutPreference());
        }
    }
}
