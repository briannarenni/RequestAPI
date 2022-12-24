// * Account & Ticket Repos
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class UserRepo
    {
        public UserRepo() {}

        // TODO: User Methods
        // Builds ticket objects to add to List<Ticket>
        // private Ticket BuildTicket(SqlDataReader reader)
        // {
        //     Ticket ticket = new Ticket();
        //     ticket.ticketId = (int)reader["ticket_id"];
        //     ticket.submittedOn = Convert.ToDateTime(reader["submitted_on"]).Date;
        //     ticket.submittedBy = Convert.ToInt32(reader["submitted_by"]);
        //     ticket.employeeName = reader["employee_name"].ToString();
        //     ticket.amount = (double?)(decimal)reader["amount"];
        //     ticket.category = reader["category"].ToString();
        //     ticket.status = reader["status"].ToString();

        //     return ticket;
        // }

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
                // result.Add(BuildTicket(reader));
            }
            reader.Close();
            return result;
        }
    }
}
