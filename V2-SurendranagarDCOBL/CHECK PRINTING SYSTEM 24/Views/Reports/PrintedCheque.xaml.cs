
using CPS.Business;
using CPS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Reflection;
using System.Windows.Data;

namespace CPS.Views.Reports
{
    public partial class PrintedCheque : UserControl
    {
        public PrintedCheque()
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

            var source2 = AccountTypeDTO.GetLookups2();

            source2.Insert(0, new LookupItem<int, string>
            {
                Key = 0,
                Value = "ALL"
            });

            cbAccountType.ItemsSource = source2;
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

                var repositoryPrintHistory =
                    new PersistenceBase<PrintHistoryDTO>(context);

                var branchId = cbBrach.SelectedValue == null
                    ? 0 : (int)cbBrach.SelectedValue;

                var accountType = cbAccountType.SelectedValue == null
                    ? 0 : (int)cbAccountType.SelectedValue;

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
                        (branchId == 0 ||
                         (branchId != 0 && r.BranchId == branchId))
                        &&
                        (accountType == 0 ||
                         (accountType != 0 &&
                          r.TransactionCode == accountType))
                        &&
                        DbFunctions.TruncateTime(ph.CreatedOn)
                            >= transactionfromDate.Date
                        &&
                        DbFunctions.TruncateTime(ph.CreatedOn)
                            <= transactiontoDate.Date
                        &&
                        ph.PrintType == PrintType.ChequeBook
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
                    } into g
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

                var resultDataSource = new List<TotalPrintReport>();

                if (response.Count > 0)
                {
                    var olDistinctBranch = response
                        .Select(o => o.BranchName)
                        .Distinct()
                        .ToList();

                    var oGrandTotalPrint = new TotalPrintReport();

                    oGrandTotalPrint.BranchName = "";
                    oGrandTotalPrint.TransactionCode = "Grand Total";
                    oGrandTotalPrint.TotalPrint =
                        response.Sum(o => o.TotalPrint);
                    oGrandTotalPrint.TotalLeaves =
                        response.Sum(o => o.TotalLeaves);

                    foreach (var branch in olDistinctBranch)
                    {
                        resultDataSource.AddRange(
                            response.Where(o => o.BranchName == branch));

                        var oTotalPrint = new TotalPrintReport();

                        oTotalPrint.BranchName = branch;
                        oTotalPrint.TransactionCode = "Sub Total";

                        oTotalPrint.TotalPrint =
                            response.Where(o => o.BranchName == branch)
                                .Sum(o => o.TotalPrint);

                        oTotalPrint.TotalLeaves =
                            response.Where(o => o.BranchName == branch)
                                .Sum(o => o.TotalLeaves);

                        resultDataSource.Add(oTotalPrint);
                    }

                    resultDataSource.Add(oGrandTotalPrint);
                }

                dgPrintedCheque.ItemsSource = resultDataSource;

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
            int totalRecords = dgPrintedCheque.Items
                .Cast<object>()
                .Count(item =>
                    item != CollectionView.NewItemPlaceholder);

            if (totalRecords == 0)
            {
                MessageBox.Show(
                    "No records available to print!",
                    "Message",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

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
                    new Run(
                        string.Format(
                            "Printed Cheque - From: {0} - To: {1}",
                            (dtPrintDateFrom.SelectedDate
                                ?? DateTime.Now.Date).ToString("dd MMM yyyy"),
                            (dtPrintDateTo.SelectedDate
                                ?? DateTime.Now.Date).ToString("dd MMM yyyy"))));

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

            int columnCount = dgPrintedCheque.Columns.Count;

            if (columnCount == 0)
                return;

            // Equal column widths according to printable page width
            double totalWidth =
                printDialog.PrintableAreaWidth
                - document.PagePadding.Left
                - document.PagePadding.Right;

            double columnWidth = totalWidth / columnCount;

            foreach (var column in dgPrintedCheque.Columns)
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

            foreach (var column in dgPrintedCheque.Columns)
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

            // Data rows including Subtotal and Grand Total
            TableRowGroup dataGroup = new TableRowGroup();

            foreach (var item in dgPrintedCheque.Items)
            {
                if (item == CollectionView.NewItemPlaceholder)
                    continue;

                TableRow row = new TableRow();

                row.FontFamily = new FontFamily("Arial");
                row.FontSize = 11;
                row.FontWeight = FontWeights.Normal;

                foreach (var column in dgPrintedCheque.Columns)
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

                    // Bold Sub Total and Grand Total rows
                    if (value == "Sub Total" || value == "Grand Total")
                    {
                        row.FontWeight = FontWeights.Bold;
                    }

                    row.Cells.Add(cell);
                }

                dataGroup.Rows.Add(row);
            }

            table.RowGroups.Add(dataGroup);
            document.Blocks.Add(table);

            // Direct printing
            printDialog.PrintDocument(
                ((IDocumentPaginatorSource)document).DocumentPaginator,
                "Printed Cheque Report");

            // Success message
            MessageBox.Show(
                "Printed cheque report has been sent to the printer successfully!",
                "Printed Cheque - Print Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
