using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class ECarsContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //DbContextOptions<TContext>
        public ECarsContext(DbContextOptions<ECarsContext> options) : base(options) { }
    }
}
