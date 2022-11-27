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

namespace TicketAPI_Data
{
    public class oldSqlRepo
    {


        // User account methods
        public static bool checkUsername(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        public static bool checkPassword(string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username AND password = @password;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using SqlDataReader reader = command.ExecuteReader();
            // while (reader.Read())
            // {
            //     return true; // Exists
            // }
            return reader.HasRows;
        }

        public static bool getPerms(string username)
        {
            bool isManager = false;
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT is_manager FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int isManagerInt = Convert.ToInt32(reader["is_manager"]);
                isManager = isManagerInt % 2 != 0;

            }
            return isManager;
        }

        public static void addUser(string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string cmdText = @"INSERT INTO [User] ([username], [password])
                VALUES (@username, @password);";

            using SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public static (int, bool) getUserInfo(string username)
        {
            // add id and pending tickets to User
            int userId = 0;
            bool isManager = false;

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT user_id, is_manager FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userId = Convert.ToInt32(reader["user_id"]);
                int isManagerInt = Convert.ToInt32(reader["is_manager"]);

                isManager = isManagerInt % 2 != 0;
            }
            return (userId, isManager);
        }



        // Ticket Methods
        public static DataTable getTickets(string query)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            DataTable result = new DataTable();

            if (reader.HasRows)
            {
                result.Load(reader);
            }
            else
            {
                throw new Exception("No records found.");
            }

            return result;
            connection.Close();
        }

        public static DataTable getTickets(string query, int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            DataTable result = new DataTable();


            if (reader.HasRows)
            {
                result.Load(reader);
            }
            else
            {
                throw new Exception("No records found.");
            }

            return result;
            connection.Close();
        }

        public static bool checkPending(string query, int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @id;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        // Update Methods (add/process requests)
        public static void addNewTicket(int userId, string username, double amount, string category)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"INSERT INTO [Ticket] ([submitted_by], [employee_name], [amount], [category])
                VALUES (@userId, @username, @amount, @category);";
            using SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@category", category);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public static bool checkPending(int ticketId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM [View.PendingTickets] WHERE ticket_id = @ticketId;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@ticketId", ticketId);

            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        public static void updatePendingRequest(string status, int id, string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = @"UPDATE [Ticket] SET status = @status WHERE ticket_id = @id;";

            using SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}