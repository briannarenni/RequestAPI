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

        // Ticket Methods
        public List<Ticket> getAllTickets(string connectionString)
        {
            string cmdText = @"SELECT * FROM [Ticket] ORDER BY [submitted_on] DESC;";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(cmdText, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                Ticket ticket = new Ticket();
                // int ticketId, DateTime submittedOn, string employee_name, string username, double amount, string category
                ticket.ticketId = (int)reader["ticket_id"];
                ticket.submittedOn = (DateTime)reader["submitted_on"]; // ? change to DateOnly
                ticket.employeeName = reader["employee_name"].ToString();
                ticket.amount = (double?)(decimal)reader["amount"];

                ticket.category = reader["category"].ToString();

                result.Add(ticket);
            }
            reader.Close();

            return result;
        }

        public List<Ticket> getAllUserTickets(string connectionString, int userId)
        {
            string cmdText = @"SELECT * FROM [Ticket] WHERE [submitted_by] = @userId;";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@userId", userId);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                Ticket ticket = new Ticket();
                // int ticketId, DateTime submittedOn, string employee_name, string username, double amount, string category
                ticket.ticketId = (int)reader["ticket_id"];
                ticket.submittedOn = (DateTime)reader["submitted_on"]; // ? change to DateOnly
                ticket.employeeName = reader["employee_name"].ToString();
                ticket.amount = (double?)(decimal)reader["amount"];

                ticket.category = reader["category"].ToString();

                result.Add(ticket);
            }
            reader.Close();

            return result;
        }

        public List<Ticket> getTicketById(string connectionString, int id)
        {
            string cmdText = @"SELECT * FROM [Ticket] WHERE [ticket_id] = @id ORDER BY [submitted_on] DESC;";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Ticket> result = new List<Ticket>();

            while (reader.Read())
            {
                Ticket ticket = new Ticket();
                // int ticketId, DateTime submittedOn, string employee_name, string username, double amount, string category
                ticket.ticketId = (int)reader["ticket_id"];
                ticket.submittedOn = (DateTime)reader["submitted_on"]; // ? change to DateOnly
                ticket.employeeName = reader["employee_name"].ToString();
                ticket.amount = (double?)(decimal)reader["amount"];

                ticket.category = reader["category"].ToString();

                result.Add(ticket);
            }
            reader.Close();

            return result;
        }

        // TODO: get all pending and get one pending
        public static bool getSinglePending(string connectionString, int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        // User Methods


    }
}
