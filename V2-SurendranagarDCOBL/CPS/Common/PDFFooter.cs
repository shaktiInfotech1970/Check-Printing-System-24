using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS.Common
{
    public class PDFFooter : PdfPageEventHelper
    {
        int pageNo = 0;
        public float[] columnWidth ;
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }
        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }
        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {


            base.OnEndPage(writer, document);
            Paragraph footer = new Paragraph(string.Format("Page No:- {0}", ++pageNo), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
            footer.Alignment = Element.ALIGN_RIGHT;

            PdfPTable footerTbl = new PdfPTable(columnWidth);
            footerTbl.SetWidthPercentage(columnWidth, iTextSharp.text.PageSize.A4.Rotate());
            footerTbl.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.Colspan=(columnWidth.Count()-1);                        
            footerTbl.AddCell(cell);
            footerTbl.AddCell(cell);
            footerTbl.WriteSelectedRows(0, -1, 0, 15, writer.DirectContent);            
        }
        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}
