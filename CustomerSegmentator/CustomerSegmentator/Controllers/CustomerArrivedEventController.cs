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

        // GET: CustomerArrivedEvent
        public async Task<IActionResult> Index()
        {
              return _context.CustomerArrivedEvent != null ? 
                          View(await _context.CustomerArrivedEvent.ToListAsync()) :
                          Problem("Entity set 'CustomerSegmentatorContext.CustomerArrivedEvent'  is null.");
        }

        // GET: CustomerArrivedEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerArrivedEvent == null)
            {
                return NotFound();
            }

            var customerArrivedEvent = await _context.CustomerArrivedEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerArrivedEvent == null)
            {
                return NotFound();
            }

            return View(customerArrivedEvent);
        }

        // GET: CustomerArrivedEvent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerArrivedEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: CustomerArrivedEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerArrivedEvent == null)
            {
                return NotFound();
            }

            var customerArrivedEvent = await _context.CustomerArrivedEvent.FindAsync(id);
            if (customerArrivedEvent == null)
            {
                return NotFound();
            }
            return View(customerArrivedEvent);
        }

        // POST: CustomerArrivedEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Quantity,Segment,Tarriff,IsCash")] CustomerArrivedEvent customerArrivedEvent)
        {
            if (id != customerArrivedEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerArrivedEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerArrivedEventExists(customerArrivedEvent.Id))
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
            return View(customerArrivedEvent);
        }

        // GET: CustomerArrivedEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerArrivedEvent == null)
            {
                return NotFound();
            }

            var customerArrivedEvent = await _context.CustomerArrivedEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerArrivedEvent == null)
            {
                return NotFound();
            }

            return View(customerArrivedEvent);
        }

        // POST: CustomerArrivedEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerArrivedEvent == null)
            {
                return Problem("Entity set 'CustomerSegmentatorContext.CustomerArrivedEvent'  is null.");
            }
            var customerArrivedEvent = await _context.CustomerArrivedEvent.FindAsync(id);
            if (customerArrivedEvent != null)
            {
                _context.CustomerArrivedEvent.Remove(customerArrivedEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerArrivedEventExists(int id)
        {
          return (_context.CustomerArrivedEvent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
