using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Crow.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

//https://discordapp.com/oauth2/authorize?client_id=491522696983871488&scope=bot&permissions=268954688

namespace Crow
{
    public class Crow
    {
        public static Crow Instance => _instance ?? (_instance = new Crow());
        private static Crow _instance;
        private Crow() { }

        public DiscordSocketClient Client;
        public CommandService CommandService;

        public dynamic Jsonvars = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(@".\private.json"));
        public CrowContext CrowContext;
        public SocketUser BotOwner;
        
        private IServiceProvider _services;


        public async Task StartAsync()
        {
            CrowContext = new CrowContext();

            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
            });
            CommandService = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await Client.LoginAsync(TokenType.Bot, Jsonvars.discord_token.ToString());
            await Client.StartAsync();

            Client.JoinedGuild += JoinedGuild;
            Client.LeftGuild   += LeftGuild;

            Client.Ready += () =>
            {
                BotOwner = Client.GetUser(ulong.Parse(Jsonvars.bot_owner_id.ToString()));
                var total_users = Client.Guilds.Sum(guild => guild.Users.Count);
                Log(new LogMessage(LogSeverity.Info, "Crow",
                    $"{Client.CurrentUser.Username} is connected to " +
                    $"{Client.Guilds.Count} guild(s), serving a total of {total_users-1} users."));

                if (BotOwner != null)
                {
                    Log(new LogMessage(LogSeverity.Info, "Crow",
                        $"Bot owner - {BotOwner.Username}#{BotOwner.Discriminator}"));
                }

                CheckForGuildsNotInDB();
                Log(new LogMessage(LogSeverity.Info, "Database", $"{CrowContext.Guilds.Count()} guilds in database."));
                Log(new LogMessage(LogSeverity.Info, "Database", $"{CrowContext.Faqs.Count()} FAQs in database."));
                Log(new LogMessage(LogSeverity.Info, "Database", $"{CrowContext.Reminders.Count()} reminders in database."));

                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

#region events

        private Task JoinedGuild(SocketGuild socketGuild)
        {
            Log(new LogMessage(LogSeverity.Info, "Crow", $"Joined guild - {socketGuild.Name}"));
            AddGuildToDB(socketGuild);
            return Task.CompletedTask;
        }

        private Task LeftGuild(SocketGuild socketGuild)
        {
            Log(new LogMessage(LogSeverity.Info, "Crow", $"Left guild - {socketGuild.Name}"));

            if (CrowContext.Guilds.Any(g => g.GuildId == socketGuild.Id.ToString()))
            {
                Guild guild = new Guild {GuildId = socketGuild.Id.ToString()};
                CrowContext.Guilds.Attach(guild);
                CrowContext.Guilds.Remove(guild);
                CrowContext.SaveChanges();
                Log(new LogMessage(LogSeverity.Info, "Crow", $"Deleted guild {socketGuild.Name} from database."));
            }
            else
            {
                Log(new LogMessage(LogSeverity.Info, "Crow", $"No database entry for guild {socketGuild.Name}"));
            }

            return Task.CompletedTask;
        }

#endregion

        private void CheckForGuildsNotInDB()
        {
            if (Client.Guilds.Count != 0)
            {
                foreach (var socketGuild in Client.Guilds)
                {
                    if (!CrowContext.Guilds.Any(g => g.GuildId == socketGuild.Id.ToString()))
                    {
                        Log(new LogMessage(LogSeverity.Info, "Database",
                            $"No database entry for guild {socketGuild.Name}. Creating new entry..."));
                        AddGuildToDB(socketGuild);
                    }
                    else
                    {
                        Log(new LogMessage(LogSeverity.Info, "Crow", $"All guilds in database."));
                    }
                }
            }
        }

        private void AddGuildToDB(SocketGuild socketGuild)
        {
            Guild guild = new Guild();
            guild.GuildId = socketGuild.Id.ToString();
            guild.CommandPrefix = "!";
            guild.ServerOwnerId = socketGuild.OwnerId.ToString();
            guild.ShouldLog = false;
            guild.LogChannelId = "";
            guild.ShouldTrackTwitch = false;
            guild.LiveRoleId = "";
            guild.ShouldAnnounceUpdates = false;
            guild.AnnounceType = 0;
            guild.UpdateChannelId = "";
            guild.ShouldAnnounceRedditPosts = false;
            guild.RedditFeedChannelId = "";

            CrowContext.Guilds.Add(guild);
            CrowContext.SaveChanges();
        }

        private async Task HandleCommandAsync(SocketMessage SocketMessage)
        {
            if (!(SocketMessage is SocketUserMessage message)) return;
            int argumentPos = 0;


        }

        public static Task Log(LogMessage logmsg)
        {
            Console.WriteLine(logmsg.ToString());
            return Task.CompletedTask;
        }

    }
}