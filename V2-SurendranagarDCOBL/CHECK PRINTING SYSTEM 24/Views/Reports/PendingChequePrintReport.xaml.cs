
using CPS.Business;
using CPS.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Reflection;
using System.Windows.Data;

namespace CPS.Views.Reports
{
    public partial class PendingChequePrintReport : UserControl
    {
        public PendingChequePrintReport()
        {
            InitializeComponent();
            BindComboBox();
            btnPrint.IsEnabled = false;
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
            using (var context = new CPSDbContext())
            {
                var repositoryRequest =
                    new PersistenceBase<RequestDTO>(context);

                var repositoryBranch =
                    new PersistenceBase<BranchMasterDTO>(context);

                var repositoryAccountType =
                    new PersistenceBase<AccountTypeDTO>(context);

                var branchId = cbBrach.SelectedValue == null
                    ? 0 : (int)cbBrach.SelectedValue;

                var accountType = cbAccountType.SelectedValue == null
                    ? 0 : (int)cbAccountType.SelectedValue;

                var query =
                    from r in repositoryRequest.GetAll()
                    join b in repositoryBranch.GetAll()
                        on r.BranchId equals b.Id
                    join at in repositoryAccountType.GetAll()
                        on r.TransactionCode equals at.Code
                    where
                        (branchId == 0 ||
                         (branchId != 0 && r.BranchId == branchId))
                        &&
                        (accountType == 0 ||
                         (accountType != 0 &&
                          r.TransactionCode == accountType))
                        &&
                        r.IsPrinted == false
                    orderby r.CreatedOn descending
                    select new
                    {
                        Request = r,
                        AccountType = at,
                        Branch = b
                    };

                var response = query.ToList();

                dgPendingChequePrintReport.ItemsSource = response;

                btnPrint.IsEnabled = response.Count > 0;

                if (response.Count == 0)
                {
                    MessageBox.Show(
                        "No records found!",
                        "Message",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            int totalPendingCheques = dgPendingChequePrintReport.Items
                .Cast<object>()
                .Count(item =>
                    item != CollectionView.NewItemPlaceholder);

            if (totalPendingCheques == 0)
            {
                MessageBox.Show(
                    "No records found!",
                    "Message",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);

                return;
            }

            PrintDialog printDialog = new PrintDialog();

            // Set Landscape for this report only
            var printTicket = printDialog.PrintTicket.Clone();

            printTicket.PageOrientation =
                System.Printing.PageOrientation.Landscape;

            printDialog.PrintTicket = printTicket;

            if (printDialog.ShowDialog() != true)
                return;

            FlowDocument document = new FlowDocument();

            // A4 page layout and margins
            document.PageWidth = printDialog.PrintableAreaWidth;
            document.PageHeight = printDialog.PrintableAreaHeight;
            document.PagePadding = new Thickness(40, 40, 40, 40);
            document.ColumnGap = 0;
            document.ColumnWidth = double.PositiveInfinity;

            // Font settings - First image style
            document.FontFamily = new FontFamily("Arial");
            document.FontSize = 11;
            document.FontWeight = FontWeights.Normal;

            // Title
            System.Windows.Documents.Paragraph title =
                new System.Windows.Documents.Paragraph(
                    new Run("Pending Cheque Print Report"));

            title.FontFamily = new FontFamily("Arial");
            title.FontSize = 24;
            title.FontWeight = FontWeights.Normal;
            title.TextAlignment = TextAlignment.Center;
            title.Margin = new Thickness(0, 0, 0, 65);

            document.Blocks.Add(title);

            // Create table
            Table table = new Table();

            table.CellSpacing = 0;
            table.BorderBrush = Brushes.Black;
            table.BorderThickness = new Thickness(0.5);

            table.TextAlignment = TextAlignment.Center;

            int columnCount =
                dgPendingChequePrintReport.Columns.Count;

            if (columnCount == 0)
                return;

            // Equal column widths according to printable page width
            double totalWidth =
                printDialog.PrintableAreaWidth
                - document.PagePadding.Left
                - document.PagePadding.Right;

            double columnWidth = totalWidth / columnCount;

            foreach (var column in dgPendingChequePrintReport.Columns)
            {
                table.Columns.Add(
                    new TableColumn
                    {
                        Width = new GridLength(
                            columnWidth,
                            GridUnitType.Pixel)
                    });
            }

            // Header
            TableRowGroup headerGroup = new TableRowGroup();
            TableRow headerRow = new TableRow();

            headerRow.FontFamily = new FontFamily("Arial");
            headerRow.FontSize = 11;
            headerRow.FontWeight = FontWeights.Normal;
            headerRow.Background = Brushes.White;

            foreach (var column in dgPendingChequePrintReport.Columns)
            {
                TableCell cell = new TableCell(
                    new System.Windows.Documents.Paragraph(
                        new Run(column.Header?.ToString() ?? "")));

                cell.FontFamily = new FontFamily("Arial");
                cell.FontSize = 11;
                cell.FontWeight = FontWeights.Normal;
                cell.BorderBrush = Brushes.Black;
                cell.BorderThickness = new Thickness(0.5);
                cell.Padding = new Thickness(2);

                headerRow.Cells.Add(cell);
            }

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Data rows
            TableRowGroup dataGroup = new TableRowGroup();

            foreach (var item in dgPendingChequePrintReport.Items)
            {
                if (item == CollectionView.NewItemPlaceholder)
                    continue;

                TableRow row = new TableRow();

                row.FontFamily = new FontFamily("Arial");
                row.FontSize = 11;
                row.FontWeight = FontWeights.Normal;

                foreach (var column in dgPendingChequePrintReport.Columns)
                {
                    string value = "";

                    if (column is DataGridBoundColumn boundColumn
                        && boundColumn.Binding is Binding binding)
                    {
                        string path = binding.Path.Path;
                        object current = item;

                        foreach (string propertyName in path.Split('.'))
                        {
                            if (current == null)
                                break;

                            PropertyInfo property =
                                current.GetType().GetProperty(propertyName);

                            current = property != null
                                ? property.GetValue(current, null)
                                : null;
                        }

                        value = current?.ToString() ?? "";
                    }

                    TableCell cell = new TableCell(
                        new System.Windows.Documents.Paragraph(
                            new Run(value)));

                    cell.FontFamily = new FontFamily("Arial");
                    cell.FontSize = 11;
                    cell.FontWeight = FontWeights.Normal;
                    cell.BorderBrush = Brushes.Black;
                    cell.BorderThickness = new Thickness(0.5);
                    cell.Padding = new Thickness(2);

                    row.Cells.Add(cell);
                }

                dataGroup.Rows.Add(row);
            }

            table.RowGroups.Add(dataGroup);
            document.Blocks.Add(table);

            // Footer
            System.Windows.Documents.Paragraph footer =
                new System.Windows.Documents.Paragraph(
                    new Run(
                        "Total Pending Cheques: " +
                        totalPendingCheques));

            footer.FontFamily = new FontFamily("Arial");
            footer.FontSize = 12;
            footer.FontWeight = FontWeights.Bold;
            footer.TextAlignment = TextAlignment.Center;
            footer.Margin = new Thickness(0, 15, 0, 0);

            document.Blocks.Add(footer);

            // Direct printing
            printDialog.PrintDocument(
                ((IDocumentPaginatorSource)document).DocumentPaginator,
                "Pending Cheque Print Report");

            // Success message
            MessageBox.Show(
                "Pending cheque print report has been sent to the printer successfully!",
                "Pending Cheque Print - Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
