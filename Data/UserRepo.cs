using System.Data.SqlClient;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class UserRepo
    {
        private readonly string? connString;

        public UserRepo() { }

        public UserRepo(IConfiguration configuration)
        {
            connString = configuration.GetValue<string>("ConnectionStrings:request-connString");
        }

        public IResult validateLogin(string username, string password)
        {
            bool existingUsername = Validators.checkUsername(connString!, username);
            bool validPassword = Validators.checkPassword(connString!, username, password);

            if (!existingUsername)
            {
                return Results.BadRequest("Username incorrect");
            }

            if (!validPassword)
            {
                return Results.BadRequest("Password incorrect");
            }

            int userId = Helpers.getAuthUser(connString!, username);
            User authUser = getUserInfo(userId);

            return Results.Ok(authUser);
        }

        public IResult validateRegister(string firstName, string lastName, string username, string password, string dept)
        {
            bool existingUsername = Validators.checkUsername(connString!, username);

            if (existingUsername)
            {
                return Results.BadRequest("Username already registered");
            }

            addUser(firstName, lastName, username, dept);
            int userId = Helpers.getAuthUser(connString!, username);
            addPassword(userId, username, password);
            User authUser = getUserInfo(userId);

            return Results.Ok(authUser);
        }

        public User getUserInfo(int userId)
        {
            User user = new User();
            using SqlConnection connection = new SqlConnection(connString!);
            connection.Open();

            string cmdText = @"SELECT * FROM [User] WHERE user_id = @userId;";
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@userId", userId);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                (int, int) ticketCount = Helpers.countTickets(connString!, userId);
                string role = Helpers.getRole(connString!, userId);
                int pending = ticketCount.Item1;
                int tickets = ticketCount.Item2;
                user = Helpers.buildUser(reader, role, pending, tickets);
            }
            return user;
        }

        public IResult addUser(string firstName, string lastName, string username, string dept = "")
        {
            using SqlConnection connection = new SqlConnection(connString!);
            connection.Open();
            string cmdText = @"INSERT INTO [User] ( [first_name], [last_name], [username], [dept])
                VALUES (@first_name, @last_name, @username, @dept);";

            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@first_name", firstName);
            command.Parameters.AddWithValue("@last_name", lastName);
            command.Parameters.AddWithValue("@dept", dept);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Ok("Registered successfully");
        }

        public IResult addPassword(int userId, string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connString!);
            connection.Open();
            string cmdText = @"INSERT INTO [Password] ([user_id], [username], [password])
        VALUES (@user_id, @username, @password);";

            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
            connection.Close();
            return Results.Ok("Password stored successfully");
        }

        public IResult updatePassword(int userId, string pw1, string pw2)
        {
            bool passwordsMatch = Validators.matchPasswords(pw1, pw2);
            if (!passwordsMatch)
            {
                return Results.BadRequest("Passwords do not match. Please enter again.");
            }
            else
            {
                using SqlConnection connection = new SqlConnection(connString!);
                connection.Open();
                string cmdText = @"UPDATE [Password] SET [password] = @password WHERE [user_id] = @userId;";
                using SqlCommand command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@password", pw2);
                command.ExecuteNonQuery();
                connection.Close();
                return Results.Ok("Password updated successfully");
            }
        }

        public IResult changeRole(int userId)
        {
            bool? currPerms = Helpers.getPerms(connString!, userId);
            bool? newRole = false;
            if (currPerms == null)
            {
                return Results.BadRequest("Invalid User ID");
            }
            else
            {
                newRole = !currPerms;
                using SqlConnection connection = new SqlConnection(connString!);
                connection.Open();
                string cmdText = @"UPDATE [User] SET [is_manager] = @newRole WHERE [user_id] = @userId;";
                using SqlCommand command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@newRole", newRole);
                command.ExecuteNonQuery();
                connection.Close();
                return Results.Ok($"User role changed to {((newRole == true) ? "Manager" : "Employee")}");
            }
        }

        public List<User> getEmployees()
        {
            string cmdText = @"SELECT * FROM [User] WHERE [is_manager] = 0 ORDER BY [dept];";
            using SqlConnection connection = new SqlConnection(connString!);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<User> result = new List<User>();
            while (reader.Read())
            {
                int userId = (int)reader.GetValue(0);
                (int, int) ticketCount = Helpers.countTickets(connString!, userId);
                string role = Helpers.getRole(connString!, userId);
                int pending = ticketCount.Item1;
                int tickets = ticketCount.Item2;
                result.Add(Helpers.buildUser(reader, role, pending, tickets));
            }
            reader.Close();
            return result;
        }

        public List<User> getUsers()
        {
            string cmdText = @"SELECT * FROM [User] ORDER BY [dept]";
            using SqlConnection connection = new SqlConnection(connString!);
            using SqlCommand command = new SqlCommand(cmdText, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<User> result = new List<User>();
            while (reader.Read())
            {
                int userId = (int)reader.GetValue(0);
                (int, int) ticketCount = Helpers.countTickets(connString!, userId);
                string role = Helpers.getRole(connString!, userId);
                int pending = ticketCount.Item1;
                int tickets = ticketCount.Item2;
                result.Add(Helpers.buildUser(reader, role, pending, tickets));
            }
            reader.Close();
            return result;
        }

        public string getPassword(int userId)
        {
            string? storedPassword = "";
            using SqlConnection connection = new SqlConnection(connString!);
            string cmdText = @"SELECT * FROM [Password] WHERE [user_id] = @userId;";
            connection.Open();
            using SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@userId", userId);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                storedPassword = reader["password"].ToString();
            }
            reader.Close();
            return storedPassword;
        }
    }
}
