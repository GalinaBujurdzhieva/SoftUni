﻿using System;
using Microsoft.Data.SqlClient;

namespace EntityFrameworkCore_ADO.NET
{
    class Program
    {
        const string connectionStringDBMaster = "Server=.;Integrated Security=true;Database=master;TrustServerCertificate=True";
        const string connectionStringDBMinions = "Server=.;Integrated Security=true;Database=MinionsDB;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            CreateMinionsDB();
            CreateTablesAnInsertDataInMinionsDB();
        }

        private static void CreateMinionsDB()
        {
            using (var sqlConnection = new SqlConnection(connectionStringDBMaster))
            {
                sqlConnection.Open();
                string sqlCreateDbCommand = "CREATE DATABASE MinionsDB";

                using (var sqlCreateDbQuery = new SqlCommand(sqlCreateDbCommand, sqlConnection))
                {
                    sqlCreateDbQuery.ExecuteNonQuery();
                }
            }
        }

        private static void CreateTablesAnInsertDataInMinionsDB()
        {
            using (var sqlConnection = new SqlConnection(connectionStringDBMinions))
            {
                sqlConnection.Open();
                string[] sqlCreateTablesInMinionsDBCommand = new string[]
                {
                    "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",
                    "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                    "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                    "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",
                    "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                    "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))"
                };

                string[] insertDataInMinionsDBCommand = new string[]
                {
                    "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",
                    "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",
                    "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",
                    "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",
                    "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)",
                    "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)"
                };

                foreach (var createTableQuery in sqlCreateTablesInMinionsDBCommand)
                {
                    using (var sqlCreateTableQuery = new SqlCommand(createTableQuery, sqlConnection))
                    {
                        sqlCreateTableQuery.ExecuteNonQuery();
                    }
                }
                foreach (var insertDataQuery in insertDataInMinionsDBCommand)
                {
                    using (var sqlInsertTableQuery = new SqlCommand(insertDataQuery, sqlConnection))
                    {
                        sqlInsertTableQuery.ExecuteNonQuery();
                    }
                }
            }
        }

        
    }
}
