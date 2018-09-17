using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace Crow.Model
{
    public class Server
    {
        public ulong ServerID { get; set; }
        public char CommandPrefix { get; set; }
        public List<string> FaqList { get; set; }
        public ulong ModeratorID { get; set; }

        public bool ShouldLog { get; set; }
        public ulong LogChannelID { get; set; }

        public Server(ulong serverID, char commandPrefix, List<string> faqList, ulong moderatorID, bool shouldLog, ulong logChannelID)
        {
            ServerID = serverID;
            CommandPrefix = commandPrefix;
            FaqList = faqList;
            ModeratorID = moderatorID;
            ShouldLog = shouldLog;
            LogChannelID = logChannelID;
        }

        public string GuildName()
        {
            return Crow.Instance.Client.GetGuild(ServerID).Name;
        }
    }
}