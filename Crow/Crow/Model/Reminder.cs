using System;
using Discord;
using Discord.WebSocket;

namespace Crow.Model
{
    public class Reminder
    {
        public int ReminderID { get; set; }
        public string Content { get; set; }
        public DateTime WhenToRemind { get; set; }
        public string AuthorID { get; set; }
        public string ChannelID { get; set; }
        public string GuildID { get; set; }
    }
}