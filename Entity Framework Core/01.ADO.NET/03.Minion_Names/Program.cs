using Microsoft.Data.SqlClient;
using System;

namespace _03.Minion_Names
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                int villainId = int.Parse(Console.ReadLine());
                string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @Id";
                using (var sqlCommand = new SqlCommand(villainNameQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", villainId);
                    var villainNameAsQueryResult = sqlCommand.ExecuteScalar();
                    if (villainNameAsQueryResult == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }
                    Console.WriteLine($"Villain: {villainNameAsQueryResult}"); 
                }

                string minionsForGivenVillain = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                             m.Name, 
                                                             m.Age
                                                      FROM MinionsVillains AS mv
                                                      JOIN Minions As m ON mv.MinionId = m.Id
                                                      WHERE mv.VillainId = @Id
                                                      ORDER BY m.Name";
                using (var sqlCommand = new SqlCommand(minionsForGivenVillain, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", villainId);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                            return;
                        }
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
                        }
                    }
                }
            }
        }
    }
}
