// * Account & Ticket Repos
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketAPI_Models;

// TODO: Add number of all/pending tickets to user info
namespace TicketAPI_Data
{
    public class UserRepo
    {
        public UserRepo() { }

        public bool checkUsername(string connString, string username)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = command.ExecuteReader();

            return reader.HasRows;
        }

        public bool checkPassword(string connString, string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username AND password = @password;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            using SqlDataReader reader = command.ExecuteReader();
            return reader.HasRows;
        }

        public static (int?, string?) getUserInfo(string connString, string username)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT user_id, username FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            int userId = 0;
            string empName = "";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userId = Convert.ToInt32(reader["user_id"]);
                empName = Convert.ToString(reader["username"]);
            }
            return (userId, empName);
        }

        public void addUser(string connString, string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"INSERT INTO [User] ([username], [password])
                VALUES (@username, @password);";

            using SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            command.ExecuteNonQuery();
            connection.Close();
        }

        // ? TODO: Add method to give manager perms
        public static bool getPerms(string connString, string username)
        {
            bool isManager = false;
            using SqlConnection connection = new SqlConnection(connString);
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
    }
}
