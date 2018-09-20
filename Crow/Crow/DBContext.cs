using System.Collections.Generic;
using Crow.Model;
using Microsoft.EntityFrameworkCore;

namespace Crow
{
    public class DBContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=D:\\Dev\\Projects\\Crow\\Crow\\Crow\\Crow.db");
        }
    }
}