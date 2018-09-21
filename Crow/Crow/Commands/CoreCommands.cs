using System.Threading.Tasks;
using Discord.Commands;

namespace Crow.Commands
{
    public class CoreCommands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Commands and information about the bot.")]
        public async Task HelpCommand()
        {
            await ReplyAsync($"I am Crow. Current commands: \nhelp \nyadda \nyadda");
        }
    }
}