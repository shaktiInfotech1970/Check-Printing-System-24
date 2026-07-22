using CPS.Business;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace CPS.Common
{
    public class ReportPDF
    {
        private DataGrid _DataGrid;
        private float[] _ColumnWidth;

        public ReportPDF(DataGrid dataGrid, float[] columnWidth)
        {
            _DataGrid = dataGrid;
            _ColumnWidth = columnWidth;
        }

        public void Generate(string fileName, IElement title, IElement titleName = null, IElement footer = null)
        {
            if (_DataGrid.Items.Count > 0)
            {
                using (var stream = new MemoryStream())
                {
                    using (var document = new Document(PageSize.A4, 0, 0, 0, 0))
                    {
                        using (var writer = PdfWriter.GetInstance(document, stream))
                        {
                            document.SetPageSize(PageSize.A4.Rotate());
                            document.Open();

                            // Main Title
                            if (title != null)
                            {
                                document.Add(title);
                            }

                            // Separator Line
                            var line = new iTextSharp.text.pdf.draw.LineSeparator(
                                4f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 10);
                            document.Add(new Chunk(line));

                            // Optional Subtitle / Branch Name
                            if (titleName != null)
                            {
                                document.Add(titleName);
                                document.Add(Chunk.NEWLINE);
                            }

                            // Report Table
                            var convertToPDF = new DataGridToPDF(_DataGrid);
                            var table = convertToPDF.GetPDFTable(_ColumnWidth);
                            document.Add(table);

                            // Optional Footer
                            if (footer != null)
                            {
                                document.Add(Chunk.NEWLINE);
                                document.Add(footer);
                            }

                            document.Close();
                        }
                    }

                    string printDocsFolder = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "PrintDocs");

                    if (!Directory.Exists(printDocsFolder))
                    {
                        Directory.CreateDirectory(printDocsFolder);
                    }

                    string reportFile = Path.Combine(printDocsFolder, fileName + ".pdf");

                    File.WriteAllBytes(reportFile, stream.ToArray());

                    System.Diagnostics.Process.Start(reportFile);

                    // Uncomment if direct printing is required.
                    // var printerPreference = PrintJob.GetPrinter();
                    // PrintJob.SendToPrinter(printerPreference, stream.ToArray(), false, false);
                }
            }
        }
    }
}