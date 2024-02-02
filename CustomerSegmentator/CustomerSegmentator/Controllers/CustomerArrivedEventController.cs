using CustomerSegmentator.Data;
using CustomerSegmentator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSegmentator.Controllers
{
    public class CustomerArrivedEventController : Controller
    {
        private readonly CustomerSegmentatorContext _context;

        public CustomerArrivedEventController(CustomerSegmentatorContext context)
        {
            _context = context;
            context.Database.Migrate();
        }

        public async Task<IActionResult> Index()
        {
              return _context.CustomerArrivedEvent != null ? 
                          View(await _context.CustomerArrivedEvent.ToListAsync()) :
                          Problem("Entity set 'CustomerSegmentatorContext.CustomerArrivedEvent'  is null.");
        }

        // GET: CustomerArrivedEvent/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,Segment,Tarriff,IsCash")] CustomerArrivedEvent customerArrivedEvent)
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
    }
}
