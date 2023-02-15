using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace TicketAPI_Data
{
    public class Validators
    {
        public static bool matchPasswords(string input1, string input2) => input1 == input2;

        public static bool checkUsername(string connString, string username)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            string cmdText = @"SELECT * FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            using SqlDataReader reader = command.ExecuteReader();
            return reader.HasRows;
        }

        public static bool checkPassword(string connString, string username, string password)
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
    }
}
