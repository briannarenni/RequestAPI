using Microsoft.EntityFrameworkCore;

namespace TicketAPI_DBModels
{

    public class TicketDB : DbContext
    {
        public TicketDB(DbContextOptions<TicketDB> options)
            : base(options)
        {

        }

        public DbSet<Ticket>? Tickets { get; set; } = null!;
    }
}