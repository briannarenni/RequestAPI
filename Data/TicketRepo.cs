using System.Data.SqlClient;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class TicketRepo
    {
        private readonly string? connString;

        public TicketRepo() { }

        public TicketRepo(IConfiguration configuration)
        {
            connString = configuration.GetValue<string>("ConnectionStrings:request-connString");
        }

        public List<Ticket> getAllTickets()
        {
            string cmdText = @"SELECT * FROM [Ticket] ORDER BY [submitted_on] DESC;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            List<Ticket> result = new List<Ticket>();
            while (reader.Read())
            {
                result.Add(Helpers.buildTicket(reader));
            }
            reader.Close();
            return result;
        }

        public List<Ticket> getPendingTickets()
        {
            string cmdText = @"SELECT * FROM [View.PendingTickets] ORDER BY [submitted_on] DESC;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            List<Ticket> result = new List<Ticket>();
            while (reader.Read())
            {
                result.Add(Helpers.buildTicket(reader));
            }
            return result;
        }

        public List<Ticket> getUserTickets(int userId)
        {
            string cmdText = @"SELECT * FROM [Ticket] WHERE [submitted_by] = @userId;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            List<Ticket> result = new List<Ticket>();
            while (reader.Read())
            {
                result.Add(Helpers.buildTicket(reader));
            }
            reader.Close();
            return result;
        }

        public Ticket? getTicketById(int ticketId)
        {
            string cmdText = @"SELECT * FROM [Ticket] WHERE [ticket_id] = @id;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            return (reader.Read()) ? Helpers.buildTicket(reader) : null;
        }

        public Ticket? getSinglePending(int ticketId)
        {
            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @id";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            return (reader.Read()) ? Helpers.buildTicket(reader) : null;
        }

        public IResult addTicket(Ticket ticket)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"INSERT INTO [Ticket] ([submitted_by], [employee_name], [amount], [category], [comments])
                VALUES (@submittedBy, @employeeName, @amount, @category, @comments);";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@submittedBy", ticket.submittedBy);
            command.Parameters.AddWithValue("@employeeName", ticket.employeeName);
            command.Parameters.AddWithValue("@amount", ticket.amount);
            command.Parameters.AddWithValue("@category", ticket.category);
            command.Parameters.AddWithValue("@comments", ticket.comments);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Created($"/tickets", "Request submitted succesfully");
        }

        public IResult updateTicketStatus(int ticketId, string status)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"UPDATE [Ticket] SET status = @status WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@id", ticketId);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Ok($"Ticket {ticketId} has been {status}");
        }
    }
}
