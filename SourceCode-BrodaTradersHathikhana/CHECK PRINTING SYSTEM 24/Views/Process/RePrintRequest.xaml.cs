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
    /// Interaction logic for PrintChequeBook.xaml
    /// </summary>
    public partial class RePrintRequest : UserControl
    {
        //BusinessServiceBase<RequestDTO> service = new BusinessServiceBase<RequestDTO>();

        public RePrintRequest()
        {
            InitializeComponent();
        }

        private void btnShowColumn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPrintJobNo.Text))
            {
                var printJobNo = Convert.ToInt32(txtPrintJobNo.Text);

                using (var context = new CPSDbContext())
                {
                    var requestRepository = new PersistenceBase<RequestDTO>(context);
                    var branchRepository = new PersistenceBase<BranchMasterDTO>(context);
                    var accountTypeRepository = new PersistenceBase<AccountTypeDTO>(context);

                    var response = (from r in requestRepository.FilterBy(w=> w.IsPrinted == true)
                                    join b in branchRepository.GetAll() on r.BranchId equals b.Id
                                    join at in accountTypeRepository.GetAll() on r.TransactionCode equals at.Code
                                    where (r.PrintJobNo == printJobNo)
                                    select new PrintRequest { Request = r, AccountType = at, Branch = b }).ToList();

                    dgRequestEntry.ItemsSource = response;
                    if (response.Count == 0)
                    {
                        MessageBox.Show("Print job not found.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter Job No.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequestEntry.ItemsSource != null)
            {
                var requests = dgRequestEntry.ItemsSource.Cast<PrintRequest>().Where(w => w.Request.IsSelected);
                if (requests.Count() <= 0)
                {
                    MessageBox.Show("Please select requests to print", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (PrintJob.RePrintRequest(requests))
                    {
                        MessageBox.Show("Requests have been printed.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        btnShowColumn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    }
                    else
                    {
                        MessageBox.Show("Print job is not created", "Warning!", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
            dgRequestEntry.SelectedIndex = -1;
            dgRequestEntry.ItemsSource = null;
        }
    }
}
