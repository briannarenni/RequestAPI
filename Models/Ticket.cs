namespace TicketAPI_Models
{
    public class Ticket
    {
        public int? ticketId { get; set; }
        public DateTime? submittedOn { get; set; }
        public int? submittedBy { get; set; }
        public string? employeeName { get; set; }
        public string? status { get; set; }
        public double? amount { get; set; }
        public string? category { get; set; }

        // Constructors
        public Ticket() { }

        public Ticket(int userId, string username, double amount, string category)
        {
            this.submittedBy = userId;
            this.employeeName = username;
            this.amount = amount;
            this.category = category;
        }

        public Ticket(int ticketId, DateTime submittedOn, int userId, string username, double amount, string category)
        {
            this.ticketId = ticketId;
            this.submittedOn = submittedOn;
            this.submittedBy = userId;
            this.employeeName = username;
            this.amount = amount;
            this.category = category;
        }
    }
}