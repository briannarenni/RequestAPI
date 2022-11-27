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

        // User Methods


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

    }
}