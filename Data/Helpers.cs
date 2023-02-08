using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class Helpers
    {
        public static Ticket buildTicket(SqlDataReader reader)
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

        public static User buildUser(SqlDataReader reader, string role, int pending, int tickets)
        {
            User user = new User();
            user.userId = (int)reader["user_id"];
            user.username = reader["username"].ToString();
            user.role = role;
            user.numPending = pending;
            user.numTickets = tickets;
            return user;
        }

        public static string getRole(string connString, string username)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string role = "";
            int isManager = 0;

            string cmdText = @"SELECT is_manager FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isManager = Convert.ToInt32(reader["is_manager"]);
            }

            role = isManager % 2 == 0 ? "employee" : "manager";
            return role;
        }

        public static (int, int) countTickets(string connString, string username)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            int numPending = 0;
            int numTickets = 0;
            string cmdText = @"SELECT COUNT(*) AS num_pending FROM [View.PendingTickets]
                WHERE employee_name = @username
                UNION ALL
                SELECT COUNT(*) AS num_tickets FROM [Ticket] WHERE employee_name = @username";

            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                numPending = Convert.ToInt32(reader[0]);
            }
            if (reader.Read())
            {
                numTickets = Convert.ToInt32(reader[0]);
            }
            return (numPending, numTickets);
        }

        public static bool? getPerms(string connString, int userId)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"SELECT is_manager FROM [User] WHERE [user_id] = @userId;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@userId", userId);
            bool isManager = (bool)command.ExecuteScalar();
            return isManager;
        }
    }
}





