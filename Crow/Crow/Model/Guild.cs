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

    public class Guild
    {
        public int ID { get; set; }
        public char CommandPrefix { get; set; }
        public ulong ServerOwnerID { get; set; }

        public bool ShouldLog { get; set; }
        public ulong LogChannelID { get; set; }

        public bool ShouldTrackTwitch { get; set; }
        public ulong LiveRoleID { get; set; }

        public bool ShouldAnnounceUpdates { get; set; } //FFFs and game updates
        public AnnounceEnum AnnounceType { get; set; }
        public ulong UpdateChannelID { get; set; }

        public bool ShouldAnnounceRedditPosts { get; set; }
        public ulong RedditFeedChannelID { get; set; }

        public List<Reminder> Reminders { get; set; }
        public List<FAQ> FAQs { get; set; }

        public override string ToString()
        {
            return Crow.Instance.Client.GetGuild(Convert.ToUInt64(ID)).Name;
        }
    }
}