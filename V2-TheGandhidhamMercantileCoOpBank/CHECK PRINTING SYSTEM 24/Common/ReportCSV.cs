using CPS.Business;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Common
{
    public class ReportCSV
    {
        private DataGrid _DataGrid;
        public ReportCSV(DataGrid dataGrid)
        {
            _DataGrid = dataGrid;
        }

        public void Generate(string fileName)
        {
            if (_DataGrid.Items.Count > 0)
            {
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                    {
                        var header = "";
                        foreach (var column in _DataGrid.Columns)
                        {
                            header = string.Format("{0},{1}", header, column.Header.ToString());
                        }
                        if(!string.IsNullOrEmpty(header)) header = header.Substring(1);
                        file.WriteLine(header);
                        for (int i = 0; i < _DataGrid.Items.Count; i++)
                        {
                            var data = "";
                            foreach (var column in _DataGrid.Columns)
                            {
                                data = string.Format("{0},{1}", data, (column.GetCellContent(_DataGrid.Items[i]) as TextBlock).Text);
                            }
                            if (!string.IsNullOrEmpty(data)) data = data.Substring(1);
                            file.WriteLine(data);
                        }
                        file.Close();
                        MessageBox.Show("File exported successfully", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error while generate report", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
        }
    }
}
