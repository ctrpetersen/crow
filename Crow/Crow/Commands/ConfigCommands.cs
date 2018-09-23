using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crow.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crow.Commands
{
    public class ConfigCommands : ModuleBase<SocketCommandContext>
    {
        [Command("config")]
        [Summary("`moderator only`\nViews all the config options for this guild and the current values, optionally allowing you to change them.\n" +
                 "*Usage:* !config to view all the options, !config <setting name> <setting value> to change one, e.g. !config ShouldLog true or !config LiveRole @RoleMention.\n" +
                 "The announcement ping type can be: None, Here or Everyone.")]
        [Alias("settings", "configuration", "setting")]
        public async Task ConfigCommand(string optionToChange = null, string optionValue = null)
        {
            if (!Crow.Instance.UserIsMod(Context))
            {
                await ReplyAsync($"Error - {Context.User.Username} is not a moderator.");
                return;
            }

            var input = TryParseInput(optionValue).ToString();
            if (input == "invalid")
                await ReplyAsync($"{optionValue} - invalid");
            await ReplyAsync(input);
            return;

            //param
            if (optionToChange != null)
            {
                if (optionValue == null)
                {
                    await ReplyAsync($"Error - you must assign a value to the option {optionToChange}");
                    return;
                }
                //big ugly incoming :monkaGIGA:

                //find type of input - bool, 

                switch (optionToChange.ToLower())
                {
                    case "changeprefix":
                    case "prefix":
                    case "commandprefix":
                        await PrefixCommand(optionValue);
                        break;
                    case "shouldlog":
                    case "log":
                        break;


                    default:
                        await ReplyAsync($"Error - No option found named {optionToChange}");
                        break;
                }
            }
            else
            {
                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());

                //no param
                string reply = $"Configuration for {Context.Guild.Name}\n";

                reply += $"\n`CommandPrefix`   {guild.CommandPrefix}";
                reply += $"\n`ShouldLog`   {guild.ShouldLog}";

                if (guild.LogChannelId != "")
                    reply += $"\n`LogChannel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.LogChannelId)).Name}";
                else
                    reply += $"\n`LogChannel`   None specified";

                reply += $"\n`ShouldTrackTwitch`   {guild.ShouldTrackTwitch}";

                if (guild.LiveRoleId != "")
                    reply += $"\n`LiveRole`   {Context.Guild.GetRole(Convert.ToUInt64(guild.LiveRoleId)).Name}";
                else
                    reply += $"\n`LiveRole`   None specified";

                reply += $"\n`ShouldAnnounceUpdates`   {guild.ShouldAnnounceUpdates}";
                reply += $"\n`AnnounceType`   {(AnnouncementTypeEnum) guild.AnnounceType}";

                if (guild.UpdateChannelId != "")
                    reply +=
                        $"\n`UpdateChannel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.UpdateChannelId)).Name}";
                else
                    reply += $"\n`UpdateChannel`   None specified";

                reply += $"\n`ShouldAnnounceRedditPosts`   {guild.ShouldAnnounceRedditPosts}";

                if (guild.RedditFeedChannelId != "")
                    reply +=
                        $"\n`RedditFeedChannel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.RedditFeedChannelId)).Name}";
                else
                    reply += $"\n`RedditFeedChannel`   None specified";

                await ReplyAsync(reply);
            }
        }

        [Command("changeprefix")]
        [Summary("`moderator only`\nChanges prefix.\n*Usage:* !prefix CHARACTER")]
        [Alias("prefix", "commandprefix")]
        public async Task PrefixCommand(string prefix)
        {
            if (prefix.Length != 1)
            {
                await ReplyAsync($"Error - Incorrect parameter `{prefix}`. Only 1 character allowed.");
                return;
            }

            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
            guild.CommandPrefix = prefix;
            Crow.Instance.CrowContext.Guilds.Update(guild);
            Crow.Instance.CrowContext.SaveChanges();
            await Crow.Log(new LogMessage(LogSeverity.Info, "Config", $"Changed prefix for {Context.Guild.Name} to {prefix}."));

            await ReplyAsync($"Successfully changed prefix to `{prefix}`");
        }

        //returns string "invalid" if no valid input was able to be parsed
        private dynamic TryParseInput(string input)
        {
            input = input.ToLower();

            //yes/no
            if (input == "yes")
                return true;
            if (input == "no")
                return false;
            //true/false
            try
            {
                return bool.Parse(input);
            }
            catch (Exception e)
            {
                //not bool, try next parse
            }
            
            //is channel mention or ID - also check if client has permission to view
            if (Context.Message.MentionedChannels.Count == 1)
            {
                var channel = Context.Message.MentionedChannels.ToList()[0];
                if (Crow.Instance.ClientCanSeeChannel(channel, Context.Guild))
                {
                    if (channel.GetType() == typeof(SocketTextChannel))
                        return channel;
                    ReplyAsync($"Error - {channel.Name} is not a text channel.");
                    return "invalid";
                }
                ReplyAsync($"Error - Cannot send messages in channel {channel.Name}");
                return "invalid";
            }



            return "invalid";
        }
    }
}