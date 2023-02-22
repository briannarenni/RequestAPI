namespace TicketAPI_Models
{
    public class User
    {
        public int? userId { get; set; }
        public string? username { get; set; }
        string? password { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? role { get; set; }
        public string? dept { get; set; }
        public int? numPending { get; set; }
        public int? numTickets { get; set; }

        public User() { }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        // ! May not be needed
        public User(int userId, string username, string fName, string lName, string role, string dept, int numPending, int numTickets)
        {
            this.username = username;
            this.password = password;
            this.firstName = fName;
            this.lastName = lName;
            this.role = "";
            this.dept = dept;
            this.numPending = 0;
            this.numTickets = 0;
        }
    }
}
