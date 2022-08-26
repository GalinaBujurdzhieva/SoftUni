using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _07.Print_All_Minion_Names
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string minionsNameQuery = @"SELECT Name FROM Minions";
                List<string> minionsNames = new List<string>();
                
                using (var minionsNamesCommand = new SqlCommand(minionsNameQuery, sqlConnection))
                {
                    using (var reader = minionsNamesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string minionName = (string)reader["Name"];
                            minionsNames.Add(minionName);
                        }

                        for (int i = 0; i < minionsNames.Count / 2; i++)
                        {
                            Console.WriteLine(minionsNames[i]);
                            Console.WriteLine(minionsNames[minionsNames.Count - 1 - i]);
                        }
                        if (minionsNames.Count % 2 == 1)
                        {
                            Console.WriteLine(minionsNames[minionsNames.Count / 2]);
                        }
                    }
                }
            }
         
        }
    }
}
