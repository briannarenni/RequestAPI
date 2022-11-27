using Microsoft.EntityFrameworkCore;

namespace TicketAPI_Models
{

    public class TicketDB : DbContext
    {
        public TicketDB(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Ticket>? Tickets { get; set; } = null!;
    }
}