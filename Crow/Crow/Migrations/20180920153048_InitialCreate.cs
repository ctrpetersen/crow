using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Crow.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandPrefix = table.Column<char>(nullable: false),
                    ServerOwnerID = table.Column<ulong>(nullable: false),
                    ShouldLog = table.Column<bool>(nullable: false),
                    LogChannelID = table.Column<ulong>(nullable: false),
                    ShouldTrackTwitch = table.Column<bool>(nullable: false),
                    LiveRoleID = table.Column<ulong>(nullable: false),
                    ShouldAnnounceUpdates = table.Column<bool>(nullable: false),
                    AnnounceType = table.Column<int>(nullable: false),
                    UpdateChannelID = table.Column<ulong>(nullable: false),
                    ShouldAnnounceRedditPosts = table.Column<bool>(nullable: false),
                    RedditFeedChannelID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildID);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    FAQID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorID = table.Column<ulong>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    GuildID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.FAQID);
                    table.ForeignKey(
                        name: "FK_FAQs_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    ReminderID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    WhenToRemind = table.Column<DateTime>(nullable: false),
                    AuthorID = table.Column<ulong>(nullable: false),
                    ChannelID = table.Column<ulong>(nullable: false),
                    GuildID = table.Column<ulong>(nullable: false),
                    GuildID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.ReminderID);
                    table.ForeignKey(
                        name: "FK_Reminders_Guilds_GuildID1",
                        column: x => x.GuildID1,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_GuildID",
                table: "FAQs",
                column: "GuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_GuildID1",
                table: "Reminders",
                column: "GuildID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
