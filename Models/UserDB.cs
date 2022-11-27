using Microsoft.EntityFrameworkCore;


namespace TicketAPI_Models
{
    public class UserDB : DbContext
    {
        public UserDB(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User>? Users { get; set; } = null!;
    }
}