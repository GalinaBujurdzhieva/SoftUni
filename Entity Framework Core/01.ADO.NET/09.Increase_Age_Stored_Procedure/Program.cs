using Microsoft.Data.SqlClient;
using System;

namespace _09.Increase_Age_Stored_Procedure
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            int minionId = int.Parse(Console.ReadLine());
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string increaseAgeProcedureQuery = "EXEC usp_GetOlder @id";
                using (var increaseAgeProcedureCommand = new SqlCommand(increaseAgeProcedureQuery, sqlConnection))
                {
                    increaseAgeProcedureCommand.Parameters.AddWithValue("@id", minionId);
                    increaseAgeProcedureCommand.ExecuteNonQuery();
                }

                string resultQuery = "SELECT Name, Age FROM Minions WHERE Id = @Id";
                using (var resultQueryCommand = new SqlCommand(resultQuery, sqlConnection))
                {
                    resultQueryCommand.Parameters.AddWithValue("@id", minionId);
                    var reader = resultQueryCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} – {reader["Age"]} years old");
                    }
                }
            }
        }
    }
}
