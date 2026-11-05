using CPS.Business;
using CPS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Reflection;

namespace CPS.Views.Reports
{
    /// <summary>
    /// Interaction logic for TotalChequePrintReport.xaml
    /// </summary>
    public partial class TotalChequePrintReport : UserControl
    {
        public TotalChequePrintReport()
        {
            InitializeComponent();
            BindComboBox();
            btnPrint.IsEnabled = false;
        }

        private void BindComboBox()
        {
            var source = BranchMasterDTO.GetLookups();

            source.Insert(0, new LookupItem<int, string>
            {
                Key = 0,
                Value = "ALL"
            });

            cbBrach.ItemsSource = source;
            cbBrach.DisplayMemberPath = "Value";
            cbBrach.SelectedValuePath = "Key";
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

                var repositoryPrintHistory =
                    new PersistenceBase<PrintHistoryDTO>(context);

                var branchId = cbBrach.SelectedValue == null
                    ? 0
                    : (int)cbBrach.SelectedValue;

                var transactionfromDate =
                    string.IsNullOrWhiteSpace(dtPrintDateFrom.Text)
                    ? DateTime.Now.Date
                    : Convert.ToDateTime(dtPrintDateFrom.Text);

                var transactiontoDate =
                    string.IsNullOrWhiteSpace(dtPrintDateTo.Text)
                    ? DateTime.Now.Date
                    : Convert.ToDateTime(dtPrintDateTo.Text);

                var query =
                    from r in repositoryRequest.GetAll()
                    join b in repositoryBranch.GetAll()
                        on r.BranchId equals b.Id
                    join at in repositoryAccountType.GetAll()
                        on r.TransactionCode equals at.Code
                    join ph in repositoryPrintHistory.GetAll()
                        on r.Id equals ph.RequestId
                    where
                        (branchId == 0 || r.BranchId == branchId)
                        && DbFunctions.TruncateTime(ph.CreatedOn)
                            >= transactionfromDate.Date
                        && DbFunctions.TruncateTime(ph.CreatedOn)
                            <= transactiontoDate.Date
                        && ph.PrintType == PrintType.ChequeBook
                        && r.IsPrinted
                    select new
                    {
                        r,
                        b,
                        at,
                        ph,
                        BookSize = ph.ChequeNoTo - ph.ChequeNoFrom + 1
                    };

                var groupedQuery =
                    from o in query
                    group o by new
                    {
                        o.r.BranchId,
                        o.b.Name,
                        o.r.TransactionCode,
                        o.BookSize
                    }
                    into g
                    select new TotalPrintReport
                    {
                        BranchName = g.Key.Name,
                        TransactionCode =
                            g.Key.TransactionCode.ToString(),
                        BookSize = g.Key.BookSize,
                        TotalPrint = g.Count(),
                        TotalLeaves = g.Sum(a => a.BookSize)
                    };

                var response = groupedQuery
                    .OrderBy(o => o.BranchName)
                    .ToList();

                var resultDataSource =
                    new List<TotalPrintReport>();

                if (response.Count > 0)
                {
                    // Get distinct branch names
                    var distinctBranches = response
                        .Select(o => o.BranchName)
                        .Distinct()
                        .ToList();

                    // Grand total
                    var grandTotalPrint = new TotalPrintReport
                    {
                        BranchName = "",
                        TransactionCode = "Grand Total",
                        TotalPrint = response.Sum(o => o.TotalPrint),
                        TotalLeaves = response.Sum(o => o.TotalLeaves)
                    };

                    foreach (var branch in distinctBranches)
                    {
                        resultDataSource.AddRange(
                            response.Where(o =>
                                o.BranchName == branch));

                        // Branch subtotal
                        var totalPrint = new TotalPrintReport
                        {
                            BranchName = branch,
                            TransactionCode = "Sub Total",

                            TotalPrint = response
                                .Where(o => o.BranchName == branch)
                                .Sum(o => o.TotalPrint),

                            TotalLeaves = response
                                .Where(o => o.BranchName == branch)
                                .Sum(o => o.TotalLeaves)
                        };

                        resultDataSource.Add(totalPrint);
                    }

                    resultDataSource.Add(grandTotalPrint);
                }

                dgTotalChequePrint.ItemsSource =
                    resultDataSource;

                btnPrint.IsEnabled =
                    resultDataSource.Count > 0;

                if (response.Count == 0)
                {
                    btnPrint.IsEnabled = false;

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
            int totalRecords = dgTotalChequePrint.Items
                .Cast<object>()
                .Count(item =>
                    item != CollectionView.NewItemPlaceholder);

            if (totalRecords == 0)
            {
                MessageBox.Show(
                    "No records available to print!",
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

            document.FontFamily = new FontFamily("Arial");
            document.FontSize = 15;
            document.FontWeight = FontWeights.Normal;

            // Report title
            System.Windows.Documents.Paragraph title =
                new System.Windows.Documents.Paragraph(
                    new Run(string.Format(
                        "Total Cheque Print - From: {0}  To: {1}",
                        (dtPrintDateFrom.SelectedDate
                            ?? DateTime.Now.Date).ToString("dd MMM yyyy"),
                        (dtPrintDateTo.SelectedDate
                            ?? DateTime.Now.Date).ToString("dd MMM yyyy"))));

            title.FontFamily = new FontFamily("Arial");
            title.FontSize = 26;
            title.FontWeight = FontWeights.Normal;
            title.TextAlignment = TextAlignment.Center;
            title.Margin = new Thickness(0, 0, 0, 10);

            document.Blocks.Add(title);

            // Create table
            Table table = new Table();

            table.CellSpacing = 0;
            table.BorderBrush = Brushes.Black;
            table.BorderThickness = new Thickness(0.5);

            table.TextAlignment = TextAlignment.Center;

            int columnCount =
                dgTotalChequePrint.Columns.Count;

            if (columnCount == 0)
                return;

            // Equal column widths based on printable page width
            double totalWidth =
                printDialog.PrintableAreaWidth
                - document.PagePadding.Left
                - document.PagePadding.Right;

            double columnWidth =
                  (totalWidth / columnCount);

            foreach (DataGridColumn column
                in dgTotalChequePrint.Columns)
            {
                table.Columns.Add(
                    new TableColumn
                    {
                        Width = new GridLength(
                            columnWidth,
                            GridUnitType.Pixel)
                    });
            }

            // Table header
            TableRowGroup headerGroup =
                new TableRowGroup();

            TableRow headerRow =
                new TableRow();

            headerRow.FontFamily =
                new FontFamily("Arial");

            headerRow.FontSize = 12;
            headerRow.FontWeight =
                FontWeights.Normal;

            headerRow.Background =
                Brushes.White;

            foreach (DataGridColumn column
                in dgTotalChequePrint.Columns)
            {
                TableCell cell = new TableCell(
                    new System.Windows.Documents.Paragraph(
                        new Run(column.Header == null
                            ? string.Empty
                            : column.Header.ToString())));

                cell.FontFamily =
                    new FontFamily("Arial");

                cell.FontSize = 12;
                cell.Padding = new Thickness(1);
                cell.FontWeight =
                    FontWeights.Normal;

                

                cell.BorderBrush =
                    Brushes.Black;

                cell.BorderThickness =
                    new Thickness(0.5);

                headerRow.Cells.Add(cell);
            }

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Table data
            TableRowGroup dataGroup =
                new TableRowGroup();

            foreach (object item
                in dgTotalChequePrint.Items)
            {
                if (item ==
                    CollectionView.NewItemPlaceholder)
                {
                    continue;
                }

                TableRow row =
                    new TableRow();

                row.FontFamily =
                    new FontFamily("Arial");

                row.FontSize = 14;
                row.FontWeight =
                    FontWeights.Normal;

                foreach (DataGridColumn column
                    in dgTotalChequePrint.Columns)
                {
                    string cellText =
                        string.Empty;

                    DataGridBoundColumn boundColumn =
                        column as DataGridBoundColumn;

                    if (boundColumn != null)
                    {
                        Binding binding =
                            boundColumn.Binding as Binding;

                        if (binding != null &&
                            binding.Path != null)
                        {
                            cellText = GetPropertyValue(
                                item,
                                binding.Path.Path);
                        }
                    }

                    TableCell cell = new TableCell(
                        new System.Windows.Documents.Paragraph(
                            new Run(cellText)));

                    cell.FontFamily =
                        new FontFamily("Arial");

                    cell.FontSize = 11;
                    cell.FontWeight =
                        FontWeights.Normal;

                    cell.Padding = new Thickness(1);

                    cell.BorderBrush =
                        Brushes.Black;

                    cell.BorderThickness =
                        new Thickness(0.5);

                    // Highlight Sub Total and Grand Total rows
                    if (cellText == "Sub Total" ||
                        cellText == "Grand Total")
                    {
                        row.FontWeight =
                            FontWeights.Bold;
                    }

                    row.Cells.Add(cell);
                }

                dataGroup.Rows.Add(row);
            }

            table.RowGroups.Add(dataGroup);
            document.Blocks.Add(table);

            // Print document
            IDocumentPaginatorSource paginator =
                document;

            printDialog.PrintDocument(
                paginator.DocumentPaginator,
                "Total Cheque Print Report");

            MessageBox.Show(
                "Total cheque print report has been sent to the printer successfully!",
                "Total Cheque Print - Print Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private string GetPropertyValue(
            object obj,
            string propertyPath)
        {
            if (obj == null ||
                string.IsNullOrWhiteSpace(propertyPath))
            {
                return string.Empty;
            }

            object current = obj;

            string[] properties =
                propertyPath.Split('.');

            foreach (string propertyName
                in properties)
            {
                if (current == null)
                    return string.Empty;

                PropertyInfo property =
                    current.GetType().GetProperty(
                        propertyName);

                if (property == null)
                    return string.Empty;

                current = property.GetValue(
                    current,
                    null);
            }

            return current == null
                ? string.Empty
                : current.ToString();
        }
    }

    public class TotalPrintReport
    {
        public string BranchName { get; set; }

        public string TransactionCode { get; set; }

        public int? BookSize { get; set; }

        public int TotalPrint { get; set; }

        public int TotalLeaves { get; set; }
    }
}
