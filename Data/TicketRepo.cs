// * Account & Ticket Repos
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class TicketRepo
    {
        public TicketRepo() { }

        // Builds ticket objects to add to List<Ticket>
        private Ticket BuildTicket(SqlDataReader reader)
        {
            Ticket ticket = new Ticket();
            ticket.ticketId = (int)reader["ticket_id"];
            ticket.submittedOn = Convert.ToDateTime(reader["submitted_on"]).Date;
            ticket.submittedBy = Convert.ToInt32(reader["submitted_by"]);
            ticket.employeeName = reader["employee_name"].ToString();
            ticket.amount = (double?)(decimal)reader["amount"];
            ticket.category = reader["category"].ToString();
            ticket.status = reader["status"].ToString();

            return ticket;
        }

        public List<Ticket> getAllTickets(string connString)
        {
            string cmdText = @"SELECT * FROM [Ticket] ORDER BY [submitted_on] DESC;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                result.Add(BuildTicket(reader));
            }
            reader.Close();
            return result;
        }

        public List<Ticket> getUserTickets(string connString, int userId)
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
                result.Add(BuildTicket(reader));
            }
            reader.Close();

            return result;
        }

        public List<Ticket> getTicketById(string connString, int id)
        {
            string cmdText = @"SELECT * FROM [Ticket] WHERE [ticket_id] = @id ORDER BY [submitted_on] DESC;";

            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                result.Add(BuildTicket(reader));
            }
            return result;
        }

        public List<Ticket> getPendingTickets(string connString)
        {
            string cmdText = @"SELECT * FROM [View.PendingTickets] ORDER BY [submitted_on] DESC;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                result.Add(BuildTicket(reader));
            }
            return result;
        }

        public List<Ticket> getSinglePending(string connString, int ticketId)
        {
            string cmdText = @"
                SELECT *
                FROM [View.PendingTickets]
                WHERE ticket_id = @id
                AND (SELECT COUNT(*) FROM [View.PendingTickets] WHERE ticket_id = @id) > 0;";
            using SqlConnection connection = new SqlConnection(connString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();
            while (reader.Read())
            {
                result.Add(BuildTicket(reader));
            }
            return result;
        }

        public IResult addTicket(string connString, Ticket ticket)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"INSERT INTO [Ticket] ([submitted_by], [employee_name], [amount], [category])
                VALUES (@submittedBy, @employeeName, @amount, @category);";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@submittedBy", ticket.submittedBy);
            command.Parameters.AddWithValue("@employeeName", ticket.employeeName);
            command.Parameters.AddWithValue("@amount", ticket.amount);
            command.Parameters.AddWithValue("@category", ticket.category);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Created($"/tickets", "Ticket submitted succesfully");
        }

        public IResult updateTicketStatus(string connString, string status, int id)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"UPDATE [Ticket] SET status = @status WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Created($"/tickets/{id}", $"Ticket {id}: {status}");
        }
    }
}
