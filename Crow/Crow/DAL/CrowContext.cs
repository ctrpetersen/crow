using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Crow.Model;

namespace Crow.DAL
{
    public class CrowContext : DbContext
    {
        public CrowContext() : base("CrowContext")
        {

        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelbuilder)
        {
            
        }
    }
}