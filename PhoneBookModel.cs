using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


// Create the database with EntityFramework

namespace PhoneBook
{
    public class PhoneBookContext : DbContext
    {
        public DbSet<PhoneBookTable> PhoneBooks { get; set; } //Collection

        public string DbPath { get; }

        public PhoneBookContext() // Path to the database and the name of the .db file (phoneBook.db)
        {
            DbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "phoneBook.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    }


    public class PhoneBookTable //The columns in the Database
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

}