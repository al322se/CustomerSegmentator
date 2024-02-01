using Microsoft.EntityFrameworkCore;

namespace CustomerSegmentator.Data
{
    public class CustomerSegmentatorContext : DbContext
    {
        public CustomerSegmentatorContext (DbContextOptions<CustomerSegmentatorContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerSegmentator.Models.CustomerArrivedEvent> CustomerArrivedEvent { get; set; } = default!;
    }
}