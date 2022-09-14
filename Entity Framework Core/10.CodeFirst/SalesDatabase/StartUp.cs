using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using System;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContext = new SalesContext();
            dbContext.Database.Migrate();
        }
    }
}
