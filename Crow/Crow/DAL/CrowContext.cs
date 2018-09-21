using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Crow.Model;

namespace Crow.DAL
{
    public class CrowContext : DbContext
    {
        private string connectionString = Crow.Instance.jsonvars.db_connection_string.ToString();
        public CrowContext() : base("CrowContext")
        {

        }

        public DbSet<Guild_original> Guilds { get; set; }
        public DbSet<FAQ_original> FAQs { get; set; }
        public DbSet<Reminder_original> Reminders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelbuilder)
        {
            
        }
    }
}