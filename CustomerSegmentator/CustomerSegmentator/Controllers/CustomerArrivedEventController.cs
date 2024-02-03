using CustomerSegmentator.Data;
using CustomerSegmentator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Client;

namespace CustomerSegmentator.Controllers
{
    public class CustomerArrivedEventController : Controller
    {
        private readonly CustomerSegmentatorContext _context;
        private readonly IOptions<AppOptions> _appOptions;

        public CustomerArrivedEventController(CustomerSegmentatorContext context,IOptions<AppOptions> appOptions )
        {
            _context = context;
            _appOptions = appOptions;
            context.Database.Migrate();
        }

        [Authorize(Roles = "Director")]
        public async Task<IActionResult> Director()
        {
            return
                View(Array.Empty<CustomerArrivedEvent>());

        }

        [Authorize(Roles = "Director")]
        public async Task<IActionResult> DownloadCsv(CancellationToken cancellationToken)
        {
            const string fileName = "customerSegments.csv";

            var records= await _context.CustomerArrivedEvent.ToListAsync(cancellationToken: cancellationToken);
            var bytes = await CsvWriteHelper.WriteCsv(async csv =>
            {
                await csv.WriteToCsvAsync(records, cancellationToken);
            });

            return File(bytes, CsvWriteHelper.ContentType, fileName);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Administrator()
        {
            PopulateItemsInViewBag();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,Segment,Tarriff,PaymentType")] CustomerArrivedEvent customerArrivedEvent)
        {
            if (ModelState.IsValid)
            {
                customerArrivedEvent.Date=DateTimeOffset.Now;
                _context.Add(customerArrivedEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Administrator));
            }

            return   RedirectToAction(nameof(Administrator));
        }

        private void PopulateItemsInViewBag()
        {
            var segments = _appOptions.Value.Segments.Select(segment => new SelectListItem(segment, segment)).ToList();
            ViewData.Add("Segments", segments);
            var tariffs = _appOptions.Value.Tariffs.Select(tariff => new SelectListItem(tariff, tariff)).ToList();
            ViewData.Add("Tarrifs", tariffs);
            var paymentTypes = _appOptions.Value.PaymentTypes.Select(pt => new SelectListItem(pt, pt)).ToList();
            ViewData.Add("PaymentTypes", paymentTypes);
        }
    }
}