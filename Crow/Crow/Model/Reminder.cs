using System;
using System.Collections.Generic;

namespace Crow.Model
{
    public partial class Reminder
    {
        public int ReminderId { get; set; }
        public string Content { get; set; }
        public DateTime WhenToRemind { get; set; }
        public string AuthorId { get; set; }
        public string ChannelId { get; set; }
        public string GuildId { get; set; }

        public Guild Guild { get; set; }
    }
}
