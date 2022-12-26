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
        public UserRepo() { }

        public User getUserInfo(string connString, string username)
        {
            User result = new User();
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                (int, int) ticketCount = Helpers.countTickets(connString, username);
                string role = Helpers.getRole(connString, username);
                int pending = ticketCount.Item1;
                int tickets = ticketCount.Item2;
                result = Helpers.buildUser(reader, role, pending, tickets);
            }

            return result;
        }

        public bool validateRegistration(string connString, string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connString);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE username = @username;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = command.ExecuteReader();
            return reader.HasRows;
        }

        // For Login
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

        // For Login
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

        public IResult addUser(string connString, string username, string password)
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
            return Results.Created($"/tickets", "Ticket submitted succesfully");

        }

        // * Finish adding
        // public List<User> geEmployees(string connString)
        // {
        //     string cmdText = @"SELECT * FROM [User] WHERE [is_manager] = 0;";
        //     using SqlConnection connection = new SqlConnection(connString);
        //     using SqlCommand command = new SqlCommand(cmdText, connection);

        //     connection.Open();
        //     using SqlDataReader reader = command.ExecuteReader();
        //     List<Ticket> result = new List<Ticket>();

        //     while (reader.Read())
        //     {
        //         result.Add(BuildTicket(reader));
        //     }
        //     reader.Close();

        //     return result;
        // }


    }
}
