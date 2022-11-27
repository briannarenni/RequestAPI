using Microsoft.EntityFrameworkCore;


namespace TicketAPI_Models
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected PersonDbContext()
        {
        }
        public DbSet<Person>? Persons { get; set; } = null!;
    }
}