namespace TicketAPI_Models
{
    public class User
    {
        public int? userId { get; set; }
        public string? username { get; set; }
        string? password { get; set; }
        public bool? isManager { get; set; }

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
