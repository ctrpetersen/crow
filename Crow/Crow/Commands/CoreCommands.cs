using System.Threading.Tasks;
using Discord.Commands;

namespace Crow.Commands
{
    public class CoreCommands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Available commands.")]
        [Alias("commands")]
        public async Task HelpCommand()
        {
            string commands = "";
            foreach (var command in Crow.Instance.CommandService.Commands)
            {
                string aliases = string.Join(", ", command.Aliases).Replace($"{command.Name}, ", "");

                commands += $"\n`{command.Name}` \n({aliases}) \n{command.Summary}\n";
            }
            await ReplyAsync($"\n{commands}\n\nIf you need help or have any questions/feedback, contact Zirr#8008.");
        }

        [Command("info")]
        [Summary("Information about the bot.")]
        [Alias("information", "invite", "inv")]
        public async Task InfoCommand()
        {
            await ReplyAsync("Welcome to Crow. I am a bot for various helpful features, most of them geared towards Factorio servers." +
                             "\n\nYou can view my commands with (x)help, (x) being the prefix of this server (! by default.)." +
                             "\n\nIf you have any issues, questions, feedback or suggestions, contact me at Zirr#8008." +
                             "\n\nTo invite me to your server, use this link:" +
                             "\n\nhttps://discordapp.com/oauth2/authorize?client_id=491522696983871488&scope=bot&permissions=268954688" +
                             "\n\nThank you for using the bot!");
        }
    }
}