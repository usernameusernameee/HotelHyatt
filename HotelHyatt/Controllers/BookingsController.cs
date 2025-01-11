using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelHyatt.Data;
using HotelHyatt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Net.Mail;
using System.Net;

namespace HotelHyatt.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly HotelHyattContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        public BookingsController(HotelHyattContext context,
            UserManager<IdentityUser> userManager,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Booking.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }
        public string EmailConfirmationUrl { get; set; }
        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,RoomType,CheckInDate,CheckOutDate")] Booking booking,
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
            }
        

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,RoomType,CheckInDate,CheckOutDate")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Bookings/Create
        /*public IActionResult CreateConfirmation()
        {
            return View();
        }
        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmation([Bind("Id,FirstName,LastName,RoomType,CheckInDate,CheckOutDate")] Booking booking,
            string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                IdentityUser user = await GetCurrentUserAsync();
                return RedirectToPage("BookingConfirmation", new { email = user.Email, returnUrl = returnUrl });
                var userId1 = await _userManager.GetUserIdAsync(user);
                var code1 = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code1 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code1));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId1, code = code1, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                using (var client = new SmtpClient())
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("nomn28097@gmail.com", "xrmbohiqvergwzbd");
                    using (var message = new MailMessage(
                        from: new MailAddress("nomn28097@gmail.com", "mohamed idrissi"),
                        to: new MailAddress(user.Email, user.Email)
                        ))
                    {

                        message.Subject = "Confirm your Booking";
                        message.Body = $"Please confirm your account by clicking here {EmailConfirmationUrl}";

                        client.Send(message);
                    }
                }
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }*/

        public async Task<IActionResult> GeneratePdf()
        {
            //this.View("Index").ToHtml(HttpContext);
            var booking = await _context.Booking.ToListAsync();
            if (booking.Count != 0)
            {
                _logger.LogInformation("*******************************"+ booking[0].FirstName);
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<!DOCTYPE html><html lang='en'><head>    <meta charset='utf-8' />    <meta name='viewport' content='width=device-width, initial-scale=1.0' />    <title>"+@ViewData["Title"]+" - HotelHyatt</title>    <script type='importmap'></script>    <link rel='stylesheet' href='~/lib/bootstrap/dist/css/bootstrap.min.css' />    <link rel='stylesheet' href='~/css/site.css' asp-append-version='true' />    <link rel='stylesheet' href='~/HotelHyatt.styles.css' asp-append-version='true' /></head><body><table class='table'>    <thead>        <tr>            <th>               FirstName            </th>            <th>                LastName            </th>            <th>                RoomType            </th>            <th>                CheckInDate            </th>            <th>               CheckOutDate            </th>            <th></th>        </tr>    </thead>    <tbody>");
                foreach (var item in booking)
                {
                    sbInterest.Append("<tr>            <td>"+                item.FirstName+"            </td>            <td>                "+item.LastName+"            </td>            <td>                "+item.RoomType+"            </td>            <td>                "+item.CheckInDate+"            </td>            <td>                "+item.CheckOutDate+"            </td></tr>");
                }
                sbInterest.Append("</tbody></table></body></html>");
                var renderer = new ChromePdfRenderer();
                using var pdf = renderer.RenderHtmlAsPdf(sbInterest.ToString());
                return File(pdf.BinaryData, "application/pdf", "output.pdf");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        
    }
}
