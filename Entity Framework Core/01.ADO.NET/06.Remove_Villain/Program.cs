using Microsoft.Data.SqlClient;
using System;

namespace _06.Remove_Villain
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @villainId";
                using (var villainNameCommand = new SqlCommand(villainNameQuery, sqlConnection))
                {
                    villainNameCommand.Parameters.AddWithValue("@villainId", villainId);
                    var villainNameAsQueryResult = villainNameCommand.ExecuteScalar();
                    if (villainNameAsQueryResult == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }
                    string villainName = (string)villainNameAsQueryResult;
                    string deleteFromMinionsVillainQuery = @"DELETE FROM MinionsVillains 
                                                           WHERE VillainId = @villainId";
                    using (var deleteFromMinionsVillainCommand = new SqlCommand(deleteFromMinionsVillainQuery, sqlConnection))
                    {
                        deleteFromMinionsVillainCommand.Parameters.AddWithValue("@villainId", villainId);
                        int deletedMinionsCount = deleteFromMinionsVillainCommand.ExecuteNonQuery();

                        string deleteVillainQuery = @"DELETE FROM Villains
                                                      WHERE Id = @villainId";
                        using (var deleteVillainCommand = new SqlCommand(deleteVillainQuery, sqlConnection))
                        {
                            deleteVillainCommand.Parameters.AddWithValue("@villainId", villainId);
                            deleteVillainCommand.ExecuteNonQuery();

                            Console.WriteLine($"{villainName} was deleted.");
                            Console.WriteLine($"{deletedMinionsCount} minions were released.");
                        }
                    }
                }
            }
        }
    }
}
