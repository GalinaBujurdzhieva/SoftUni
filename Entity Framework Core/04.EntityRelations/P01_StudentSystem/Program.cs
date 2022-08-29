using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var myContext = new StudentSystemContext();
            myContext.Database.EnsureDeleted();
            myContext.Database.EnsureCreated();
        }
    }
}
