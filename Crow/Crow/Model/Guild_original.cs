using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace Crow.Model
{
    public enum AnnounceEnum
    {
        None = 0,
        Here = 1,
        Everyone = 2
    }

    public class Guild_original
    {
        public string GuildID { get; set; }
        public char CommandPrefix { get; set; }
        public string ServerOwnerID { get; set; }

        public bool ShouldLog { get; set; }
        public string LogChannelID { get; set; }

        public bool ShouldTrackTwitch { get; set; }
        public string LiveRoleID { get; set; }

        public bool ShouldAnnounceUpdates { get; set; } //FFFs and game updates
        public AnnounceEnum AnnounceType { get; set; }
        public string UpdateChannelID { get; set; }

        public bool ShouldAnnounceRedditPosts { get; set; }
        public string RedditFeedChannelID { get; set; }

        public ICollection<Reminder_original> Reminders { get; set; }
        public ICollection<FAQ_original> FAQs { get; set; }

        public override string ToString()
        {
            return Crow.Instance.Client.GetGuild(IdHelper.UlongConvert(GuildID)).Name;
        }
    }
}