using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _08.Increase_Minion_Age
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            int[] minionsIds = Console.ReadLine().Split().Select(int.Parse).ToArray();
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                foreach (int minionId in minionsIds)
                {
                    string minionsUpdateQuery = @"UPDATE Minions
                                                 SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                 WHERE Id = @Id";
                    using (var updateQueryCommand = new SqlCommand(minionsUpdateQuery, sqlConnection))
                    {
                        updateQueryCommand.Parameters.AddWithValue("@Id", minionId);
                        updateQueryCommand.ExecuteNonQuery();
                    }
                }
                string minionResultQuery = "SELECT Name, Age FROM Minions";
                using (var resultCommand = new SqlCommand(minionResultQuery, sqlConnection))
                {
                    using (var reader = resultCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                        }
                    }
                }
            }
            
        }
    }
}
