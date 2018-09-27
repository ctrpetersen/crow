using Discord.Commands;

namespace Crow.Commands
{
    public class LinkMod
    {
        [Command("linkmod")]
        [Summary("Finds a mod from parameter. If the mod name contains spaces, it must be wrapped in quotes.")]
        [Alias("findmod, mod")]
        public void LinkModCommand(string name)
        {

        }
    }
}