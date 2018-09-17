using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Crow
{
    public class Crow
    {
        public DiscordSocketClient Client;
        public CommandService CommandService;

        private IServiceProvider _services;
        private static Crow _instance;

        public static Crow Instance => _instance ?? (_instance = new Crow());

        private Crow() {}

        public async Task StartAsync()
        {
            Client = new DiscordSocketClient();
            CommandService = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await Client.LoginAsync(TokenType.Bot, "");
            await Client.StartAsync();

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

    }
}