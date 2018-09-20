﻿// <auto-generated />
using System;
using Crow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Crow.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20180920153048_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065");

            modelBuilder.Entity("Crow.Model.FAQ", b =>
                {
                    b.Property<int>("FAQID")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("AuthorID");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("GuildID");

                    b.HasKey("FAQID");

                    b.HasIndex("GuildID");

                    b.ToTable("FAQs");
                });

            modelBuilder.Entity("Crow.Model.Guild", b =>
                {
                    b.Property<int>("GuildID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnnounceType");

                    b.Property<char>("CommandPrefix");

                    b.Property<ulong>("LiveRoleID");

                    b.Property<ulong>("LogChannelID");

                    b.Property<ulong>("RedditFeedChannelID");

                    b.Property<ulong>("ServerOwnerID");

                    b.Property<bool>("ShouldAnnounceRedditPosts");

                    b.Property<bool>("ShouldAnnounceUpdates");

                    b.Property<bool>("ShouldLog");

                    b.Property<bool>("ShouldTrackTwitch");

                    b.Property<ulong>("UpdateChannelID");

                    b.HasKey("GuildID");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Crow.Model.Reminder", b =>
                {
                    b.Property<int>("ReminderID")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("AuthorID");

                    b.Property<ulong>("ChannelID");

                    b.Property<string>("Content");

                    b.Property<ulong>("GuildID");

                    b.Property<int?>("GuildID1");

                    b.Property<DateTime>("WhenToRemind");

                    b.HasKey("ReminderID");

                    b.HasIndex("GuildID1");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Crow.Model.FAQ", b =>
                {
                    b.HasOne("Crow.Model.Guild")
                        .WithMany("FAQs")
                        .HasForeignKey("GuildID");
                });

            modelBuilder.Entity("Crow.Model.Reminder", b =>
                {
                    b.HasOne("Crow.Model.Guild")
                        .WithMany("Reminders")
                        .HasForeignKey("GuildID1");
                });
#pragma warning restore 612, 618
        }
    }
}
