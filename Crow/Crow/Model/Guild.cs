﻿using System;
using System.Collections.Generic;

namespace Crow.Model
{
    public partial class Guild
    {
        public Guild()
        {
            Faqs = new HashSet<Faq>();
            Reminders = new HashSet<Reminder>();
        }

        public string GuildId { get; set; }
        public string CommandPrefix { get; set; }
        public string ServerOwnerId { get; set; }
        public bool ShouldLog { get; set; }
        public string LogChannelId { get; set; }
        public bool ShouldTrackTwitch { get; set; }
        public string LiveRoleId { get; set; }
        public bool ShouldAnnounceUpdates { get; set; }
        public int AnnounceType { get; set; }
        public string UpdateChannelId { get; set; }
        public bool ShouldAnnounceRedditPosts { get; set; }
        public string RedditFeedChannelId { get; set; }

        public ICollection<Faq> Faqs { get; set; }
        public ICollection<Reminder> Reminders { get; set; }
    }
}
