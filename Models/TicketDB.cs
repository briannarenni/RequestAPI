using Microsoft.EntityFrameworkCore;

namespace TicketAPI_Models
{

    public class TicketDb : DbContext
    {
        public TicketDb(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Ticket>? Tickets { get; set; } = null!;
    }
}