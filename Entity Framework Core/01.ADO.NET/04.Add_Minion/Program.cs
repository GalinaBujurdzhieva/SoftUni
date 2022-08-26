using Microsoft.Data.SqlClient;
using System;

namespace _04.Add_Minion
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            string[] minnionInput = Console.ReadLine().Split();
            string minionName = minnionInput[1];
            int minionAge = int.Parse(minnionInput[2]);
            string minionTownName = minnionInput[3];
            string[] villainInput = Console.ReadLine().Split();
            string villainName = villainInput[1];

            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();

                string townNameQuery = "SELECT Id FROM Towns WHERE Name = @townName";
                using (var townNameCommand = new SqlCommand(townNameQuery, sqlConnection))
                {
                    townNameCommand.Parameters.AddWithValue("@townName", minionTownName);
                    var townIdAsQueryResult = townNameCommand.ExecuteScalar();
                    if (townIdAsQueryResult == null)
                    {
                        string townInsertQuery = "INSERT INTO Towns (Name) VALUES (@townName)";
                        using (var townInsertCommand = new SqlCommand(townInsertQuery, sqlConnection))
                        {
                            townInsertCommand.Parameters.AddWithValue("@townName", minionTownName);
                            townInsertCommand.ExecuteNonQuery();
                        }
                        Console.WriteLine($"Town {minionTownName} was added to the database.");
                    }
                    townIdAsQueryResult = townNameCommand.ExecuteScalar();
                    int townId = (int)townIdAsQueryResult;

                    string villainNameQuery = "SELECT Id FROM Villains WHERE Name = @Name";
                    using (var villainNameCommand = new SqlCommand(villainNameQuery, sqlConnection))
                    {
                        villainNameCommand.Parameters.AddWithValue("@Name", villainName);
                        var villainIdAsQueryResult = villainNameCommand.ExecuteScalar();
                        if (villainIdAsQueryResult == null)
                        {
                            string villainInsertQuery = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                            using (var villainInsertCommand = new SqlCommand(villainInsertQuery, sqlConnection))
                            {
                                villainInsertCommand.Parameters.AddWithValue("@villainName", villainName);
                                villainInsertCommand.ExecuteNonQuery();
                            }
                            Console.WriteLine($"Villain {villainName} was added to the database.");
                        }
                        villainIdAsQueryResult = villainNameCommand.ExecuteScalar();
                        int villainId = (int)villainIdAsQueryResult;

                        string minionNameQuery = "SELECT Id FROM Minions WHERE Name = @Name";
                        using (var minionNameCommand = new SqlCommand(minionNameQuery, sqlConnection))
                        {
                            minionNameCommand.Parameters.AddWithValue("@Name", minionName);
                            var minionIdAsQueryResult = minionNameCommand.ExecuteScalar();
                            if (minionIdAsQueryResult == null)
                            {
                                string minionInsertQuery = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
                                using (var minionInsertCommand = new SqlCommand(minionInsertQuery, sqlConnection))
                                {
                                    minionInsertCommand.Parameters.AddWithValue("@nam", minionName);
                                    minionInsertCommand.Parameters.AddWithValue("@age", minionAge);
                                    minionInsertCommand.Parameters.AddWithValue("@townId", townId);
                                    minionInsertCommand.ExecuteNonQuery();
                                }
                            }
                            minionIdAsQueryResult = minionNameCommand.ExecuteScalar();
                            int minionId = (int)minionIdAsQueryResult;

                            string minionAsVillianServantQuery = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
                            using (var minionAsVillianServantQueryResult = new SqlCommand(minionAsVillianServantQuery, sqlConnection))
                            {
                                minionAsVillianServantQueryResult.Parameters.AddWithValue("@minionId", minionId);
                                minionAsVillianServantQueryResult.Parameters.AddWithValue("@villainId", villainId);
                                minionAsVillianServantQueryResult.ExecuteNonQuery();
                            }
                            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                        }
                    }
                }
            }
        }
    }
}
