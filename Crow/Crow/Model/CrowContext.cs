using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Crow.Model
{
    public partial class CrowContext : DbContext
    {
        private string connectionString = Crow.Instance.jsonvars.connection_string.ToString();

        public CrowContext()
        {
        }

        public CrowContext(DbContextOptions<CrowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Guild> Guilds { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQs");

                entity.Property(e => e.Faqid)
                    .HasColumnName("FAQID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuthorId)
                    .IsRequired()
                    .HasColumnName("AuthorID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GuildId)
                    .IsRequired()
                    .HasColumnName("GuildID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Guild)
                    .WithMany(p => p.Faqs)
                    .HasForeignKey(d => d.GuildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FAQ_GuildID");
            });

            modelBuilder.Entity<Guild>(entity =>
            {
                entity.Property(e => e.GuildId)
                    .HasColumnName("GuildID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CommandPrefix)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LiveRoleId)
                    .IsRequired()
                    .HasColumnName("LiveRoleID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LogChannelId)
                    .IsRequired()
                    .HasColumnName("LogChannelID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RedditFeedChannelId)
                    .IsRequired()
                    .HasColumnName("RedditFeedChannelID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServerOwnerId)
                    .IsRequired()
                    .HasColumnName("ServerOwnerID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateChannelId)
                    .IsRequired()
                    .HasColumnName("UpdateChannelID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.Property(e => e.ReminderId)
                    .HasColumnName("ReminderID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuthorId)
                    .IsRequired()
                    .HasColumnName("AuthorID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ChannelId)
                    .IsRequired()
                    .HasColumnName("ChannelID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.GuildId)
                    .IsRequired()
                    .HasColumnName("GuildID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WhenToRemind).HasColumnType("datetime");

                entity.HasOne(d => d.Guild)
                    .WithMany(p => p.Reminders)
                    .HasForeignKey(d => d.GuildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reminders_GuildID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
