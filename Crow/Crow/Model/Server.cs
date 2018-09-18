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

    public class Server
    {
        public ulong ServerID { get; set; }
        public char CommandPrefix { get; set; }
        public Dictionary<string, string> FaqDict { get; set; }
        public ulong ModeratorID { get; set; }

        public bool ShouldLog { get; set; }
        public ulong LogChannelID { get; set; }

        public bool ShouldTrackTwitch { get; set; }
        public ulong LiveRoleID { get; set; }

        public bool ShouldAnnounceUpdates { get; set; } //FFFs and game updates
        public AnnounceEnum AnnounceType { get; set; }
        public ulong UpdateChannelID { get; set; }

        public bool ShouldAnnounceRedditPosts { get; set; }
        public ulong RedditFeedChannelID { get; set; }


        public Server()
        {
            ShouldLog = false;
            ShouldTrackTwitch = false;
            ShouldAnnounceUpdates = false;
            ShouldAnnounceRedditPosts = false;
        }

        public override string ToString()
        {
            return Crow.Instance.Client.GetGuild(ServerID).Name;
        }
    }
}