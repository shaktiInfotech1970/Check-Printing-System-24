using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CPS.Common
{
    public class DataGridToPDF
    {
        private DataGrid _DataGrid;
        public DataGridToPDF(DataGrid oDataGrid)
        {
            _DataGrid = oDataGrid;
        }

        public PdfPTable GetPDFTable(float[] columnWidth)
        {
            BaseFont fontVerdana = BaseFont.CreateFont("fonts/VERDANA.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            Font fontBold = new Font(fontVerdana, 9, Font.BOLD);
            Font fontNormal = new Font(fontVerdana, 8, Font.NORMAL);

            PdfPTable table = new PdfPTable(_DataGrid.Columns.Count);
            table.SetWidthPercentage(columnWidth, iTextSharp.text.PageSize.A4.Rotate());

            table.DefaultCell.Border = Rectangle.NO_BORDER; //To hide borders

            foreach (var column in _DataGrid.Columns)
            {
                var cell = new PdfPCell(new Phrase(column.Header.ToString(), fontBold));
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.BorderWidthBottom = 0.1f;
                cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cell);
            }
            table.HeaderRows = 1;
            for (int i = 0; i < _DataGrid.Items.Count; i++)
            {
                for (int j = 0; j < table.NumberOfColumns; j++)
                {

                    var cell = new PdfPCell(new Phrase(((System.Windows.Controls.TextBlock)(GetCell(i, j).Content)).Text, fontNormal));
                    cell.BorderWidthTop = 0f;
                    cell.BorderWidthLeft = 0f;
                    cell.BorderWidthRight = 0f;
                    cell.BorderWidthBottom = 0.1f;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell.FixedHeight = 25f;
                    table.AddCell(cell);
                }
            }
            return table;
        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);

            if (rowContainer != null)
            {
                _DataGrid.ScrollIntoView(rowContainer, _DataGrid.Columns[column]);
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)_DataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                _DataGrid.UpdateLayout();
                _DataGrid.ScrollIntoView(_DataGrid.Items[index]);
                row = (DataGridRow)_DataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
