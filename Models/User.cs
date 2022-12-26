namespace TicketAPI_Models
{
    public class User
    {
        public int? userId { get; set; }
        public string? username { get; set; }
        string? password { get; set; }
        public string? role { get; set; }
        public int? numPending { get; set; }
        public int? numTickets { get; set; }

        public User() { }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        // ! May not be needed
        public User(int userId, string username, string role, int numPending, int numTickets)
        {
            this.username = username;
            this.password = password;
            this.role = "";
            this.numPending = 0;
            this.numTickets = 0;
        }
    }
}
