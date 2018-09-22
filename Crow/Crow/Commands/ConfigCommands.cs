using System;
using System.Threading.Tasks;
using Crow.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Crow.Commands
{
    public class ConfigCommands : ModuleBase<SocketCommandContext>
    {
        [Command("config")]
        [Summary("`moderator only`\nViews all the config options for this guild and the current values, optionally allowing you to change them.\n" +
                 "*Usage:* !config to view all the options, !config <setting name> <setting value> to change one, e.g. !config ShouldLog true or !config LiveRole @RoleMention.\n" +
                 "The announcement ping type can be: None, Here or Everyone.")]
        [Alias("settings", "configuration", "setting")]
        public async Task ConfigCommand(string optionToChange = null)
        {
            if (!Crow.Instance.UserIsMod(Context))
            {
                await ReplyAsync($"Error - {Context.User.Username} is not a moderator.");
                return;
            }

            //param
            if (optionToChange != null)
            {
                await ReplyAsync(optionToChange);
                return;
            }

            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());

            //no param
            string reply = $"Configuration for {Context.Guild.Name}\n";
                                                
            reply += $"\n`CommandPrefix`   {guild.CommandPrefix}";
            reply += $"\n`ShouldLog`   {guild.ShouldLog}";

            if (guild.LogChannelId != "")
                reply += $"\n`LogChannelId`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.LogChannelId)).Name}";
            else
                reply += $"\n`LogChannelId`   None specified";

            reply += $"\n`ShouldTrackTwitch`   {guild.ShouldTrackTwitch}";

            if (guild.LiveRoleId != "")
                reply += $"\n`LiveRoleId`   {Context.Guild.GetRole(Convert.ToUInt64(guild.LiveRoleId)).Name}";
            else
                reply += $"\n`LiveRoleId`   None specified";

            reply += $"\n`ShouldAnnounceUpdates`   {guild.ShouldAnnounceUpdates}";
            reply += $"\n`AnnounceType`   {(AnnouncementTypeEnum)guild.AnnounceType}";

            if (guild.UpdateChannelId != "")
                reply += $"\n`UpdateChannelId`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.UpdateChannelId)).Name}";
            else
                reply += $"\n`UpdateChannelId`   None specified";

            reply += $"\n`ShouldAnnounceRedditPosts`   {guild.ShouldAnnounceRedditPosts}";

            if (guild.RedditFeedChannelId != "")
                reply += $"\n`RedditFeedChannelId`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.RedditFeedChannelId)).Name}";
            else
                reply += $"\n`RedditFeedChannelId`   None specified";

            await ReplyAsync(reply);

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
    }
}