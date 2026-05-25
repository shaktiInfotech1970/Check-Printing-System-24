using CPS.Business;
using CPS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for DayWiseChequePrint.xaml
    /// </summary>
    public partial class DaywiseChequePrint : UserControl
    {
        private int TotalNoOfBooks { get; set; }

        public DaywiseChequePrint()
        {
            InitializeComponent();
            BindComboBox();
            btnPrint.IsEnabled = false;
            var permission = ((mainWindow)App.Current.Windows[0]).LoggedInUserPermission;
        }

        private void BindComboBox()
        {
            cbBrach.ItemsSource = BranchMasterDTO.GetLookups();
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";

            cbAccountType.ItemsSource = AccountTypeDTO.GetLookups2();
            cbAccountType.DisplayMemberPath = "Value";
            cbAccountType.SelectedValuePath = "Key";
        }

        private void btnShowColumns_Click(object sender, RoutedEventArgs e)
        {
            if (cbBrach.SelectedValue != null)
            {
                using (var context = new CPSDbContext())
                {
                    var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                    var repositoryBranch = new PersistenceBase<BranchMasterDTO>(context);
                    var repositoryAccountType = new PersistenceBase<AccountTypeDTO>(context);
                    var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);

                    var branchId = cbBrach.SelectedValue == null ? 0 : (int)cbBrach.SelectedValue;
                    var accountType = cbAccountType.SelectedValue == null ? 0 : (int)cbAccountType.SelectedValue;
                    var transactionDate = string.IsNullOrWhiteSpace(dtTransactionDate.Text) ? System.DateTime.Now.Date : System.Convert.ToDateTime(dtTransactionDate.Text);

                    var query = (from r in repositoryRequest.GetAll()
                                 join b in repositoryBranch.GetAll() on r.BranchId equals b.Id
                                 join at in repositoryAccountType.GetAll() on r.TransactionCode equals at.Code
                                 join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId
                                 where (branchId == 0 || (branchId != 0 && r.BranchId == branchId))
                                 && (accountType == 0 || (accountType != 0 && r.TransactionCode == accountType))
                                 && DbFunctions.TruncateTime(ph.CreatedOn) == transactionDate.Date
                                 && ph.PrintType == PrintType.ChequeBook
                                 select new { Request = r, AccountType = at, Branch = b, PrintHistory = ph }).OrderBy(o => o.Branch.Id);

                    var response = query.ToList();

                    //As Each Record Consist Of One Book
                    response.ForEach(a => a.Request.NoOfChequeBook = 1);
                    TotalNoOfBooks = response.Sum(a => a.Request.NoOfChequeBook);

                    dgDaywiseChequePrint.ItemsSource = response;
                    btnPrint.IsEnabled = true;
                    if (response.Count == 0)
                    {
                        btnPrint.IsEnabled = false;
                        MessageBox.Show("No records found!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select branch", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Paragraph title = new Paragraph(string.Format("Daywise Cheque Print:- Transaction Date: {0}", (dtTransactionDate.SelectedDate ?? DateTime.Now.Date).ToString("dd MMM yyyy")), new Font(Font.FontFamily.HELVETICA, 15));
            title.Alignment = Element.ALIGN_CENTER;

            Paragraph titleName = null;
            Paragraph footer = null;
            if (dgDaywiseChequePrint.Items.Count > 0)
            {
                BaseFont fontVerdana = BaseFont.CreateFont("fonts/VERDANA.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                Font fontBold = new Font(fontVerdana, 9, Font.BOLD);
                titleName = new Paragraph(string.Format("{0}", cbBrach.Text), fontBold);
                titleName.Alignment = Element.ALIGN_CENTER;
                if (dgDaywiseChequePrint.Items.Count > 0)
                {
                    footer = new Paragraph(string.Format("                                Total Books:-       {0}", TotalNoOfBooks), fontBold);
                    footer.PaddingTop = 50F;
                    footer.Alignment = Element.ALIGN_CENTER;
                }
            }


            ReportPDF report = new ReportPDF(dgDaywiseChequePrint, new float[] { 40, 90, 180, 30, 30, 45, 45, 35, 30, 115, 50, 90 });
            report.Generate("DaywiseChequePrint", title, titleName, footer);
        }
    }
}
