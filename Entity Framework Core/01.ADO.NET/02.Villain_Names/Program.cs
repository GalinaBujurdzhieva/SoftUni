using Microsoft.Data.SqlClient;
using System;

namespace _02.Villain_Names
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string villainNamesQuery = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount 
                                             FROM Villains AS v 
                                             JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                             GROUP BY v.Id, v.Name 
                                             HAVING COUNT(mv.VillainId) > 3 
                                             ORDER BY COUNT(mv.VillainId)";
                using (var sqlCommand = new SqlCommand(villainNamesQuery, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                        }
                    }
                }
            }
        }
    }
}
