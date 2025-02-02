using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;


namespace InvoiceGenerator.Controllers
{
    [Route("invoices")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceServices _InvoiceServices;
        private readonly IServiceRenderRepository _ServiceRenderRepository;
        private readonly INotyfService _notyfService;

        public InvoiceController(
            IInvoiceServices InvoiceServices,
            INotyfService notyfService,
            IServiceRenderRepository ServiceRenderRepository
           )
        {
            _InvoiceServices = InvoiceServices;
            _notyfService = notyfService;
            _ServiceRenderRepository = ServiceRenderRepository;
            
        }
        [HttpGet("all-invoices")]
        public async Task<IActionResult> Invoices()
        {
            var result = await _InvoiceServices.GetAllInvoice();
            return View(result.Data);
        }
        [HttpGet("download-invoice")]
        public async Task<IActionResult> DownloadInvoicePdf(Guid Id)
        {
            var invoiceData = await _InvoiceServices.GetInvoiceById(Id);

            if (invoiceData == null || invoiceData.Data == null)
            {
                return NotFound("Invoice not found.");
            }

            using (var ms = new MemoryStream())
            {
                // Further adjusted top margin to move content slightly higher
                Document document = new Document(PageSize.A4, 50, 50, 150, 25); // Reduced top margin to 150
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // ✅ Add full-page watermark
                string watermarkPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "watermark.jpg");
                if (System.IO.File.Exists(watermarkPath))
                {
                    Image watermark = Image.GetInstance(watermarkPath);
                    watermark.SetAbsolutePosition(0, 0); // Keep the watermark in place
                    watermark.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    watermark.Alignment = Image.UNDERLYING;
                    PdfContentByte canvas = writer.DirectContentUnder;
                    canvas.AddImage(watermark);
                }

                // ✅ Move text slightly higher, further reduced spacing before content
                document.Add(new Paragraph(" ") { SpacingBefore = 150 }); // Adjusted value to 150 to move it up

                // 📌 Invoice Content
                document.Add(new Paragraph("The Management,", new Font(Font.FontFamily.HELVETICA, 12)));
                document.Add(new Paragraph($"{invoiceData.Data.BuyerName},", new Font(Font.FontFamily.HELVETICA, 12)));
                document.Add(new Paragraph(invoiceData.Data.Address, FontFactory.GetFont("Arial", 10)));
                document.Add(Chunk.NEWLINE);
                document.Add(new Paragraph("PEST CONTROL INVOICE:", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
                document.Add(Chunk.NEWLINE);
                // 📌 Invoice Details Table
                PdfPTable invoiceDetailsTable = new PdfPTable(3) { WidthPercentage = 100 };
                AddTableHeader(invoiceDetailsTable, "Invoice Date", "Service Date", "Warranty");
                invoiceDetailsTable.AddCell(DateTime.Now.ToString("dd/MM/yyyy"));
                invoiceDetailsTable.AddCell($"Start: {invoiceData.Data.ServiceStartDate:dd/MM/yyyy} Finish: {invoiceData.Data.ServiceEndDate:dd/MM/yyyy}");
                invoiceDetailsTable.AddCell("90 days");
                document.Add(invoiceDetailsTable);
                document.Add(Chunk.NEWLINE);
                // 📌 Service Description & Area of Coverage
                PdfPTable mainTable = new PdfPTable(2) { WidthPercentage = 100 };
                mainTable.SetWidths(new float[] { 50f, 50f });
                PdfPTable serviceTable = new PdfPTable(1) { WidthPercentage = 100 };
                serviceTable.AddCell(new PdfPCell(new Phrase("Service Description(s)", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                int serviceIndex = 1;
                foreach (var service in invoiceData.Data.ServiceRenderred)
                {
                    serviceTable.AddCell(new PdfPCell(new Phrase($"{serviceIndex++}. {service}", FontFactory.GetFont("Arial", 10)))
                    {
                        Border = Rectangle.NO_BORDER
                    });
                }

                PdfPTable areaTable = new PdfPTable(1) { WidthPercentage = 100 };
                areaTable.AddCell(new PdfPCell(new Phrase("Area of Coverage", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                int areaIndex = 1;
                foreach (var area in invoiceData.Data.AreaOfCoverage)
                {
                    areaTable.AddCell(new PdfPCell(new Phrase($"{areaIndex++}. {area}", FontFactory.GetFont("Arial", 10)))
                    {
                        Border = Rectangle.NO_BORDER
                    });
                }
                mainTable.AddCell(new PdfPCell(serviceTable));
                mainTable.AddCell(new PdfPCell(areaTable));
                document.Add(mainTable);
                document.Add(Chunk.NEWLINE);

                // 📌 Total Cost
                document.Add(new Paragraph($"Total Cost: ₦{invoiceData.Data.TotalCost:N}", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                document.Add(Chunk.NEWLINE);

                // 📌 Footer (Account & Officer Info)
                AddFooter(document);

                document.Close();

                string fileName = $"{invoiceData.Data.BuyerName} Invoice.pdf";
                return File(ms.ToArray(), "application/pdf", fileName);
            }
        }
        // 🔻 Helper Methods (No Logo Anymore)
        private void AddFooter(Document document)
        {
            document.Add(new Paragraph("ACCOUNT DETAILS:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            document.Add(new Paragraph("Account Number: 8128509998"));
            document.Add(new Paragraph("Account Name: Fooju's Fumigants Enterprises"));
            document.Add(new Paragraph("Bank Name: MoniePoint"));
            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Pest Control Officer"));
            document.Add(new Paragraph("Ojuola Fausat Oluwatosin"));
            document.Add(new Paragraph("Annual License"));
            document.Add(new Paragraph("Reg. No.: EHO/5541/2022/2023/C/0212"));
        }
        private void AddTableHeader(PdfPTable table, params string[] headers)
        {
            foreach (var header in headers)
            {
                table.AddCell(new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 10, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY
                });
            }
        }

        [HttpGet("create-invoice")]
        public async Task<IActionResult> CreateInvoice()
        {
            var selectService = _InvoiceServices.GetServiceSelect();
            if (selectService == null)
            {
                selectService = new List<SelectServiceDto>();
            }
            ViewData["SelectService"] = selectService;

            var selectArea = _InvoiceServices.GetAreaSelect();
            if (selectArea == null)
            {
                selectArea = new List<SelectAreaDto>();
            }
            ViewData["SelectArea"] = selectArea;
            return View();
        }
        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoiceAsync(CreateInvoiceDto request)
        {
           
            var result = await _InvoiceServices.CreateInvoice(request);
            if (result.IsSuccessful)
            {
                return RedirectToAction("Invoices");
            }
            return RedirectToAction("Invoices");
        }
        [HttpGet("delete-invoice/{Id}")]
        public async Task<IActionResult> DeleteInvoiceAsync([FromRoute] Guid Id)
        {  
            var result = await _InvoiceServices.Delete(Id);
            if (result.IsSuccessful)
            {
                return RedirectToAction("Invoices");
            }
            return RedirectToAction("Invoices");
        }
        [HttpGet("Invoice/{Id}")]
        public async Task<IActionResult> InvoiceDetail([FromRoute] Guid Id)
        {
            var result = await _InvoiceServices.GetInvoice(Id);
            return View(result.Data);
        }
        [HttpGet("update-invoice/{Id}")]
        public async Task<IActionResult> UpdateInvoice([FromRoute] Guid Id)
        {
            var selectService = _InvoiceServices.GetServiceSelect();
            if (selectService == null)
            {
                selectService = new List<SelectServiceDto>();
            }
            ViewData["SelectService"] = selectService;

            var selectArea = _InvoiceServices.GetAreaSelect();
            if (selectArea == null)
            {
                selectArea = new List<SelectAreaDto>();
            }
            ViewData["SelectArea"] = selectArea;
            var result = await _InvoiceServices.GetInvoice(Id);
            return View(result.Data);
        }
        [HttpPost("update-invoice/{Id}")]
        public async Task<IActionResult> UpdateInvoiceAsync([FromRoute] Guid Id, [FromForm] UpdateInvoiceDto request)
        {
            var result = await _InvoiceServices.UpdateInvoice(Id, request);
            if (result.IsSuccessful)
            {
                return RedirectToAction("InvoiceDetail", new { Id = Id });
            }
            return RedirectToAction("Invoice");
        }
    }
}
