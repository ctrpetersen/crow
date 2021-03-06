﻿using System;
using System.Linq;
using System.Security.Policy;
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
        [Summary(
            "`moderator only`\nViews all the config options for this guild and the current values, optionally allowing you to change them.\n" +
            "*Usage:* !config to view all the options, !config <setting name> <setting value> to change one, e.g. !config ShouldLog true or !config LiveRole @RoleMention" +
            "(alternatively just the name of it. Remember to wrap it in quotes if it contains a space!\n" +
            "The announcement ping type can be: None, Here or Everyone.")]
        [Alias("settings", "configuration", "setting")]
        public async Task ConfigCommand(string optionToChange = null, string optionValue = null)
        {
            if (!Crow.Instance.UserIsMod(Context))
            {
                await ReplyAsync($"Error - {Context.User.Username} is not a moderator.");
                return;
            }

            //param
            if (optionToChange != null)
            {
                if (optionValue == null)
                {
                    await ReplyAsync($"Error - you must assign a value to the option {optionToChange}");
                    return;
                }

                optionToChange = optionToChange.ToLower();


                //big switch... heck.
                switch (optionToChange)
                {
                    case "commandprefix":
                    case "prefix":
                    case "changeprefix":
                        if (IsSingleChar(optionValue) != null)
                        {
                            await PrefixCommand(optionValue);
                            return;
                        }

                        await ReplyAsync($"Error - Incorrect parameter for setting `prefix`.");
                        return;

                    case "shouldlog":
                    case "log":
                        bool? logBool = IsBool(optionValue);

                        switch (logBool)
                        {
                            case null:
                                await ReplyAsync($"Error - Incorrect parameter for setting `shouldlog`.");
                                return;
                            case true:
                            case false:
                                bool shouldLogBool = (bool) logBool;
                                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                                guild.ShouldLog = shouldLogBool;
                                Crow.Instance.CrowContext.Guilds.Update(guild);
                                Crow.Instance.CrowContext.SaveChanges();
                                await ReplyAsync($"Successfully changed `shouldlog` to `{logBool}`");
                                return;
                            default:
                                return;
                        }

                    case "logchannel":
                    case "logchannelid":
                        var logChannel = IsChannel(optionValue);
                        if (logChannel != null)
                        {
                            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                            guild.LogChannelId = logChannel.Id.ToString();
                            Crow.Instance.CrowContext.Guilds.Update(guild);
                            Crow.Instance.CrowContext.SaveChanges();
                            await ReplyAsync($"Successfully changed `logchannel` to `{logChannel.Name}`");
                            return;
                        }

                        await ReplyAsync($"Error - Incorrect parameter for setting `logchannel`.");
                        return;

                    case "shouldtracktwitch":
                    case "tracktwitch":
                        bool? twitchBool = IsBool(optionValue);

                        switch (twitchBool)
                        {
                            case null:
                                await ReplyAsync($"Error - Incorrect parameter for setting `shouldtracktwitch`.");
                                return;
                            case true:
                            case false:
                                bool shouldLogBool = (bool) twitchBool;
                                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                                guild.ShouldLog = shouldLogBool;
                                Crow.Instance.CrowContext.Guilds.Update(guild);
                                Crow.Instance.CrowContext.SaveChanges();
                                await ReplyAsync($"Successfully changed `shouldtracktwitch` to `{twitchBool}`");
                                return;
                            default:
                                return;
                        }

                    case "liverole":
                    case "liveroleid":
                        var role = IsRole(optionValue);
                        if (role != null)
                        {
                            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                            guild.LiveRoleId = role.Id.ToString();
                            Crow.Instance.CrowContext.Guilds.Update(guild);
                            Crow.Instance.CrowContext.SaveChanges();
                            await ReplyAsync($"Successfully changed `liverole` to `{role.Name}`");
                            return;
                        }

                        await ReplyAsync($"Error - Incorrect parameter for setting `liverole`.");
                        return;

                    case "shouldannounceupdates":
                    case "announceupdates":
                        bool? announceUpdatesBool = IsBool(optionValue);

                        switch (announceUpdatesBool)
                        {
                            case null:
                                await ReplyAsync($"Error - Incorrect parameter for setting `shouldannounceupdates`.");
                                return;
                            case true:
                            case false:
                                bool shouldAnnounceUpdates = (bool) announceUpdatesBool;
                                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                                guild.ShouldAnnounceUpdates = shouldAnnounceUpdates;
                                Crow.Instance.CrowContext.Guilds.Update(guild);
                                Crow.Instance.CrowContext.SaveChanges();
                                await ReplyAsync(
                                    $"Successfully changed `shouldannounceupdates` to `{announceUpdatesBool}`");
                                return;
                            default:
                                return;
                        }

                    case "announcetype":
                    case "announcementtype":
                        var announcementType = IsAnnouncementTypeEnum(optionValue);
                        if (announcementType != null)
                        {
                            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                            guild.AnnounceType = (int) announcementType;
                            Crow.Instance.CrowContext.Guilds.Update(guild);
                            Crow.Instance.CrowContext.SaveChanges();
                            await ReplyAsync($"Successfully changed `announcetype` to `{announcementType}`");
                            return;
                        }
                        await ReplyAsync($"Error - Incorrect parameter for setting `announcetype`.");
                        return;

                    case "updatechannel":
                    case "announcementchannel":
                    case "updatechannelid":
                    case "announcementchannelid":
                        var updateChannel = IsChannel(optionValue);
                        if (updateChannel != null)
                        {
                            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                            guild.UpdateChannelId = updateChannel.Id.ToString();
                            Crow.Instance.CrowContext.Guilds.Update(guild);
                            Crow.Instance.CrowContext.SaveChanges();
                            await ReplyAsync($"Successfully changed `updatechannel` to `{updateChannel.Name}`");
                            return;
                        }

                        await ReplyAsync($"Error - Incorrect parameter for setting `updatechannel`.");
                        return;

                    case "shouldannounceredditposts":
                    case "announceredditposts":
                    case "redditfeed":
                        bool? redditBool = IsBool(optionValue);

                        switch (redditBool)
                        {
                            case null:
                                await ReplyAsync($"Error - Incorrect parameter for setting `shouldlog`.");
                                return;
                            case true:
                            case false:
                                bool shouldRedditBool = (bool)redditBool;
                                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                                guild.ShouldAnnounceRedditPosts = shouldRedditBool;
                                Crow.Instance.CrowContext.Guilds.Update(guild);
                                Crow.Instance.CrowContext.SaveChanges();
                                await ReplyAsync($"Successfully changed `shouldannounceredditposts` to `{redditBool}`");
                                return;
                            default:
                                return;
                        }

                    case "redditfeedchannel":
                    case "redditfeedchannelid":
                    case "redditchannel":
                        var redditChannel = IsChannel(optionValue);
                        if (redditChannel != null)
                        {
                            var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                            guild.RedditFeedChannelId = redditChannel.Id.ToString();
                            Crow.Instance.CrowContext.Guilds.Update(guild);
                            Crow.Instance.CrowContext.SaveChanges();
                            await ReplyAsync($"Successfully changed `redditfeedchannel` to `{redditChannel.Name}`");
                            return;
                        }

                        await ReplyAsync($"Error - Incorrect parameter for setting `redditfeedchannel`.");
                        return;
                    default:
                        await ReplyAsync($"Error - No command named {optionToChange}");
                        return;
                }
            }

            //no param
            else
            {
                var guild = Crow.Instance.CrowContext.Guilds.Find(Context.Guild.Id.ToString());
                string reply = $"Configuration for {Context.Guild.Name}\n";

                reply += $"\n`CommandPrefix`  {guild.CommandPrefix}";
                reply += $"\n`ShouldLog`  {guild.ShouldLog}";

                if (guild.LogChannelId != "")
                    reply += $"\n`LogChannel`  {Context.Guild.GetChannel(Convert.ToUInt64(guild.LogChannelId)).Name}";
                else
                    reply += $"\n`LogChannel`  None specified";

                reply += $"\n`ShouldTrackTwitch`  {guild.ShouldTrackTwitch}";

                if (guild.LiveRoleId != "")
                    reply += $"\n`LiveRole`  {Context.Guild.GetRole(Convert.ToUInt64(guild.LiveRoleId)).Name}";
                else
                    reply += $"\n`LiveRole`  None specified";

                reply += $"\n`ShouldAnnounceUpdates`  {guild.ShouldAnnounceUpdates}";
                reply += $"\n`AnnounceType`  {(AnnouncementTypeEnum) guild.AnnounceType}";

                if (guild.UpdateChannelId != "")
                    reply +=
                        $"\n`UpdateChannel`  {Context.Guild.GetChannel(Convert.ToUInt64(guild.UpdateChannelId)).Name}";
                else
                    reply += $"\n`UpdateChannel`  None specified";

                reply += $"\n`ShouldAnnounceRedditPosts`  {guild.ShouldAnnounceRedditPosts}";

                if (guild.RedditFeedChannelId != "")
                    reply +=
                        $"\n`RedditFeedChannel`  {Context.Guild.GetChannel(Convert.ToUInt64(guild.RedditFeedChannelId)).Name}";
                else
                    reply += $"\n`RedditFeedChannel`  None specified";

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
            await Crow.Log(new LogMessage(LogSeverity.Info, "Config",
                $"Changed prefix for {Context.Guild.Name} to {prefix}."));

            await ReplyAsync($"Successfully changed prefix to `{prefix}`");
        }
        
        public bool? IsBool(string input)
        {
            switch (input.ToLower())
            {
                case "yes":
                case "true":
                    return true;
                case "no":
                case "false":
                    return false;
            }

            return null;
        }

        public SocketGuildChannel IsChannel(string input)
        {
            if (Context.Message.MentionedChannels.Count == 1)
            {
                var channel = Context.Message.MentionedChannels.ToList()[0];
                if (Crow.ClientCanSeeChannel(channel, Context.Guild))
                {
                    if (channel.GetType() == typeof(SocketTextChannel))
                        return channel;
                    ReplyAsync($"Error - {channel.Name} is not a text channel.");
                    return null;
                }

                ReplyAsync($"Error - Cannot send messages in channel {channel.Name}");
                return null;
            }

            if (input.Length == 18 && IsDigits(input))
            {
                var channel = Context.Guild.GetChannel(ulong.Parse(input));
                if (channel != null)
                {
                    if (Crow.ClientCanSeeChannel(channel, Context.Guild))
                        return channel;
                    ReplyAsync($"Error - Cannot send messages in channel {channel.Name}");
                    return null;
                }
            }

            return null;
        }

        public SocketRole IsRole(string input)
        {
            input = input.ToLower();

            if (Context.Message.MentionedRoles.Count == 1)
            {
                return Context.Message.MentionedRoles.ToList()[0];
            }

            return Context.Guild.Roles.FirstOrDefault(socketRole => socketRole.Name.ToLower() == input);
        }

        public AnnouncementTypeEnum? IsAnnouncementTypeEnum(string input)
        {
            input = input.ToLower();
            switch (input)
            {
                case "none":
                case "0":
                    return AnnouncementTypeEnum.None;
                case "here":
                case "1":
                    return AnnouncementTypeEnum.Here;
                case "everyone":
                case "2":
                    return AnnouncementTypeEnum.Everyone;
                default:
                    return null;
            }
        }

        public string IsSingleChar(string input)
        {
            return input.Length == 1 ? input : null;
        }


        private static bool IsDigits(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}