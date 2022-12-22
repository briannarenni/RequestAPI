using Microsoft.EntityFrameworkCore;

// Old connection string
// static StreamReader conFile = new System.IO.StreamReader("/Users/briannarene/_code/_tools/connection-strings/request-DB.txt");
// static string connString = conFile.ReadToEnd();

// FROM PROGRAM.CS
// builder.Services.AddDbContext<UserDB>(options =>
//         options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
// builder.Services.AddDbContext<TicketDB>(options =>
//         options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

namespace TicketAPI_DBModels
{
    public class RequestDB : DbContext
    {
        public RequestDB(DbContextOptions<UserDB> options)
            : base(options)
        {

        }

        public RequestDB(DbContextOptions<TicketDB> options)
            : base(options)
        {

        }

        public DbSet<User>? Users { get; set; } = null!;
        public DbSet<User>? Tickets { get; set; } = null!;
    }
}
