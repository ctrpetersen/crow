using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        private dynamic jsonvars = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(@"C:\Repos\Projects\crow\Crow\Crow\private.json"));
        public SocketUser bot_owner;
        
        private IServiceProvider _services;


        public async Task StartAsync()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
            });
            CommandService = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await Client.LoginAsync(TokenType.Bot, jsonvars.discord_token.ToString());
            await Client.StartAsync();

            Client.Ready += () =>
            {
                bot_owner = Client.GetUser(ulong.Parse(jsonvars.bot_owner_id.ToString()));
                var total_users = Client.Guilds.Sum(guild => guild.Users.Count);
                Log(new LogMessage(LogSeverity.Info, "Crow",
                    $"{Client.CurrentUser.Username} is connected to " +
                    $"{Client.Guilds.Count} guild(s), serving a total of {total_users} online users."));
                Log(new LogMessage(LogSeverity.Info, "Crow",
                    $"Bot owner - {bot_owner.Username}#{bot_owner.Discriminator}"));
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {

        }

        public static Task Log(LogMessage logmsg)
        {
            Console.WriteLine(logmsg.ToString());
            return Task.CompletedTask;
        }

    }
}