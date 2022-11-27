using Microsoft.EntityFrameworkCore;


namespace TicketAPI_Models
{
    public class UserDb : DbContext
    {
        public UserDb(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User>? Users { get; set; } = null!;
    }
}