using Microsoft.EntityFrameworkCore;

namespace TicketAPI_Models
{
    public class RequestDB : DbContext
    {
        public RequestDB(DbContextOptions<RequestDB> options) : base(options) { }

        public DbSet<User>? Users => Set<User>();
        public DbSet<Ticket>? Tickets => Set<Ticket>();

    }


}