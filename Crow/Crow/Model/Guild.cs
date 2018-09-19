using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace Crow.Model
{
    public enum AnnounceEnum
    {
        None,
        Here,
        Everyone
    }

    public class Guild
    {
        public ulong GuildID { get; set; }
        public char CommandPrefix { get; set; }
        public Dictionary<string, string> FaqDict { get; set; }
        public List<ulong> ModeratorRoleIDs { get; set; }

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

        public override string ToString()
        {
            return Crow.Instance.Client.GetGuild(GuildID).Name;
        }
    }
}