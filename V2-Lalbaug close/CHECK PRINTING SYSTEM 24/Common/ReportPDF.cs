using CPS.Business;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public void Generate(string fileName, IElement title)
        {
            if (_DataGrid.Items.Count > 0)
            {
                using (var stream = new MemoryStream())
                {
                    using (var document = new Document(PageSize.A4, 0, 0, 0, 0))
                    {
                        using (var writer = PdfWriter.GetInstance(document, stream))
                        {
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            // open the document for writing  
                            document.Open();

                            document.Add(title);

                            iTextSharp.text.pdf.draw.LineSeparator line1 = new iTextSharp.text.pdf.draw.LineSeparator(4f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 10);
                            document.Add(new Chunk(line1));

                            var convertToPDF = new DataGridToPDF(_DataGrid);
                            //var table = convertToPDF.GetPDFTable(new float[] { 5, 15, 20, 5, 5, 5, 5, 5, 5, 20, 5, 5 });
                            var table = convertToPDF.GetPDFTable(_ColumnWidth);
                            document.Add(table);
                            document.Close();
                        }
                    }

                    //Uncomment this code to save PDF at particular location.
                    if (!Directory.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PrintDocs")))
                    {
                        Directory.CreateDirectory(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PrintDocs"));
                    }
                    var reportFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PrintDocs", string.Format("{0}.pdf", fileName));
                    System.IO.File.WriteAllBytes(reportFile, stream.ToArray());
                    System.Diagnostics.Process.Start(reportFile);

                    //var printerPreference = PrintJob.GetPrinter();
                    //PrintJob.SendToPrinter(printerPreference, stream.ToArray(), false, false);
                }
                //MessageBox.Show("Report sent to printer!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
