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
        [Summary("Views all the config options for this guild and the current values.")]
        [Alias("settings", "configuration")]
        public async Task ConfigCommand()
        {
            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
            string reply = $"Configuration for {Context.Guild.Name}\n";
                                                
            reply += $"\n`Command prefix`   {guild.CommandPrefix}";
            reply += $"\n`Should log`   {guild.ShouldLog}";

            if (guild.LogChannelId != "")
                reply += $"\n`Log channel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.LogChannelId)).Name}";
            else
                reply += $"\n`Log channel`   None specified";

            reply += $"\n`Should track twitch`   {guild.ShouldTrackTwitch}";

            if (guild.LiveRoleId != "")
                reply += $"\n`Live role`   {Context.Guild.GetRole(Convert.ToUInt64(guild.LiveRoleId)).Name}";
            else
                reply += $"\n`Live role`   None specified";

            reply += $"\n`Should announce updates`   {guild.ShouldAnnounceUpdates}";
            reply += $"\n`Announce type`   {(AnnouncementTypeEnum)guild.AnnounceType}";

            if (guild.UpdateChannelId != "")
                reply += $"\n`Update channel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.UpdateChannelId)).Name}";
            else
                reply += $"\n`Update channel`   None specified";

            reply += $"\n`Should announce reddit posts`   {guild.ShouldAnnounceUpdates}";

            if (guild.RedditFeedChannelId != "")
                reply += $"\n`Reddit feed channel`   {Context.Guild.GetChannel(Convert.ToUInt64(guild.RedditFeedChannelId)).Name}";
            else
                reply += $"\n`Reddit feed channel`   None specified";

            await ReplyAsync(reply);

        }

        [Command("changeprefix")]
        [Summary("`moderator only` Changes prefix.\nUsage: !prefix CHARACTER")]
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

        [Command("Log")]
        [Summary("`moderator only` \nLog new users joining/leaving in customisable channel.\nUsage: !log true/false (yes/no also accepted)")]
        [Alias("shouldlog")]
        public async Task LogCommand(string option)
        {
            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
            
        }
    }
}