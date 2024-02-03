using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CustomerSegmentator.Data;
using CustomerSegmentator.Models;
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

        public async Task<IActionResult> Index()
        {
            return
                View(Array.Empty<CustomerArrivedEvent>());

        }

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

        // GET: CustomerArrivedEvent/Create
        public IActionResult Create()
        {
            PopulateItemsInViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,Segment,Tarriff,PaymentType")] CustomerArrivedEvent customerArrivedEvent)
        {
            if (ModelState.IsValid)
            {
                customerArrivedEvent.Date=DateTimeOffset.Now;
                _context.Add(customerArrivedEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerArrivedEvent);
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

public static class CsvWriteHelper
{
    public const string ContentType = "text/csv";

    public static async Task<byte[]> WriteCsv(Func<CsvWriter, Task> writeRecords)
    {
        await using var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        await using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        await writeRecords(csv);
        await csv.FlushAsync();
        await writer.FlushAsync();
        return memoryStream.ToArray();
    }

    public static async Task WriteToCsvAsync<T>(this CsvWriter csvWriter, IReadOnlyCollection<T> records, CancellationToken cancellationToken)
    {
        csvWriter.WriteHeader(typeof(T));
        await csvWriter.NextRecordAsync();
        await csvWriter.WriteRecordsAsync(records, cancellationToken);
        await csvWriter.NextRecordAsync();
    }
}