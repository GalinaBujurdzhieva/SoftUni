using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _05.ChangeTownNamesCasing
{
    class Program
    {
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            string countryNameInput = Console.ReadLine();
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string countryNameQuery = @"SELECT t.Name 
                                            FROM Towns as t
                                            JOIN Countries AS c ON c.Id = t.CountryCode
                                            WHERE c.Name = @countryName";
                using (var townNameCommand = new SqlCommand(countryNameQuery, sqlConnection))
                {
                    townNameCommand.Parameters.AddWithValue("@countryName", countryNameInput);

                    using (var townNamesReader = townNameCommand.ExecuteReader())
                    {
                        if (!townNamesReader.HasRows)
                        {
                            Console.WriteLine("No town names were affected.");
                            return;
                        }
                    }

                    string updateTownNamesQuery = @"UPDATE Towns
                                                    SET Name = UPPER(Name)
                                                    WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                    using (var townNamesUpdateCommand = new SqlCommand(updateTownNamesQuery, sqlConnection))
                    {
                        townNamesUpdateCommand.Parameters.AddWithValue("@countryName", countryNameInput);
                        using (var updatedTownNamesReader = townNameCommand.ExecuteReader())
                        {
                            List<string> updatedTowns = new List<string>();
                            while (updatedTownNamesReader.Read())
                            {
                                string updatedTownName = (string)updatedTownNamesReader["Name"];
                                updatedTowns.Add(updatedTownName);
                            }
                            Console.WriteLine($"{updatedTowns.Count} town names were affected.");
                            Console.WriteLine($"[{string.Join(", ", updatedTowns)}]");
                        }
                    }
                }

            }
        }
    }
}
