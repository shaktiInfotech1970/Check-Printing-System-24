using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zen.Barcode;

namespace CPS.Business
{
    public class BatchInPrinting
    {
        public int PageNo { get; set; }
        public PrintBatch PrintBatch { get; set; }
        public RequestLayout RequestLayout { get; set; }
        public ChequeLayout ChequeLayout { get; set; }
    }

    public class PrinterJob
    {
        public PrinterJob()
        {
            PrintBatches = new List<PrintBatch>();
        }

        public List<PrintBatch> PrintBatches { get; set; }
    }

    public class PrintBatch
    {
        public PrintBatch()
        {
            ChequePages = new List<PrintPage>();
        }

        public PrintPage RequestPage { get; set; }
        public IEnumerable<PrintPage> ChequePages { get; set; }

        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }

    public class PrintPage
    {
        public PrintPage()
        {
            Sections = new List<PrintSection>();
        }

        public int PageNo { get; set; }
        public List<PrintSection> Sections { get; set; }
    }

    public class PrintSection
    {
        public int SequenceNo { get; set; }
        public PrintRequest PrintRequest { get; set; }
    }

    public class PrintJob
    {
        #region Database Update

        private static bool MarkRequestAsPrint(RequestDTO request)
        {
            var oLoggedInUser = (UserMasterDTO)App.Current.Windows[0].Tag;
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<RequestDTO>(context);
                var existing = repository.FindBy(f => f.Id == request.Id && f.IsPrinted == false).FirstOrDefault();
                if (existing != null)
                {
                    existing.IsPrinted = true;
                    existing.PrintJobNo = request.PrintJobNo;
                    existing.UpdatedBy = oLoggedInUser.UserId;
                    existing.UpdatedOn = DateTime.Now;

                    var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (repository.SaveOrUpdate(existing, errors))
                    {
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool AddPrintHistory(RequestDTO request, PrintType PrintType)
        {
            var printHistory = new PrintHistoryDTO
            {
                RequestId = request.Id,
                PrintType = PrintType,
                ChequeNoFrom = request.ChequeFrom,
                ChequeNoTo = request.ChequeTo
            };

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PrintHistoryDTO>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(printHistory, errors))
                    context.SaveChanges();
                else
                    return false;
            }

            return false;
        }

        #endregion

        #region Request Helper

        private static IEnumerable<RequestGroup> GroupedRequest(IEnumerable<PrintRequest> requests)
        {
            return requests.GroupBy(g => g.Request.NoOfCheque, (key, element) => new RequestGroup { BookSize = key, Requests = element });
        }

        private static RequestGroup SplitRequestGroup(RequestGroup requestGroup)
        {
            var requests = new List<PrintRequest>();

            foreach (var request in requestGroup.Requests)
            {
                if (request.Request.NoOfChequeBook == 1)
                    requests.Add(request);
                else
                {
                    for (int i = 0; i < request.Request.NoOfChequeBook; i++)
                    {
                        var r = request.DeepCopy();
                        r.Request.ChequeFrom--;
                        r.Request.ChequeFrom = r.Request.ChequeFrom + (r.Request.NoOfCheque * i) + 1;
                        r.Request.ChequeTo = r.Request.ChequeFrom + r.Request.NoOfCheque - 1;

                        requests.Add(r);
                    }
                }
            }

            requestGroup.Requests = requests;
            return requestGroup;
        }

        private static PrinterJob GetPrinterJob(IEnumerable<RequestGroup> requestGroups, int startPage, int endPage)
        {
            var printJob = new PrinterJob();

            var pageNo = 0;
            foreach (var requestGroup in requestGroups)
            {
                var batches = GetPrintBatches(requestGroup, ref pageNo);
                // Filter batches based on page range print
                batches = batches.Where(w => w.StartPage <= endPage && startPage <= w.EndPage);
                //Reset page start and End for batch
                var resetBatch = batches.ToList();
                resetBatch.ForEach(each =>
                {
                    each.RequestPage.Sections.ForEach(s =>
                    {
                        s.PrintRequest.ChequeNoFrom = s.PrintRequest.Request.ChequeFrom;
                        s.PrintRequest.ChequeNoTo = s.PrintRequest.Request.ChequeTo;
                    });

                    var startOffSet = startPage - each.StartPage;
                    var endOffSet = each.EndPage - endPage + (each.StartPage - 1);

                    if (each.StartPage < startPage)
                    {
                        each.StartPage = startPage;
                        each.ChequePages = each.ChequePages.Where(w => w.PageNo >= startPage);
                        each.RequestPage.Sections.ForEach(s => { s.PrintRequest.ChequeNoFrom = s.PrintRequest.Request.ChequeFrom + startOffSet; });
                    }
                    if (each.EndPage > endPage)
                    {
                        each.EndPage = endPage;
                        each.ChequePages = each.ChequePages.Where(w => w.PageNo <= endPage);
                        each.RequestPage.Sections.ForEach(s => { s.PrintRequest.ChequeNoTo = s.PrintRequest.Request.ChequeTo - endOffSet; });
                    }
                });

                printJob.PrintBatches.AddRange(resetBatch);
            }

            return printJob;
        }

        private static IEnumerable<PrintBatch> GetPrintBatches(RequestGroup requestGroup, ref int pageNo)
        {
            var batches = new List<PrintBatch>();

            for (int i = 0; i < requestGroup.Requests.Count(); i += 3)
            {
                var requests = requestGroup.Requests.Skip(i).Take(3);
                if (requests.Count() < 3)
                {
                    foreach (var r in requests)
                    {
                        var batch = new PrintBatch { StartPage = pageNo + 1 };
                        batch.RequestPage = GetRequestPage(r);
                        batch.ChequePages = GetChequePages(r, requestGroup.BookSize, ref pageNo);
                        batch.EndPage = pageNo;

                        batches.Add(batch);
                    }
                }
                else
                {
                    var batch = new PrintBatch { StartPage = pageNo + 1 };
                    batch.RequestPage = GetRequestPage(requests);
                    batch.ChequePages = GetChequePages(requests, requestGroup.BookSize, ref pageNo);
                    batch.EndPage = pageNo;

                    batches.Add(batch);
                }
            }

            return batches;
        }

        private static PrintPage GetRequestPage(PrintRequest printRequest)
        {
            var printPage = new PrintPage { PageNo = 1 };
            printPage.Sections.Add(new PrintSection { PrintRequest = printRequest });

            return printPage;
        }

        private static PrintPage GetRequestPage(IEnumerable<PrintRequest> printRequests)
        {
            var printPage = new PrintPage { PageNo = 1 };
            printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Take(1).FirstOrDefault() });
            printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Skip(1).Take(1).FirstOrDefault() });
            printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Skip(2).Take(1).FirstOrDefault() });

            return printPage;
        }

        private static IEnumerable<PrintPage> GetChequePages(PrintRequest printRequest, int bookSize, ref int pageNo)
        {
            var printPages = new List<PrintPage>();
            for (int i = 1; i <= (bookSize / 3); i++)
            {
                var diviser = bookSize / 3;
                pageNo++;
                var sequenceNo = i;
                var printPage = new PrintPage { PageNo = pageNo };
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequest, SequenceNo = sequenceNo });
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequest, SequenceNo = sequenceNo + diviser });
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequest, SequenceNo = sequenceNo + (diviser + diviser) });
                printPages.Add(printPage);
            }

            return printPages;
        }

        private static IEnumerable<PrintPage> GetChequePages(IEnumerable<PrintRequest> printRequests, int bookSize, ref int pageNo)
        {
            var printPages = new List<PrintPage>();
            for (int i = 1; i <= bookSize; i++)
            {
                pageNo++;
                var printPage = new PrintPage { PageNo = pageNo };
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Take(1).FirstOrDefault(), SequenceNo = i });
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Skip(1).Take(1).FirstOrDefault(), SequenceNo = i });
                printPage.Sections.Add(new PrintSection { PrintRequest = printRequests.Skip(2).Take(1).FirstOrDefault(), SequenceNo = i });
                printPages.Add(printPage);
            }

            return printPages;
        }

        #endregion

        #region Public Methods

        public static bool Print(IEnumerable<PrintRequest> requests)
        {
            var printJobNo = Counter.NextValue(Counters.PrintJob);
            var assingPrintJobRequests = requests.ToList();
            assingPrintJobRequests.ForEach(each => each.Request.PrintJobNo = printJobNo);

            var printerPreference = GetPrinter();
            var requestGroups = GroupedRequest(requests).Select(s => SplitRequestGroup(s)).ToList();
            var printerJob = GetPrinterJob(requestGroups, 1, int.MaxValue);
            return SendToPrinter(printerPreference, printerJob, PrintType.ChequeBook);
        }

        public static bool RePrint(IEnumerable<PrintRequest> requests)
        {
            var printerPreference = GetPrinter();
            var requestGroups = GroupedRequest(requests).Select(s => SplitRequestGroup(s)).ToList();
            var printerJob = GetPrinterJob(requestGroups, 1, int.MaxValue);
            return SendToPrinter(printerPreference, printerJob, PrintType.RePrintChequeBook);
        }

        public static bool RePrintSinglePage(IEnumerable<PrintRequest> requests, int from, int to)
        {
            var printerPreference = GetPrinter();
            var requestGroups = GroupedRequest(requests).Select(s => SplitRequestGroup(s)).ToList();
            var printerJob = GetPrinterJob(requestGroups, from, to);
            return SendToPrinter(printerPreference, printerJob, PrintType.RePrintCheque);
        }

        public static bool RePrintRequest(IEnumerable<PrintRequest> requests)
        {
            var printerPreference = GetPrinter();
            var requestGroups = GroupedRequest(requests).Select(s => SplitRequestGroup(s)).ToList();
            var printerJob = GetPrinterJob(requestGroups, 1, int.MaxValue);
            return SendToPrinter(printerPreference, printerJob, PrintType.RePrintRequest);
        }

        #endregion

        #region Value Helper

        private static string GetBranchAddress(BranchMasterDTO branch)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(branch.Name))
                sb.Append(branch.Name);

            if (!string.IsNullOrWhiteSpace(branch.AddressLine1))
                sb.Append(": " + branch.AddressLine1);

            if (!string.IsNullOrWhiteSpace(branch.AddressLine2))
                sb.Append(", " + branch.AddressLine2);

            if (!string.IsNullOrWhiteSpace(branch.AddressLine3))
                sb.Append(", " + branch.AddressLine3);

            if (!string.IsNullOrWhiteSpace(branch.City))
                sb.Append(string.Format(", {0} - {1}", branch.City, branch.PostalCode));

            return sb.ToString();
        }

        private static float GetDPI(float cm)
        {
            var unit = (72 / 2.54f);
            return cm * unit;
        }

        private static void PrintNameAddress(PrintPageEventArgs e, RequestDTO request, float x, float y)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            Font font = new Font("Verdana", 9);
            Font fontBold = new Font("Verdana", 9, FontStyle.Bold);

            if (!string.IsNullOrWhiteSpace(request.Name))
                e.Graphics.DrawString(request.Name, fontBold, brush, x, y);

            y += 20;

            if (!string.IsNullOrWhiteSpace(request.Address1))
                e.Graphics.DrawString(request.Address1, font, brush, x, y);

            y += 13;
            if (!string.IsNullOrWhiteSpace(request.Address2))
                e.Graphics.DrawString(request.Address2, font, brush, x, y);

            y += 13;
            if (!string.IsNullOrWhiteSpace(request.Address3))
                e.Graphics.DrawString(request.Address3, font, brush, x, y);

            y += 13;
            if (!string.IsNullOrWhiteSpace(request.City))
                e.Graphics.DrawString(string.Format("{0}, {1}", request.City, request.PostalCode), font, brush, x, y);
        }

        private static string GetStamp(RequestDTO request)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(request.Name))
                sb.Append(request.Name);

            if (!string.IsNullOrWhiteSpace(request.JointName1))
                sb.Append(Environment.NewLine + request.JointName1);

            if (!string.IsNullOrWhiteSpace(request.JointName2))
                sb.Append(Environment.NewLine + request.JointName2);

            if (!string.IsNullOrWhiteSpace(request.Signatory1) || !string.IsNullOrWhiteSpace(request.Signatory2) || !string.IsNullOrWhiteSpace(request.Signatory3))
            {
                sb.Append(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            }
            if (!string.IsNullOrWhiteSpace(request.Signatory1))
            {
                sb.Append(request.Signatory1);
            }
            if (!string.IsNullOrWhiteSpace(request.Signatory2))
            {
                sb.Append(Environment.NewLine + request.Signatory2);
            }
            if (!string.IsNullOrWhiteSpace(request.Signatory3))
            {
                sb.Append(Environment.NewLine + request.Signatory3);
            }

            return sb.ToString();
        }

        private static string GetMICR(RequestDTO request, int currentChequeNo)
        {
            return string.Format("C{0}C {1}A {2}C {3}", (request.ChequeFrom + (currentChequeNo - 1)).ToString("000000"), request.MICRCode, Convert.ToInt32(request.AccountNo.Substring(Math.Max(0, request.AccountNo.Length - 6))).ToString("000000"), request.TransactionCode);
        }

        private static string GetAuditText(RequestDTO request)
        {
            var oLoggedInUser = (UserMasterDTO)App.Current.Windows[0].Tag;
            return string.Format("{0, -10} {1, -5} {2, -5} {3, -20}", oLoggedInUser.UserId, request.PrintJobNo, 1, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }

        private static string GetAuditText(RequestDTO request, int pageNo)
        {
            var oLoggedInUser = (UserMasterDTO)App.Current.Windows[0].Tag;
            return string.Format("{0, -10} {1, -5} {2, -5} {3, -20}", oLoggedInUser.UserId, request.PrintJobNo, pageNo, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }

        private static Image GetBarcode(string number, int barHeight, System.Drawing.RotateFlipType rotateFlipType)
        {
            var bdf = BarcodeDrawFactory.Code128WithChecksum;
            var img = bdf.Draw(number, barHeight, 1);
            img.RotateFlip(rotateFlipType);

            return img;
        }

        private static Image GetAccountPayeeImage()
        {
            System.Windows.Media.ImageSourceConverter c = new System.Windows.Media.ImageSourceConverter();
            return (Image)CPS.Properties.Resources.ACPayee;
        }

        #endregion

        #region Print

        private static BatchInPrinting BatchInPrinting = null;

        private static PrinterPreference GetPrinter()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PrinterPreference>(context);
                return repository.GetAll().FirstOrDefault();
            }
        }

        private static bool SendToPrinter(PrinterPreference printerPreference, PrinterJob printerJob, PrintType printType)
        {
            if (printerPreference != null)
            {
                var requestLayout = new RequestLayout();
                var chequeLayout = new ChequeLayout();

                using (var context = new CPSDbContext())
                {
                    requestLayout = context.RequestLayout.FirstOrDefault();
                    chequeLayout = context.ChequeLayout.FirstOrDefault();
                }

                foreach (var batch in printerJob.PrintBatches)
                {
                    BatchInPrinting = new BatchInPrinting { PageNo = 1, PrintBatch = batch, RequestLayout = requestLayout, ChequeLayout = chequeLayout };

                    if ((printType == PrintType.ChequeBook && printerPreference.RequestTray != printerPreference.ChequeTray) || printType == PrintType.RePrintRequest)
                    {
                        using (var requestPrintDocument = GetPrintDocument(printerPreference.Name, printerPreference.RequestTray, new PrintPageEventHandler(requestPrintDocument_PrintPage)))
                        {
                            requestPrintDocument.Print();
                        }
                    }
                    if (printType != PrintType.Request && printType != PrintType.RePrintRequest)
                    {
                        using (var chequePrintDocument = GetPrintDocument(printerPreference.Name, printerPreference.ChequeTray, new PrintPageEventHandler(chequePrintDocument_PrintPage)))
                        {
                            chequePrintDocument.Print();
                        }
                    }

                    foreach (var section in BatchInPrinting.PrintBatch.RequestPage.Sections)
                    {
                        if (printType == PrintType.ChequeBook)
                        {
                            MarkRequestAsPrint(section.PrintRequest.Request);
                            AddPrintHistory(section.PrintRequest.Request, PrintType.ChequeBook);
                            if (printerPreference.RequestTray != printerPreference.ChequeTray)
                                AddPrintHistory(section.PrintRequest.Request, PrintType.Request);
                        }
                        else if (printType == PrintType.RePrintCheque)
                        {
                            section.PrintRequest.Request.ChequeFrom = section.PrintRequest.ChequeNoFrom;
                            section.PrintRequest.Request.ChequeTo = section.PrintRequest.ChequeNoTo;
                            AddPrintHistory(section.PrintRequest.Request, printType);
                        }
                        else
                        {
                            AddPrintHistory(section.PrintRequest.Request, printType);
                        }
                    }
                }

                return true;
            }
            else
            {
                System.Windows.MessageBox.Show("Printer is not configure.", "Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        private static PrintDocument GetPrintDocument(string printerName, int paperSource, PrintPageEventHandler printPage)
        {
            var printerSettings = new PrinterSettings();

            PrintDocument printDoc = new PrintDocument();
            PaperSize oPS = new PaperSize();
            oPS.RawKind = (int)PaperKind.Letter;

            printDoc.PrinterSettings = new PrinterSettings();
            printDoc.PrinterSettings.PrinterName = printerName;
            printDoc.DefaultPageSettings.PaperSize = oPS;
            if (printerSettings.PaperSources != null && printerSettings.PaperSources.Count > 0)
            {
                printDoc.DefaultPageSettings.PaperSource = printerSettings.PaperSources[0];
            }
            printDoc.DefaultPageSettings.PaperSource.RawKind = paperSource;
            printDoc.PrintPage += printPage;

            return printDoc;
        }

        private const float PageWidth = 21.59f;
        private const float PageHeight = 25.7175f;

        private const float SectionWidth = 20.32f;
        private const float SectionHeight = 12.965f;

        private static void requestPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            for (int i = 1; i <= BatchInPrinting.PrintBatch.RequestPage.Sections.Count; i++)
                PrintRequestSection(e, BatchInPrinting.PrintBatch.RequestPage, i);
        }

        private static void PrintRequestSection(PrintPageEventArgs e, PrintPage printPage, int sectionNo)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            Font font9 = new Font("Verdana", 9);
            Font font9Bold = new Font("Verdana", 9, FontStyle.Bold);
            Font font5 = new Font("Verdana", 5);

            var section = printPage.Sections.ToList()[sectionNo - 1];
            var x = (GetDPI(PageWidth) - GetDPI(SectionWidth));
            var y = (GetDPI(SectionHeight) * (sectionNo - 1));

            if (BatchInPrinting.RequestLayout.branchAddress1Visble)
                e.Graphics.DrawString(GetBranchAddress(section.PrintRequest.Branch), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.branchAddress1X), y + GetDPI(BatchInPrinting.RequestLayout.branchAddress1Y));

            if (BatchInPrinting.RequestLayout.branchAddress2Visble)
                e.Graphics.DrawString(GetBranchAddress(section.PrintRequest.Branch), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.branchAddress2X), y + GetDPI(BatchInPrinting.RequestLayout.branchAddress2Y));

            if (BatchInPrinting.RequestLayout.chequeFrom1Visble)
                e.Graphics.DrawString(section.PrintRequest.Request.ChequeFrom.ToString("00000"), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.chequeFrom1X), y + GetDPI(BatchInPrinting.RequestLayout.chequeFrom1Y));

            if (BatchInPrinting.RequestLayout.chequeTo1Visble)
                e.Graphics.DrawString(section.PrintRequest.Request.ChequeTo.ToString("00000"), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.chequeTo1X), y + GetDPI(BatchInPrinting.RequestLayout.chequeTo1Y));

            if (BatchInPrinting.RequestLayout.chequeFrom2Visble)
                e.Graphics.DrawString(section.PrintRequest.Request.ChequeFrom.ToString("00000"), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.chequeFrom2X), y + GetDPI(BatchInPrinting.RequestLayout.chequeFrom2Y));

            if (BatchInPrinting.RequestLayout.chequeTo2Visble)
                e.Graphics.DrawString(section.PrintRequest.Request.ChequeTo.ToString("00000"), font9, brush, x + GetDPI(BatchInPrinting.RequestLayout.chequeTo2X), y + GetDPI(BatchInPrinting.RequestLayout.chequeTo2Y));

            if (BatchInPrinting.RequestLayout.nameAddress1Visble)
                PrintNameAddress(e, section.PrintRequest.Request, x + GetDPI(BatchInPrinting.RequestLayout.nameAddress1X), y + GetDPI(BatchInPrinting.RequestLayout.nameAddress1Y));

            if (BatchInPrinting.RequestLayout.nameAddress2Visble)
                PrintNameAddress(e, section.PrintRequest.Request, x + GetDPI(BatchInPrinting.RequestLayout.nameAddress2X), y + GetDPI(BatchInPrinting.RequestLayout.nameAddress2Y));

            if (BatchInPrinting.RequestLayout.accountNo1Visble)
                e.Graphics.DrawString(string.Format("{0}: {1}", section.PrintRequest.AccountType.Name, section.PrintRequest.Request.AccountNoFull), font9Bold, brush, x + GetDPI(BatchInPrinting.RequestLayout.accountNo1X), y + GetDPI(BatchInPrinting.RequestLayout.accountNo1Y));

            if (BatchInPrinting.RequestLayout.accountNo2Visble)
                e.Graphics.DrawString(string.Format("{0}: {1}", section.PrintRequest.AccountType.Name, section.PrintRequest.Request.AccountNoFull), font9Bold, brush, x + GetDPI(BatchInPrinting.RequestLayout.accountNo2X), y + GetDPI(BatchInPrinting.RequestLayout.accountNo2Y));

            var barcode = GetBarcode(section.PrintRequest.Request.AccountNoFull, 15, System.Drawing.RotateFlipType.RotateNoneFlipNone);
            if (BatchInPrinting.RequestLayout.barcode1Visble)
                e.Graphics.DrawImage(barcode, x + GetDPI(BatchInPrinting.RequestLayout.barcode1X), y + GetDPI(BatchInPrinting.RequestLayout.barcode1Y));

            if (BatchInPrinting.RequestLayout.barcode2Visble)
                e.Graphics.DrawImage(barcode, x + GetDPI(BatchInPrinting.RequestLayout.barcode2X), y + GetDPI(BatchInPrinting.RequestLayout.barcode2Y));

            var auditText = GetAuditText(section.PrintRequest.Request);
            var auditTextSize = e.Graphics.MeasureString(auditText, font5);
            e.Graphics.RotateTransform(-90);
            if (BatchInPrinting.RequestLayout.audiText1Visble)
                e.Graphics.DrawString(auditText, font5, brush, new PointF((-y) + (-auditTextSize.Width) + (-GetDPI(BatchInPrinting.RequestLayout.audiText1Y)), x + GetDPI(BatchInPrinting.RequestLayout.audiText1X)));
            if (BatchInPrinting.RequestLayout.audiText2Visble)
                e.Graphics.DrawString(auditText, font5, brush, new PointF((-y) + (-auditTextSize.Width) + (-GetDPI(BatchInPrinting.RequestLayout.audiText2Y)), x + GetDPI(BatchInPrinting.RequestLayout.audiText2X)));

            e.Graphics.ResetTransform();
        }

        private static void chequePrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var printPage = BatchInPrinting.PrintBatch.ChequePages.Skip(BatchInPrinting.PageNo - 1).Take(1).FirstOrDefault();
            if (printPage != null)
            {
                BatchInPrinting.PageNo++;

                for (int i = 1; i <= printPage.Sections.Count; i++)
                    PrintChequeSection(e, printPage, i);

                var nextPage = BatchInPrinting.PrintBatch.ChequePages.Skip(BatchInPrinting.PageNo - 1).Take(1).FirstOrDefault();
                if (nextPage != null) e.HasMorePages = true;
            }
        }

        private static void PrintChequeSection(PrintPageEventArgs e, PrintPage printPage, int sectionNo)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            Font font9 = new Font("Verdana", 9);
            Font font7 = new Font("Verdana", 7);
            Font font8Bold = new Font("Verdana", 8, FontStyle.Bold);
            Font font9Bold = new Font("Verdana", 9, FontStyle.Bold);
            Font font5 = new Font("Verdana", 5);
            Font micrFont = new Font("MICR Encoding", 19f, FontStyle.Bold);

            var section = printPage.Sections.ToList()[sectionNo - 1];
            var x = (GetDPI(PageWidth) - GetDPI(SectionWidth));
            var y = (GetDPI(SectionHeight) * (sectionNo - 1));

            if (BatchInPrinting.ChequeLayout.branchAddressVisble)
                e.Graphics.DrawString(GetBranchAddress(section.PrintRequest.Branch), font7, brush, x + GetDPI(BatchInPrinting.ChequeLayout.branchAddressX), y + GetDPI(BatchInPrinting.ChequeLayout.branchAddressY));

            if (BatchInPrinting.ChequeLayout.ifscVisble)
                e.Graphics.DrawString(string.Format("IFSC: {0}", section.PrintRequest.Branch.IFSC), font8Bold, brush, x + GetDPI(BatchInPrinting.ChequeLayout.ifscX), y + GetDPI(BatchInPrinting.ChequeLayout.ifscY));

            if (BatchInPrinting.ChequeLayout.orderOrBarerVisble)
                e.Graphics.DrawString(string.Format("Or {0}", section.PrintRequest.Request.BearerOrder), font9, brush, x + GetDPI(BatchInPrinting.ChequeLayout.orderOrBarerX), y + GetDPI(BatchInPrinting.ChequeLayout.orderOrBarerY));

            if (BatchInPrinting.ChequeLayout.accountNoVisble)
                e.Graphics.DrawString(string.Format("{0}: {1}", section.PrintRequest.AccountType.Name, section.PrintRequest.Request.AccountNoFull), font9Bold, brush, x + GetDPI(BatchInPrinting.ChequeLayout.accountNoX), y + GetDPI(BatchInPrinting.ChequeLayout.accountNoY));

            if (BatchInPrinting.ChequeLayout.stampVisble)
            {
                RectangleF stampRect = new RectangleF(x + GetDPI(BatchInPrinting.ChequeLayout.stampX), y + GetDPI(BatchInPrinting.ChequeLayout.stampY), 450, 110);
                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;

                // Draw the text and the surrounding rectangle.
                e.Graphics.DrawString(GetStamp(section.PrintRequest.Request), font8Bold, brush, stampRect, stringFormat);
            }

            if (BatchInPrinting.ChequeLayout.micrVisble)
            {
                //e.Graphics.DrawString(GetMICR(section.PrintRequest.Request, section.SequenceNo), micrFont, brush, x + GetDPI(BatchInPrinting.ChequeLayout.micrX), y + GetDPI(BatchInPrinting.ChequeLayout.micrY));
                PointF pointf = new PointF(x + GetDPI(BatchInPrinting.ChequeLayout.micrX), y + GetDPI(BatchInPrinting.ChequeLayout.micrY));
                var micrText = GetMICR(section.PrintRequest.Request, section.SequenceNo);
                var micrX = x + GetDPI(BatchInPrinting.ChequeLayout.micrX);
                var micrY = y + GetDPI(BatchInPrinting.ChequeLayout.micrY);
                foreach (var c in micrText.ToArray())
                {
                    e.Graphics.DrawString(c.ToString(), micrFont, brush, micrX, micrY);
                    micrX += 12.3f;
                }
            }

            if (section.PrintRequest.Request.BearerOrder.ToLower() == "order" && BatchInPrinting.ChequeLayout.accountPayeeVisble)
            {
                var imgAccountPayee = GetAccountPayeeImage();
                e.Graphics.DrawImage(imgAccountPayee, x + GetDPI(BatchInPrinting.ChequeLayout.accountPayeeX), y + GetDPI(BatchInPrinting.ChequeLayout.accountPayeeY));
            }

            var imgBarcode = GetBarcode(string.Format("{0}{1}{2}{3}", (section.PrintRequest.Request.ChequeFrom + (section.SequenceNo - 1)).ToString("000000"), section.PrintRequest.Request.MICRCode, Convert.ToInt32(section.PrintRequest.Request.AccountNo.Substring(Math.Max(0, section.PrintRequest.Request.AccountNo.Length - 6))).ToString("000000"), section.PrintRequest.Request.TransactionCode), 6, System.Drawing.RotateFlipType.Rotate270FlipNone);

            if (BatchInPrinting.ChequeLayout.barcodeVisble)
                e.Graphics.DrawImage(imgBarcode, x + GetDPI(BatchInPrinting.ChequeLayout.barcodeX), y + GetDPI(BatchInPrinting.ChequeLayout.barcodeY));

            if (BatchInPrinting.ChequeLayout.audiTextVisble)
            {
                var auditText = GetAuditText(section.PrintRequest.Request, printPage.PageNo);
                var auditTextSize = e.Graphics.MeasureString(auditText, font5);
                e.Graphics.RotateTransform(-90);
                e.Graphics.DrawString(auditText, font5, brush, new PointF((-y) + (-auditTextSize.Width) + GetDPI(-BatchInPrinting.ChequeLayout.audiTextY), x + GetDPI(BatchInPrinting.ChequeLayout.audiTextX)));
                e.Graphics.ResetTransform();
            }
        }

        #endregion

    }
}
