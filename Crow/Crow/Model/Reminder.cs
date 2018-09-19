using System;
using Discord;
using Discord.WebSocket;

namespace Crow.Model
{
    public class Reminder
    {
        public string Content { get; set; }
        public DateTime WhenToRemind { get; set; }
        public ulong AuthorID { get; set; }
        public ulong ChannelID { get; set; }
        public ulong GuildID { get; set; }
    }
}