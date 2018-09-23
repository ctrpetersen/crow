using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace Crow.Commands
{
    public class CoreCommands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Lists all available commands, or just one. \n*Usage:* !help / !help <command name>")]
        [Alias("commands", "command")]
        public async Task HelpCommand(string commandParam = null)
        {
            string reply = "";
            //one command
            if (commandParam != null)
            {
                foreach (var command in Crow.Instance.CommandService.Commands)
                {
                    if (commandParam.ToLower() == command.Name.ToLower() || command.Aliases.Contains(commandParam.ToLower()))
                    {
                        string aliases = string.Join(", ", command.Aliases).Replace($"{command.Name}, ", "");
                        reply = $"\n__**{command.Name}**__ \n*({aliases})* \n{command.Summary}\n";
                        break;
                    }
                    reply = $"No command named {commandParam} found.";
                }
            }

            //all commands
            else
            {
                foreach (var command in Crow.Instance.CommandService.Commands)
                {
                    string aliases = string.Join(", ", command.Aliases).Replace($"{command.Name}, ", "");

                    reply += $"\n__**{command.Name}**__ \n*({aliases})* \n{command.Summary}\n";
                }
            }

            await ReplyAsync($"{reply}");
        }

        [Command("info")]
        [Summary("Information about the bot.")]
        [Alias("information", "invite", "inv")]
        public async Task InfoCommand()
        {
            await ReplyAsync("Welcome to Crow. I am a bot for various helpful features, most of them geared towards Factorio servers." +
                             "\n\nYou can view my commands with (x)help, (x) being the prefix of this server (! by default.)." +
                             "\nYou can also always summon me by mentioning me in place of the prefix." +
                             "\n\nIf you have any issues, questions, feedback or suggestions, contact me at Zirr#8008." +
                             "\nI am still a work-in-progress, so if you encounter any bugs or issues, please let me know." +
                             "\n\nTo invite me to your server, use this link:" +
                             "\n\n<https://discordapp.com/oauth2/authorize?client_id=491522696983871488&scope=bot&permissions=268954688>" +
                             "\n\nThe main server is discord.gg/factorio." +
                             "\n\nI am also open source! You can find it at <https://github.com/ctrpetersen/crow>" +
                             "\nPull requests are more than welcome." +
                             "\n\nThank you for using the bot!");
        }
    }
}