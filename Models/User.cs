namespace TicketAPI_Models
{
    public class User
    {
        public int? userId { get; set; } // Filled by DB
        public string? username { get; set; }
        string? password { get; set; }
        public bool? isManager { get; set; } // Filled by DB

        // Constructors
        public User() { }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.isManager = false;
        }

        public User(string username, string password, bool perms)
        {
            this.username = username;
            this.password = password;
            this.isManager = perms;
        }
    }
}
