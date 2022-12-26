using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using TicketAPI_Models;

namespace TicketAPI_Data
{
    public class Validators
    {
        public static bool matchPasswords(string input1, string input2) => input1 == input2;

        // public static bool usernameExists() {}
    }
}
