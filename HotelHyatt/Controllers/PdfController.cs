using HotelHyatt.Data;
using HotelHyatt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelHyatt.Controllers
{
    public class PdfController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelHyattContext _context;
        public PdfController(HotelHyattContext context,
            ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> GeneratePdf()
        {
            var booking = await _context.Booking.ToListAsync();
            _logger.LogInformation("*******************************" + booking[0].FirstName);
            var renderer = new ChromePdfRenderer();
            using var pdf = renderer.RenderHtmlAsPdf("");
            
            return (IActionResult)File(pdf.BinaryData, "application/pdf", "output.pdf");
        }

        
    }
}
