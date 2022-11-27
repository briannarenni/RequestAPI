using Microsoft.EntityFrameworkCore;


namespace TicketAPI_DBModels
{
    public class UserDB : DbContext
    {
        public UserDB(DbContextOptions<UserDB> options)
            : base(options)
        {

        }

        public DbSet<User>? Users { get; set; } = null!;
    }
}