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
                SetPlaying("!help");

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

#region events

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

#endregion

#region database handling

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
                Guild guild = CrowContext.Guilds.Find(socketGuild.Id.ToString());
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

        private void CheckForGuildsNotInDB()
        {
            if (Client.Guilds.Count != 0)
            {
                foreach (var socketGuild in Client.Guilds)
                {
                    if (!CrowContext.Guilds.Any(g => g.GuildId == socketGuild.Id.ToString()))
                    {
                        Log(new LogMessage(LogSeverity.Warning, "Database",
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

#endregion

#region tasks

        private void SetPlaying(string message, ActivityType activityType = ActivityType.Playing)
        {
            Client.SetGameAsync($"{message} | {Client.Guilds.Sum(guild => guild.Users.Count) - 1} users, {Client.Guilds.Count} guilds");
        }

#endregion

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            var msg = socketMessage as SocketUserMessage;
            if (msg == null) return;
            if (msg.Author.IsBot) return;

            var channel = msg.Channel as SocketGuildChannel;
            if (channel == null) return;

            var guild = CrowContext.Guilds.Find(channel.Guild.Id.ToString());

            char charPrefix = Convert.ToChar(guild.CommandPrefix);

            int argPos = 0;
            if (msg.HasCharPrefix(charPrefix, ref argPos) || msg.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(Client, msg);
                await CommandService.ExecuteAsync(context, argPos, _services);
                //if (!result.IsSuccess)
                //    await Log(new LogMessage(LogSeverity.Error, "CommandService", result.ErrorReason));
            }
        }

        public static Task Log(LogMessage logmsg)
        {
            switch (logmsg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now} [{logmsg.Severity,8}] {logmsg.Source}: {logmsg.Message} {logmsg.Exception}");
            Console.ResetColor();
            return Task.CompletedTask;
        }

    }
}