// * Account & Ticket Repos
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class SqlRepo
    {
        public SqlRepo() { }

        // Returned data will build ticket object
        private Ticket BuildTicket(SqlDataReader reader)
        {
            Ticket ticket = new Ticket();
            ticket.ticketId = (int)reader["ticket_id"];
            ticket.submittedOn = Convert.ToDateTime(reader["submitted_on"]).Date;
            ticket.employeeName = reader["employee_name"].ToString();
            ticket.amount = (double?)(decimal)reader["amount"];
            ticket.category = reader["category"].ToString();
            ticket.status = reader["status"].ToString();

            return ticket;
        }

        // Get all tickets
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


        // Get all employee's tickets
        public List<Ticket> getAllUserTickets(string connString, int userId)
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

        // Get single ticket
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
            reader.Close();

            return result;
        }

        // Check that pending ticket exits
        public bool checkPendingTickets(string connString, int id)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        // get all pending
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
            reader.Close();
            return result;
        }

        // Get single pending
        public List<Ticket> getSinglePending(string connString, int ticketId)
        {
            using SqlConnection connection = new SqlConnection(connString);
            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            using SqlDataReader reader = command.ExecuteReader();

            List<Ticket> result = new List<Ticket>();
            bool exists = checkPendingTickets(connString, ticketId);
            if (exists)
            {
                connection.Open();
                while (reader.Read())
                {
                    result.Add(BuildTicket(reader));
                }
                reader.Close();
            }
            return result;
        }

        // TODO: User Methods

    }
}
